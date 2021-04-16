namespace Pushbullet

open System.Net
open FSharp.Data

module DeviceCommands =

    [<Literal>]
    let DeviceUrl = "https://api.pushbullet.com/v2/devices"

    let listDevices =
        try
            Http.RequestString(DeviceUrl, headers = SystemCommands.header, query = [("active", "true")]) |> CommandHelper.prettifyJson
        with
        | :? WebException as ex -> $"{ex.Message}"