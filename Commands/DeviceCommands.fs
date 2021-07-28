namespace Pushbullet

open System.Net
open FSharp.Data
open System
open CommandHelper

module DeviceCommands =

    let list () =
        try
            Http.RequestString($"{BaseUrl}/devices", headers = SystemCommands.getHeader(), query = [Actives]) 
            |> DataResponse.Parse
            |> fun r -> r.Devices
            |> Array.indexed |> Array.map (fun (i, e) -> $"{i} [{e.Iden}] {e.Nickname}")
            |> String.concat Environment.NewLine
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException

    let getDeviceInfo (iden: string) =
        try
            Http.RequestString($"{BaseUrl}/devices/{iden}", headers = SystemCommands.getHeader()) 
            |> DeviceResponse.Parse
            |> fun d -> $"[{d.Iden}]:\nName: {d.Nickname}\nDevice: {d.Manufacturer} {d.Model}\nAppVersion: {d.AppVersion}\nCreated: {d.Created |> unixTimestampToDateTime}\nModified: {d.Modified |> unixTimestampToDateTime}"
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException

    let getDeviceId (index: int) : string =
        try
            Http.RequestString($"{BaseUrl}/devices", headers = SystemCommands.getHeader(), query = [Actives]) 
            |> DataResponse.Parse
            |> fun r -> r.Devices
            |> fun a -> if a.Length > index then a.[index].Iden else String.Empty
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException

    let delete id =
        try
            Http.RequestString($"{BaseUrl}/devices/{id}", httpMethod = "DELETE", headers = SystemCommands.getHeader()) |> ignore
            "Device deleted!"
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException
