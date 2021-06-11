namespace Pushbullet

open System.Net
open FSharp.Data

module PushCommands =

    let list (limit: int) =
        let limit = if limit <= 0 then "1" else $"{limit}"
        try
            Http.RequestString($"{CommandHelper.BaseUrl}/pushes", headers = SystemCommands.header, query = [("limit", limit)]) |> CommandHelper.prettifyJson
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> CommandHelper.formatException
        
    let delete (id: string) =
        try
            Http.RequestString($"{CommandHelper.BaseUrl}/pushes/{id}", httpMethod = "DELETE", headers = SystemCommands.header) |> ignore
            "Push deleted!"
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> CommandHelper.formatException

    let push (json: string, message: string) =
        try
            Http.RequestString($"{CommandHelper.BaseUrl}/pushes", httpMethod = "POST", headers = SystemCommands.header, body = TextRequest json) |> ignore
            message
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> CommandHelper.formatException

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
