namespace Pushbullet

open FSharp.Data

type ChannelInfoResponse = JsonProvider<"./Data/ChannelInfoData.json", ResolutionFolder=__SOURCE_DIRECTORY__>

type DataResponse = JsonProvider<"./Data/DataLists.json", ResolutionFolder=__SOURCE_DIRECTORY__>

type DeviceResponse = JsonProvider<"./Data/DeviceData.json", ResolutionFolder=__SOURCE_DIRECTORY__>

type ErrorResponse = JsonProvider<"./Data/Error.json", ResolutionFolder=__SOURCE_DIRECTORY__>

type MessageResponse = JsonProvider<"./Data/MessageData.json", ResolutionFolder=__SOURCE_DIRECTORY__>

type PushResponse = JsonProvider<"./Data/PushData.json", ResolutionFolder=__SOURCE_DIRECTORY__>

type UserResponse = JsonProvider<"./Data/UserData.json", ResolutionFolder=__SOURCE_DIRECTORY__>
