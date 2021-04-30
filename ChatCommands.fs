namespace Pushbullet

open System.Net
open FSharp.Data

module ChatCommands =

    [<Literal>]
    let ChatUrl = "https://api.pushbullet.com/v2/chats"

    let list =
        try
            Http.RequestString(ChatUrl, headers = SystemCommands.header, query = [("active", "true")]) |> CommandHelper.prettifyJson
        with
        | :? WebException as ex -> $"{ex.Message}"

    let delete id =
        try
            Http.RequestString($"{ChatUrl}/{id}", httpMethod = "DELETE", headers = SystemCommands.header) |> ignore
            "Chat deleted!"
        with
        | :? WebException as ex -> $"{ex.Message}"
