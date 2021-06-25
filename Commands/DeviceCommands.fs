namespace Pushbullet

open System.Net
open FSharp.Data
open System

module DeviceCommands =

    let list =

        let formatDevice (d: DataResponse.Devicis) =
            $"({d.Iden}) {d.Nickname}"

        try
            Http.RequestString($"{CommandHelper.BaseUrl}/devices", headers = SystemCommands.getHeader(), query = [("active", "true")]) 
            |> DataResponse.Parse
            |> fun r -> r.Devices
            |> Array.map formatDevice
            |> String.concat Environment.NewLine
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> CommandHelper.formatException

    let delete id =
        try
            Http.RequestString($"{CommandHelper.BaseUrl}/devices/{id}", httpMethod = "DELETE", headers = SystemCommands.getHeader()) |> ignore
            "Device deleted!"
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> CommandHelper.formatException
