namespace Pusbullet

open System
open FSharp.Data
open Newtonsoft.Json
open Newtonsoft.Json.Serialization

type LowercaseContractResolver () =
    inherit DefaultContractResolver ()
        override _.ResolvePropertyName (propertyName: string) =
            propertyName.ToLower()

module Program =

    [<Literal>]
    let PushbulletKey = "PUSHBULLET_KEY"

    let toJson a =
        let settings = JsonSerializerSettings()
        settings.ContractResolver <- (LowercaseContractResolver() :> IContractResolver)
        JsonConvert.SerializeObject(a, settings)

    let setKey key =
        Environment.SetEnvironmentVariable(PushbulletKey, key, EnvironmentVariableTarget.User)

    let getKey =
        Environment.GetEnvironmentVariable(PushbulletKey, EnvironmentVariableTarget.User)

    let push (a: string) =
        let pushUri = "https://api.pushbullet.com/v2/pushes"
        let header = [("Access-Token", getKey); (HttpRequestHeaders.ContentType "application/json")]
        Http.RequestString(pushUri, httpMethod = "POST", headers = header, body = TextRequest a ) |> ignore

    let pushText body =
        {| Type = "note"; Body = body |} |> toJson |> push

    let pushNote (title: string option) (body: string option) =
        let title = defaultArg title ""
        let body = defaultArg body ""
        {| Type = "note"; Title = title; Body = body |} |> toJson |> push

    let pushLink (url: string) (title: string option) (body: string option) =
        let title = defaultArg title ""
        let body = defaultArg body ""
        {| Type = "link"; Url = url; Title = title; Body = body |} |> toJson |> push

    [<EntryPoint>]
    let main ([<ParamArray>] argv: string[]): int =
        let command = argv.[0]

        let breakup = getKey = "" && command <> "--set-key"

        if not breakup then
            match command with
            | "-k" | "--set-key" -> setKey argv.[1]
            | "-t" | "--text" -> pushText argv.[1]
            /// Add more commands.
            | _ -> Console.WriteLine("Command not found!")
        else
            Console.WriteLine("You have to set your API key with: \"--set-key o.Abc12345xyz\" ")

        0