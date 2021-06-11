namespace Pushbullet

open System.Net
open FSharp.Data

module ChatCommands =

    let list =
        try
            Http.RequestString($"{CommandHelper.BaseUrl}/chats", headers = SystemCommands.header, query = [("active", "true")]) |> CommandHelper.prettifyJson
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> CommandHelper.formatException

    let delete id =
        try
            Http.RequestString($"{CommandHelper.BaseUrl}/chats/{id}", httpMethod = "DELETE", headers = SystemCommands.header) |> ignore
            "Chat deleted!"
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> CommandHelper.formatException
