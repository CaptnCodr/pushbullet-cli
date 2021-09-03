namespace Pushbullet

open System
open Utilities

module ChatCommands =

    [<Literal>]
    let private Chats = "chats"

    let list () =
        HttpService.GetListRequest Chats
        |> DataResponse.Parse
        |> fun r -> r.Chats
        |> Array.map (fun c -> $"[{c.Iden}] with: {c.With.Email} per {c.With.Type} created at {c.Created |> unixTimestampToDateTime}, modified at {c.Modified |> unixTimestampToDateTime}")
        |> String.concat Environment.NewLine

    let delete id =
        HttpService.DeleteRequest $"{Chats}/{id}" "Chat deleted!"

    let update id status =
        let message = if status then "Chat muted." else "Chat unmuted."
        HttpService.PostRequest $"{Chats}/{id}" {| Muted = status |} message

    let create email =
        HttpService.PostRequest Chats {| Email = email |} "Chat created!"