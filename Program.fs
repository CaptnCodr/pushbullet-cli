open System
open FSharp.Data

let setKey key =
    Environment.SetEnvironmentVariable("PUSHBULLET_KEY", key, EnvironmentVariableTarget.User)

let getKey =
    Environment.GetEnvironmentVariable("PUSHBULLET_KEY", EnvironmentVariableTarget.User)


let push (a: string) =
    let pushUri = "https://api.pushbullet.com/v2/pushes"
    let header = [("Access-Token", getKey); (HttpRequestHeaders.ContentType "application/json")]

    Http.RequestString(pushUri, httpMethod = "POST", headers = header, body = TextRequest a ) |> ignore

let pushText body =
    (sprintf "{ \"type\": \"note\", \"title\": \" \", \"body\": \"%s\" }" body) |> push

let pushNote (title: string option) (body: string option) =
    let title = defaultArg title ""
    let body = defaultArg body ""
    (sprintf "{ \"type\": \"note\", \"title\": \"%s\", \"body\": \"%s\" }" title body) |> push

let pushLink (url: string) (title: string option) (body: string option) =
    let title = defaultArg title ""
    let body = defaultArg body ""
    (sprintf "{ \"type\": \"link\", \"url\": \"%s\", \"title\": \"%s\", \"body\": \"%s\" }" url title body) |> push

[<EntryPoint>]
let main ([<ParamArray>] argv: string[]): int =
    let command = argv.[0]

    let breakup = getKey = "" && command <> "--set-key"

    if not breakup then
        match command with
        | "--set-key" -> setKey argv.[1]
        | "-t" | "--text" -> pushText argv.[1]
    else
        Console.WriteLine("You have to set your API key with: \"--set-key o.Abc12345xyz\" ")

    0 // return an integer exit code