namespace Pushbullet

open Resources
open FSharp.Data
open System
open Extensions.DateTimeExtension

module PushCommands =

    type PushResponse = JsonProvider<"./../Data/PushData.json", ResolutionFolder=__SOURCE_DIRECTORY__>
    type PushListResponse = JsonProvider<"./../Data/PushList.json", ResolutionFolder=__SOURCE_DIRECTORY__>

    type PushTextCommand = PushTextCommand of body: string * deviceid: string option
    type PushNoteCommand = PushNoteCommand of title: string option * body: string option * deviceid: string option

    type PushLinkCommand =
        | PushLinkCommand of url: string * title: string option * body: string option * deviceid: string option

    type PushClipCommand = PushClipCommand of string
    type ListPushesCommand = ListPushesCommand of int
    type GetPushCommand = GetPushCommand of string
    type DeletePushCommand = DeletePushCommand of string

    [<Literal>]
    let private Pushes = "pushes"

    let list (ListPushesCommand limit) =
        let formatPush (p: PushListResponse.Push) =
            if p.Type.Equals "link" then
                ListLinkPushOutput.FormattedString(
                    p.Iden,
                    p.Created.ofUnixTimeToDateTime,
                    p.Type,
                    p.Title,
                    p.Url.Value,
                    p.Body
                )
            else
                ListTextPushOutput.FormattedString(p.Iden, p.Created.ofUnixTimeToDateTime, p.Type, p.Title, p.Body)

        HttpService.GetRequest Pushes [ ("limit", $"{limit}"); ("active", $"{true}") ]
        |> PushListResponse.Parse
        |> _.Pushes
        |> Array.map formatPush
        |> String.concat Environment.NewLine

    let getSinglePush (GetPushCommand id) =
        HttpService.GetRequest $"{Pushes}/{id}" []
        |> PushResponse.Parse
        |> fun p ->
            GetSinglePushOutput.FormattedString(
                p.Iden,
                p.ReceiverEmail,
                p.Created.ofUnixTimeToDateTime,
                p.Modified.ofUnixTimeToDateTime,
                p.TargetDeviceIden,
                p.Type,
                p.Title,
                p.Body,
                p.Url
            )

    let delete (DeletePushCommand id) =
        HttpService.DeleteRequest $"{Pushes}/{id}" PushDeleted

    let push (message: ResourceTypes) (json: 't) =
        HttpService.PostRequest "pushes" json message

    let pushText (PushTextCommand(body, device)) =
        {| Type = "note"
           Body = body
           Device_iden = device |}
        |> push PushSent

    let pushNote (PushNoteCommand(title, body, device)) =
        {| Type = "note"
           Title = title
           Body = body
           Device_iden = device |}
        |> push PushSent

    let pushLink (PushLinkCommand(url, title, body, device)) =
        {| Type = "link"
           Url = url
           Title = title
           Body = body
           Device_iden = device |}
        |> push LinkSent

    let pushClip (PushClipCommand body) =
        HttpService.PostRequest
            "ephemerals"
            {| Push = {| Body = body; Type = "clip" |}
               Type = "push" |}
            ClipSent
