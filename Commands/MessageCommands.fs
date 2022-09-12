namespace Pushbullet

open FSharp.Data
open Resources

module MessageCommands =

    type MessageResponse = JsonProvider<"./../Data/MessageData.json", ResolutionFolder=__SOURCE_DIRECTORY__>

    type SendMessageCommand = SendMessageCommand of device:string * number:string * body:string
    type DeleteMessageCommand = DeleteMessageCommand of string

    [<Literal>]
    let private Texts = "texts"

    let create (SendMessageCommand (device, number, body)) =
        {| Data = {| Target_Device_Iden = device; Addresses = [number]; Message = body |} |}
        |> fun json -> HttpService.PostRequest Texts json Empty
        |> MessageResponse.Parse
        |> fun r -> CreateMessageOutput.FormattedString(r.Iden, MessageSent)

    let delete (DeleteMessageCommand id) = 
        HttpService.PostRequest Texts {| Iden = id |} MessageDeleted
