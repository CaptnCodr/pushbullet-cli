namespace Pushbullet

open System.Net
open FSharp.Data

module DeviceCommands =

    [<Literal>]
    let DeviceUrl = "https://api.pushbullet.com/v2/devices"

    let list =
        try
            Http.RequestString(DeviceUrl, headers = SystemCommands.header, query = [("active", "true")]) |> CommandHelper.prettifyJson
        with
        | :? WebException as ex -> $"{ex.Message}"

    let delete id =
        try
            Http.RequestString($"{DeviceUrl}/{id}", httpMethod = "DELETE", headers = SystemCommands.header) |> ignore
            "Device deleted!"
        with
        | :? WebException as ex -> $"{ex.Message}"
