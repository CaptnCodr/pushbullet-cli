﻿namespace Pushbullet

open Resources

module MessageCommands =

    type SendMessageCommand = SendMessageCommand of device:string * number:string * body:string
    type DeleteMessageCommand = DeleteMessageCommand of string

    [<Literal>]
    let private Texts = "texts"

    let create (SendMessageCommand (device, number, body)) =
        {| Data = {| Target_Device_Iden = device; Addresses = [number]; Message = body |} |}
        |> fun json -> HttpService.PostRequest Texts json ""
        |> MessageResponse.Parse
        |> fun r -> $"[{r.Iden}] {MessageSent.ResourceString}"

    let delete (DeleteMessageCommand id) = 
        {| Iden = id |}
        |> fun json -> HttpService.PostRequest Texts json MessageDeleted.ResourceString