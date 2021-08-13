namespace Pushbullet

open System
open Utilities

module PushCommands =

    let list limit =
        let formatPush (p: DataResponse.Push) =
            if p.Type.Equals "link" then
                $"[{p.Iden} at {p.Created |> unixTimestampToDateTime}] ({p.Type}) {p.Title}{Environment.NewLine}     URL: {p.Url.Value}{Environment.NewLine}     {p.Body}"
            else
                $"[{p.Iden} at {p.Created |> unixTimestampToDateTime}] ({p.Type}) {p.Title} {p.Body}"

        HttpService.GetRequest "pushes" [("limit", $"{limit}"); ("active", "true")]
        |> DataResponse.Parse
        |> fun r -> r.Pushes
        |> Array.map formatPush
        |> String.concat Environment.NewLine

    let getSinglePush id =
        HttpService.GetRequest $"pushes/{id}" []
        |> PushResponse.Parse
        |> fun p -> $"[{p.Iden}]:\nreceiver: {p.ReceiverEmail}\ncreated: {p.Created |> unixTimestampToDateTime}\nmodified: {p.Modified |> unixTimestampToDateTime}\ntarget device: {p.TargetDeviceIden}\ntype: {p.Type}\ntitle: {p.Title}\nbody: {p.Body}\nurl: {p.Url}"
        
    let delete id =
        HttpService.DeleteRequest $"pushes/{id}" "Push deleted!"

    let push (message: string) (json: 't) =
        HttpService.PostRequest "pushes" json message

    let pushText body device =
        {| Type = "note"; Body = body; Device_iden = device |} |> push "Push sent."

    let pushNote title body device =
        {| Type = "note"; Title = title; Body = body; Device_iden = device |} |> push "Push sent."

    let pushLink url title body device =
        {| Type = "link"; Url = url; Title = title; Body = body; Device_iden = device |} |> push "Link sent."

    let pushClip body =
        HttpService.PostRequest "ephemerals" {| Push = {| Body = body; Type = "clip" |}; Type = "push" |} "Clip Sent."