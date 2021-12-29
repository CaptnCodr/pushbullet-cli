namespace Pushbullet

open System
open Resources
open Utilities

module ChatCommands =

    type UpdateChatCommand = UpdateChatCommand of id:string * status:bool
    type CreateChatCommand = CreateChatCommand of string
    type DeleteChatCommand = DeleteChatCommand of string

    [<Literal>]
    let private Chats = "chats"

    let list () =
        HttpService.GetListRequest Chats
        |> DataResponse.Parse
        |> fun r -> r.Chats
        |> Array.map (fun c -> $"[{c.Iden}] with: {c.With.Email} per {c.With.Type} created at {c.Created |> unixTimestampToDateTime}, modified at {c.Modified |> unixTimestampToDateTime}")
        |> String.concat Environment.NewLine

    let delete (DeleteChatCommand id)=
        HttpService.DeleteRequest $"{Chats}/{id}" ChatDeleted

    let update (UpdateChatCommand(id, status)) =
        HttpService.PostRequest $"{Chats}/{id}" {| Muted = status |} [ChatUnmuted;ChatMuted].[status |> Convert.ToInt32]

    let create (CreateChatCommand email) =
        HttpService.PostRequest Chats {| Email = email |} ChatCreated