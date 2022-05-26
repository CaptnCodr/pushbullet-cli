namespace Pushbullet

open System
open Resources
open Utilities

module DeviceCommands =

    type GetDeviceInfoCommand = GetDeviceInfoCommand of string
    type GetDeviceCommand = GetDeviceCommand of int
    type DeleteDeviceCommand = DeleteDeviceCommand of string

    [<Literal>]
    let private Devices = "devices"

    let list () =
        HttpService.GetListRequest Devices
        |> DataResponse.Parse
        |> fun r -> r.Devices
        |> Array.indexed |> Array.map (fun (i, e) -> DeviceListOutput.FormattedString(i, e.Iden, e.Nickname))
        |> String.concat Environment.NewLine

    let getDeviceInfo (GetDeviceInfoCommand iden) =
        HttpService.GetRequest $"{Devices}/{iden}" []
        |> DeviceResponse.Parse
        |> fun d -> GetDeviceOutput.FormattedString(d.Iden, d.Nickname, d.Manufacturer, d.Model, d.AppVersion, d.Created |> unixTimestampToDateTime, d.Modified |> unixTimestampToDateTime)

    let getDeviceId (GetDeviceCommand index) =
        HttpService.GetListRequest Devices
        |> DataResponse.Parse
        |> fun r -> r.Devices
        |> fun a -> a |> Array.tryItem index 
        |> function | Some v -> v.Iden | None -> ""

    let delete (DeleteDeviceCommand id) =
        HttpService.DeleteRequest $"{Devices}/{id}" DeviceDeleted
