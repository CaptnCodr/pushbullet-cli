namespace Pushbullet

open System.Net
open FSharp.Data
open System

module PushCommands =

    let list (limit: int) =
        
        let formatPush (p: DataResponse.Push) =
            if p.Type.Equals "link" then
                $"URL: {p.Url.Value}{Environment.NewLine}    {p.Body}"
            else
                $"({p.Type}) {p.Body}"

        let limit = if limit <= 0 then "1" else $"{limit}"
        try
            Http.RequestString($"{CommandHelper.BaseUrl}/pushes", headers = SystemCommands.getHeader(), query = [("limit", limit)]) 
            |> DataResponse.Parse
            |> fun r -> r.Pushes
            |> Array.map formatPush
            |> String.concat Environment.NewLine
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> CommandHelper.formatException
        
    let delete (id: string) =
        try
            Http.RequestString($"{CommandHelper.BaseUrl}/pushes/{id}", httpMethod = "DELETE", headers = SystemCommands.getHeader()) |> ignore
            "Push deleted!"
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> CommandHelper.formatException

    let push (json: string, message: string) =
        try
            Http.RequestString($"{CommandHelper.BaseUrl}/pushes", httpMethod = "POST", headers = SystemCommands.getHeader(), body = TextRequest json) |> ignore
            message
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> CommandHelper.formatException

    let pushText body (device: string option) =
        let device = if device.IsNone then null else device.Value
        {| Type = "note"; Body = body; Device_iden = device |} |> CommandHelper.toJson |> fun j -> push (j, "Push sent.")

    let pushNote (title: string option) (body: string option) (device: string option) =
        let title = if title.IsNone then null else title.Value
        let body = if body.IsNone then null else body.Value
        let device = if device.IsNone then null else device.Value
        {| Type = "note"; Title = title; Body = body; Device_iden = device |} |> CommandHelper.toJson |> fun j -> push (j, "Push sent.")

    let pushLink (url: string) (title: string option) (body: string option) (device: string option) =
        let title = if title.IsNone then null else title.Value
        let body = if body.IsNone then null else body.Value
        let device = if device.IsNone then null else device.Value
        {| Type = "link"; Url = url; Title = title; Body = body; Device_iden = device |} |> CommandHelper.toJson |> fun j -> push (j, "Link sent.")
