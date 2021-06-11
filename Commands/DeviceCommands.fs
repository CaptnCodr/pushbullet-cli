namespace Pushbullet

open System.Net
open FSharp.Data

module DeviceCommands =

    let list =
        try
            Http.RequestString($"{CommandHelper.BaseUrl}/devices", headers = SystemCommands.header, query = [("active", "true")]) |> CommandHelper.prettifyJson
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> CommandHelper.formatException

    let delete id =
        try
            Http.RequestString($"{CommandHelper.BaseUrl}/devices/{id}", httpMethod = "DELETE", headers = SystemCommands.header) |> ignore
            "Device deleted!"
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> CommandHelper.formatException
