namespace Pushbullet

open System.Net
open FSharp.Data
open System

module ChatCommands =

    let list =
        
        let formatChat (c: DataResponse.Chat) =
            $"With: {c.With.Email}"

        try
            Http.RequestString($"{CommandHelper.BaseUrl}/chats", headers = SystemCommands.getHeader(), query = [("active", "true")])
            |> DataResponse.Parse
            |> fun r -> r.Chats
            |> Array.map formatChat
            |> String.concat Environment.NewLine
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> CommandHelper.formatException

    let delete id =
        try
            Http.RequestString($"{CommandHelper.BaseUrl}/chats/{id}", httpMethod = "DELETE", headers = SystemCommands.getHeader()) |> ignore
            "Chat deleted!"
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> CommandHelper.formatException
