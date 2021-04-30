namespace Pushbullet

open System.Net
open FSharp.Data

module PushCommands =

    [<Literal>]
    let PushUrl = "https://api.pushbullet.com/v2/pushes"

    let get (parameters: list<string * string>) =
        try
            Http.RequestString(PushUrl, headers = SystemCommands.header, query = parameters) |> CommandHelper.prettifyJson
        with
        | :? WebException as ex -> $"{ex.Message}"

    let push (json: string, message: string) =
        try
            Http.RequestString(PushUrl, httpMethod = "POST", headers = SystemCommands.header, body = TextRequest json) |> ignore
            message
        with
        | :? WebException as ex -> $"{ex.Message}"

    let delete (id: string) =
        try
            Http.RequestString($"{PushUrl}/{id}", httpMethod = "DELETE", headers = SystemCommands.header) |> ignore
            "Push deleted!"
        with
        | :? WebException as ex -> $"{ex.Message}"

    let pushText body =
        {| Type = "note"; Body = body |} |> CommandHelper.toJson |> fun j -> push (j, "Push sent.")

    let pushNote (title: string option) (body: string option) =
        let title = if title.IsNone then null else title.Value
        let body = if body.IsNone then null else body.Value
        {| Type = "note"; Title = title; Body = body |} |> CommandHelper.toJson |> fun j -> push (j, "Push sent.")

    let pushLink (url: string) (title: string option) (body: string option) =
        let title = if title.IsNone then null else title.Value
        let body = if body.IsNone then null else body.Value
        {| Type = "link"; Url = url; Title = title; Body = body |} |> CommandHelper.toJson |> fun j -> push (j, "Link sent.")

    let list (limit: int) =
        let limit = if limit <= 0 then "1" else $"{limit}"
        [("limit", limit)] |> get