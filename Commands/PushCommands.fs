namespace Pushbullet

open System.Net
open FSharp.Data
open System
open CommandHelper

module PushCommands =

    let list limit =
        let formatPush (p: DataResponse.Push) =
            if p.Type.Equals "link" then
                $"[{p.Iden} at {p.Created |> unixTimestampToDateTime}] ({p.Type}) {p.Title}{Environment.NewLine}     URL: {p.Url.Value}{Environment.NewLine}     {p.Body}"
            else
                $"[{p.Iden} at {p.Created |> unixTimestampToDateTime}] ({p.Type}) {p.Title} {p.Body}"

        try
            Http.RequestString($"{BaseUrl}/pushes", headers = SystemCommands.getHeader(), query = [("limit", $"{limit}"); Actives]) 
            |> DataResponse.Parse
            |> fun r -> r.Pushes
            |> Array.map formatPush
            |> String.concat Environment.NewLine
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException
        
    let delete id =
        try
            Http.RequestString($"{BaseUrl}/pushes/{id}", httpMethod = "DELETE", headers = SystemCommands.getHeader()) |> ignore
            "Push deleted!"
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException

    let push (json: string) (message: string) =
        try
            Http.RequestString($"{BaseUrl}/pushes", httpMethod = "POST", headers = SystemCommands.getHeader(), body = TextRequest json) |> ignore
            message
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException

    let pushText body device =
        {| Type = "note"; Body = body; Device_iden = device |> toValue |} |> toJson |> fun j -> push j "Push sent."

    let pushNote title body device =
        {| Type = "note"; Title = title |> toValue; Body = body |> toValue; 
            Device_iden = device |> toValue |} |> toJson |> fun j -> push j "Push sent."

    let pushLink url title body device =
        {| Type = "link"; Url = url; Title = title |> toValue; Body = body |> toValue; 
            Device_iden = device |> toValue |} |> toJson |> fun j -> push j "Link sent."

    let pushClip body =
        try
            let json = {| Push = {| Body = body; Type = "clip" |}; Type = "push" |} |> toJson
            Http.RequestString($"{BaseUrl}/ephemerals", httpMethod = "POST", headers = SystemCommands.getHeader(), body = TextRequest json) |> ignore
            "Clip Sent."
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException