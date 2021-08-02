namespace Pushbullet

open System
open CommandHelper

module DeviceCommands =

    let list () =
        HttpService.GetRequest "devices" [Actives]
        |> DataResponse.Parse
        |> fun r -> r.Devices
        |> Array.indexed |> Array.map (fun (i, e) -> $"{i} [{e.Iden}] {e.Nickname}")
        |> String.concat Environment.NewLine

    let getDeviceInfo iden =
        HttpService.GetRequest $"devices/{iden}" []
        |> DeviceResponse.Parse
        |> fun d -> $"[{d.Iden}]:\nName: {d.Nickname}\nDevice: {d.Manufacturer} {d.Model}\nAppVersion: {d.AppVersion}\nCreated: {d.Created |> unixTimestampToDateTime}\nModified: {d.Modified |> unixTimestampToDateTime}"

    let getDeviceId index =
        HttpService.GetRequest "devices" [Actives]
        |> DataResponse.Parse
        |> fun r -> r.Devices
        |> fun a -> if a.Length > index then a.[index].Iden else String.Empty

    let delete id =
        HttpService.DeleteRequest $"devices/{id}" "Device deleted!"
