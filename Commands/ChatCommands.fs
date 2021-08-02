namespace Pushbullet

open System
open CommandHelper

module ChatCommands =

    let list () =
        HttpService.GetRequest "chats" [Actives]
        |> DataResponse.Parse
        |> fun r -> r.Chats
        |> Array.map (fun c -> $"[{c.Iden}] with: {c.With.Email} per {c.With.Type} created at {c.Created |> unixTimestampToDateTime}, modified at {c.Modified |> unixTimestampToDateTime}")
        |> String.concat Environment.NewLine

    let delete id =
        HttpService.DeleteRequest $"chats/{id}" "Chat deleted!"

    let update id status =
        let json = {| Muted = status |} |> toJson
        let message = if status then "Chat muted." else "Chat unmuted."
        HttpService.PostRequest $"chats/{id}" json message

    let create email =
        let json = {| Email = email |} |> toJson
        HttpService.PostRequest "chats" json "Chat created!"