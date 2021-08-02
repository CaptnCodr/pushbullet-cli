namespace Pushbullet

open System
open CommandHelper

module PushCommands =

    let list limit =
        let formatPush (p: DataResponse.Push) =
            if p.Type.Equals "link" then
                $"[{p.Iden} at {p.Created |> unixTimestampToDateTime}] ({p.Type}) {p.Title}{Environment.NewLine}     URL: {p.Url.Value}{Environment.NewLine}     {p.Body}"
            else
                $"[{p.Iden} at {p.Created |> unixTimestampToDateTime}] ({p.Type}) {p.Title} {p.Body}"

        HttpService.GetRequest "pushes" [("limit", $"{limit}"); Actives]
        |> DataResponse.Parse
        |> fun r -> r.Pushes
        |> Array.map formatPush
        |> String.concat Environment.NewLine
        
    let delete id =
        HttpService.DeleteRequest $"pushes/{id}" "Push deleted!"

    let push (json: string) (message: string) =
        HttpService.PostRequest "pushes" json message

    let pushText body device =
        {| Type = "note"; Body = body; Device_iden = device |> toValue |} |> toJson |> fun j -> push j "Push sent."

    let pushNote title body device =
        {| Type = "note"; Title = title |> toValue; Body = body |> toValue; 
            Device_iden = device |> toValue |} |> toJson |> fun j -> push j "Push sent."

    let pushLink url title body device =
        {| Type = "link"; Url = url; Title = title |> toValue; Body = body |> toValue; 
            Device_iden = device |> toValue |} |> toJson |> fun j -> push j "Link sent."

    let pushClip body =
        let json = {| Push = {| Body = body; Type = "clip" |}; Type = "push" |} |> toJson
        HttpService.PostRequest "ephemerals" json "Clip Sent."