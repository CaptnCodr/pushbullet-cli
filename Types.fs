namespace Pushbullet

open FSharp.Data

//Remove when fixed in dotnet-sdk
module Workaround =
    [<Literal>]
    let refDir = __SOURCE_DIRECTORY__

type DataResponse = JsonProvider<"./Data/DataLists.json", ResolutionFolder=Workaround.refDir>

type ChannelInfoResponse = JsonProvider<"./Data/ChannelInfoData.json", ResolutionFolder=Workaround.refDir>

type ErrorResponse = JsonProvider<"./Data/Error.json", ResolutionFolder=Workaround.refDir>