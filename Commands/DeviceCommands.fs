namespace Pushbullet

open System
open Utilities

module DeviceCommands =

    [<Literal>]
    let private Devices = "devices"

    let list () =
        HttpService.GetListRequest Devices
        |> DataResponse.Parse
        |> fun r -> r.Devices
        |> Array.indexed |> Array.map (fun (i, e) -> $"{i} [{e.Iden}] {e.Nickname}")
        |> String.concat Environment.NewLine

    let getDeviceInfo iden =
        HttpService.GetRequest $"{Devices}/{iden}" []
        |> DeviceResponse.Parse
        |> fun d -> $"[{d.Iden}]:\nname: {d.Nickname}\ndevice: {d.Manufacturer} {d.Model}\napp version: {d.AppVersion}\ncreated: {d.Created |> unixTimestampToDateTime}\nmodified: {d.Modified |> unixTimestampToDateTime}"

    let getDeviceId index =
        HttpService.GetListRequest Devices
        |> DataResponse.Parse
        |> fun r -> r.Devices
        |> fun a -> if a.Length > index then a.[index].Iden else String.Empty

    let delete id =
        HttpService.DeleteRequest $"{Devices}/{id}" "Device deleted!"
