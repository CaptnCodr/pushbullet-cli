namespace Pushbullet

open FSharp.Data

//Remove when fixed in dotnet-sdk
module Workaround =
    [<Literal>]
    let refDir = __SOURCE_DIRECTORY__

type ChannelInfoResponse = JsonProvider<"./Data/ChannelInfoData.json", ResolutionFolder=Workaround.refDir>

type DataResponse = JsonProvider<"./Data/DataLists.json", ResolutionFolder=Workaround.refDir>

type DeviceResponse = JsonProvider<"./Data/DeviceData.json", ResolutionFolder=Workaround.refDir>

type ErrorResponse = JsonProvider<"./Data/Error.json", ResolutionFolder=Workaround.refDir>

type UserResponse = JsonProvider<"./Data/UserData.json", ResolutionFolder=Workaround.refDir>