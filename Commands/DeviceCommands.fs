namespace Pushbullet

open System
open FSharp.Data
open Resources
open Extensions.DateTimeExtension

module DeviceCommands =

    type DeviceResponse = JsonProvider<"./../Data/DeviceData.json", ResolutionFolder=__SOURCE_DIRECTORY__>
    type DeviceListResponse = JsonProvider<"./../Data/DeviceList.json", ResolutionFolder=__SOURCE_DIRECTORY__>

    type GetDeviceInfoCommand = GetDeviceInfoCommand of string
    type GetDeviceCommand = GetDeviceCommand of int
    type DeleteDeviceCommand = DeleteDeviceCommand of string

    [<Literal>]
    let private Devices = "devices"

    let list () =
        HttpService.GetListRequest Devices
        |> DeviceListResponse.Parse
        |> fun r -> r.Devices
        |> Array.indexed
        |> Array.map (fun (i, e) -> DeviceListOutput.FormattedString(i, e.Iden, e.Nickname))
        |> String.concat Environment.NewLine

    let getDeviceInfo (GetDeviceInfoCommand iden) =
        HttpService.GetRequest $"{Devices}/{iden}" []
        |> DeviceResponse.Parse
        |> fun d ->
            GetDeviceOutput.FormattedString(
                d.Iden,
                d.Nickname,
                d.Manufacturer,
                d.Model,
                d.AppVersion,
                d.Created.ofUnixTimeToDateTime,
                d.Modified.ofUnixTimeToDateTime
            )

    let getDeviceId (GetDeviceCommand index) =
        HttpService.GetListRequest Devices
        |> DeviceListResponse.Parse
        |> fun r -> r.Devices
        |> fun a -> a |> Array.tryItem index
        |> function
            | Some v -> v.Iden
            | None -> ""

    let delete (DeleteDeviceCommand id) =
        HttpService.DeleteRequest $"{Devices}/{id}" DeviceDeleted
