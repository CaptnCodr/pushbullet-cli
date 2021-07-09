namespace Pushbullet

open System.Net
open FSharp.Data
open System
open CommandHelper

module DeviceCommands =

    let list () =
        try
            Http.RequestString($"{BaseUrl}/devices", headers = SystemCommands.getHeader(), query = [("active", "true")]) 
            |> DataResponse.Parse
            |> fun r -> r.Devices
            |> fun a -> [for i in 0 .. a.Length - 1 do $"{i} [{a.[i].Iden}] {a.[i].Nickname}"]
            |> String.concat Environment.NewLine
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException

    let getDeviceId (index: int) : string =
        try
            Http.RequestString($"{BaseUrl}/devices", headers = SystemCommands.getHeader(), query = [("active", "true")]) 
            |> DataResponse.Parse
            |> fun r -> r.Devices
            |> fun a -> if a.Length > index then a.[index].Iden else ""
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException

    let delete id =
        try
            Http.RequestString($"{BaseUrl}/devices/{id}", httpMethod = "DELETE", headers = SystemCommands.getHeader()) |> ignore
            "Device deleted!"
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException
