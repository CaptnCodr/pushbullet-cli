namespace Pushbullet

open System.Net
open FSharp.Data
open System
open CommandHelper

module ChatCommands =

    let list () =        
        let formatChat (c: DataResponse.Chat) =
            $"[{c.Iden} at {c.Created |> unixTimestampToDateTime}] with: {c.With.Email} per {c.With.Type}"

        try
            Http.RequestString($"{BaseUrl}/chats", headers = SystemCommands.getHeader(), query = [("active", "true")])
            |> DataResponse.Parse
            |> fun r -> r.Chats
            |> Array.map formatChat
            |> String.concat Environment.NewLine
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException

    let delete id =
        try
            Http.RequestString($"{BaseUrl}/chats/{id}", httpMethod = "DELETE", headers = SystemCommands.getHeader()) |> ignore
            "Chat deleted!"
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException
