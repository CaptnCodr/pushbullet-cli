namespace Pushbullet

open Extensions.ResourceExt

module Resources =

    [<Literal>]
    let ResourceFile = "pushbullet-cli.Resources.Strings"

    type ResourceTypes =
        | CliArguments_Key
        | CliArguments_Profile
        | CliArguments_Limits
        | CliArguments_Grants
        | CliArguments_PushInfo
        | CliArguments_Push
        | CliArguments_Link
        | CliArguments_Pushes
        | CliArguments_Clip
        | CliArguments_Sms
        | CliArguments_Device
        | CliArguments_Devices
        | CliArguments_Chat
        | CliArguments_Chats
        | CliArguments_Delete
        | CliArguments_Subscriptions
        | CliArguments_ChannelInfo
        | CliArguments_Help
        | CliArguments_Version

        | PushArgs_Device
        | PushArgs_Text
        | PushArgs_Note

        | LinkArgs_Device
        | LinkArgs_Url
        | LinkArgs_Title
        | LinkArgs_Body

        | ChatArgs_Update
        | ChatArgs_Create

        | DeleteArgs_Push
        | DeleteArgs_Chat
        | DeleteArgs_Device
        | DeleteArgs_Subscription
        | DeleteArgs_Sms
        | DeleteArgs_Key

        | Info_SetupKey
        | Info_CommandNotFound

        | KeySet
        | KeyRemoved

        | GetProfileOutput
        | GetLimitsOutput
        | ListGrantsOutput

        | ChatListOutput
        | ChatCreated
        | ChatMuted
        | ChatUnmuted
        | ChatDeleted

        | DeviceListOutput
        | GetDeviceOutput
        | DeviceDeleted

        | CreateMessageOutput
        | MessageSent
        | MessageDeleted

        | ListLinkPushOutput
        | ListTextPushOutput
        | GetSinglePushOutput
        | PushSent
        | LinkSent
        | ClipSent
        | PushDeleted

        | ListSubscriptionOutput
        | SubscriptionDeleted

        | Errors_NotEnoughArguments
        | Errors_ParameterInvalid
        | Errors_NoParametersGiven
        | Empty

        member this.ResourceString = ResourceFile |> resourceManager |> getResourceString this

        member this.FormattedString([<System.ParamArray>] args) =
            ResourceFile |> resourceManager |> getFormattedString this args
