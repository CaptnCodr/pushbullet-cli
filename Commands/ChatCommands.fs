namespace Pushbullet

open System
open FSharp.Data
open Resources
open Extensions.DateTimeExtension

module ChatCommands =

    type ChatListResponse = JsonProvider<"./../Data/ChatList.json", ResolutionFolder=__SOURCE_DIRECTORY__>

    type UpdateChatCommand = UpdateChatCommand of id: string * status: bool
    type CreateChatCommand = CreateChatCommand of string
    type DeleteChatCommand = DeleteChatCommand of string

    [<Literal>]
    let private Chats = "chats"

    let list () =
        HttpService.GetListRequest Chats
        |> ChatListResponse.Parse
        |> _.Chats
        |> Array.map (fun c ->
            ChatListOutput.FormattedString(
                c.Iden,
                c.With.Email,
                c.With.Type,
                c.Created.ofUnixTimeToDateTime,
                c.Modified.ofUnixTimeToDateTime
            ))
        |> String.concat Environment.NewLine

    let delete (DeleteChatCommand id) =
        HttpService.DeleteRequest $"{Chats}/{id}" ChatDeleted

    let update (UpdateChatCommand(id, status)) =
        HttpService.PostRequest
            $"{Chats}/{id}"
            {| Muted = status |}
            [ ChatUnmuted; ChatMuted ].[status |> Convert.ToInt32]

    let create (CreateChatCommand email) =
        HttpService.PostRequest Chats {| Email = email |} ChatCreated
