namespace Pushbullet

open System.Net
open FSharp.Data
open System
open CommandHelper

module ChatCommands =

    let list () =
        let formatChat (c: DataResponse.Chat) =
            $"[{c.Iden}] with: {c.With.Email} per {c.With.Type} created at {c.Created |> unixTimestampToDateTime}, modified at {c.Modified |> unixTimestampToDateTime}"

        try
            Http.RequestString($"{BaseUrl}/chats", headers = SystemCommands.getHeader(), query = [Actives])
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

    let update id status =
        try
            let json = {| Muted = status |} |> toJson
            Http.RequestString($"{BaseUrl}/chats/{id}", httpMethod = "POST", headers = SystemCommands.getHeader(), body = TextRequest json ) |> ignore
            if status then "Chat muted." else "Chat unmuted."
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException

    let create email =
        try
            let json = {| Email = email |} |> toJson
            Http.RequestString($"{BaseUrl}/chats", httpMethod = "POST", headers = SystemCommands.getHeader(), body = TextRequest json ) |> ignore
            "Chat created!"
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException