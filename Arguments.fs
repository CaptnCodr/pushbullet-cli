namespace Pushbullet

open Argu
open Resources

module Arguments =

    type ChatArgs =
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-u")>] Update of id: string * status: string
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-c")>] Create of email: string

        interface IArgParserTemplate with
            member this.Usage =
                match this with 
                | Update _ -> ChatArgs_Update.ResourceString
                | Create _ -> ChatArgs_Create.ResourceString

    type PushArgs =
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-d")>] Device of deviceId: string
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-t")>] Text of body: string
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-n")>] Note of title: string * body: string
        
        interface IArgParserTemplate with
            member this.Usage =
                match this with 
                | Text _ -> PushArgs_Text.ResourceString
                | Note _ -> PushArgs_Note.ResourceString
                | Device _ -> PushArgs_Device.ResourceString

    type LinkArgs =
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-d")>] Device of deviceId: string
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-u");Mandatory>] Url of link: string
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-t");EqualsAssignment>] Title of title: string option
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-b");EqualsAssignment>] Body of body: string option
        
        interface IArgParserTemplate with
            member this.Usage =
                match this with 
                | Device _ -> LinkArgs_Device.ResourceString
                | Url _ -> LinkArgs_Url.ResourceString
                | Title _ -> LinkArgs_Title.ResourceString
                | Body _ -> LinkArgs_Body.ResourceString

    type DeleteArgs =
        | [<CliPrefix(CliPrefix.None); AltCommandLine("-p")>] Push of id: string
        | [<CliPrefix(CliPrefix.None); AltCommandLine("-c")>] Chat of id: string
        | [<CliPrefix(CliPrefix.None); AltCommandLine("-d")>] Device of id: string
        | [<CliPrefix(CliPrefix.None); AltCommandLine("-s")>] Subscription of id: string
        | [<CliPrefix(CliPrefix.None); AltCommandLine("-m")>] Sms of id: string
        | [<CliPrefix(CliPrefix.None); AltCommandLine("-k")>] Key
        
        interface IArgParserTemplate with
            member this.Usage =
                match this with 
                | Push _ -> DeleteArgs_Push.ResourceString
                | Chat _ -> DeleteArgs_Chat.ResourceString
                | Device _ -> DeleteArgs_Device.ResourceString
                | Subscription _ -> DeleteArgs_Subscription.ResourceString
                | Sms _ -> DeleteArgs_Sms.ResourceString
                | Key -> DeleteArgs_Key.ResourceString

    [<DisableHelpFlags>]
    type CliArguments =
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-cs")>] Chats
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-c")>] Chat of ParseResults<ChatArgs>

        | [<CliPrefix(CliPrefix.None);AltCommandLine("-m")>] Sms of device: string * number: string * body: string

        | [<CliPrefix(CliPrefix.None);AltCommandLine("-pi")>] PushInfo of pushid: string
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-p", "text", "-t")>] Push of ParseResults<PushArgs>
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-l", "url", "-u")>] Link of ParseResults<LinkArgs>
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-cl")>] Clip of json: string
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-ps");EqualsAssignment>] Pushes of number: int option

        | [<CliPrefix(CliPrefix.None);AltCommandLine("-d")>] Device of deviceid: string
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-ds")>] Devices
        
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-s", "subs")>] Subscriptions
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-ci")>] ChannelInfo of tag: string

        | [<CliPrefix(CliPrefix.None);AltCommandLine("-del", "-r")>] Delete of ParseResults<DeleteArgs>
        
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-k");EqualsAssignment>] Key of key: string option
        | [<CliPrefix(CliPrefix.None);CustomCommandLine("me");AltCommandLine("-i")>] Profile
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-x")>] Limits
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-g")>] Grants
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-h");>] Help
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-v");>] Version

        interface IArgParserTemplate with
            member this.Usage = 
                match this with
                | Key _ -> CliArguments_Key.ResourceString
                | Profile -> CliArguments_Profile.ResourceString
                | Limits -> CliArguments_Limits.ResourceString
                | Grants -> CliArguments_Grants.ResourceString
                | Help -> CliArguments_Help.ResourceString
                | Version -> CliArguments_Version.ResourceString

                | Chat _ -> CliArguments_Chat.ResourceString
                | Chats -> CliArguments_Chats.ResourceString

                | Device _ -> CliArguments_Device.ResourceString
                | Devices -> CliArguments_Devices.ResourceString

                | Sms _ -> CliArguments_Sms.ResourceString


                | PushInfo _ -> CliArguments_PushInfo.ResourceString
                | Push _ -> CliArguments_Push.ResourceString
                | Link _ -> CliArguments_Link.ResourceString
                | Clip _ -> CliArguments_Clip.ResourceString
                | Pushes _ -> CliArguments_Pushes.ResourceString

                | Subscriptions -> CliArguments_Subscriptions.ResourceString
                | ChannelInfo _ -> CliArguments_ChannelInfo.ResourceString

                | Delete _ -> CliArguments_Delete.ResourceString