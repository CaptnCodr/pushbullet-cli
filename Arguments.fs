namespace Pushbullet

open Argu

module Arguments =

    type PushArgs =
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-d")>] Device of deviceId: string
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-t")>] Text of body: string
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-n")>] Note of title: string * body: string
        
        interface IArgParserTemplate with

            member this.Usage =
                match this with 
                | Device _ -> "Specification of the device for a push."
                | Text _ -> "Specification of the push text."
                | Note _ -> "The note to push with title and message."

    type ChatArgs =
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-u")>] Update of id: string * status: string
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-c")>] Create of email: string

        interface IArgParserTemplate with

            member this.Usage =
                match this with 
                | Update _ -> "Update specified chat with a mute flag."
                | Create _ -> "Create new chat with given email address."

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
                | Push _ -> "Delete specified push."
                | Chat _ -> "Delete specified chat."
                | Device _ -> "Delete specified device."
                | Subscription _ -> "Delete specified subscription."
                | Sms _ -> "Delete specified sms."
                | Key -> "Delete the configured key."

    [<DisableHelpFlags>]
    type CliArguments =
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-k")>] Key of key:string option
        | [<CliPrefix(CliPrefix.None);CustomCommandLine("me");AltCommandLine("-i")>] Profile
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-x")>] Limits
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-g")>] Grants

        | [<CliPrefix(CliPrefix.None);AltCommandLine("-pi")>] PushInfo of pushid:string
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-p", "text")>] Push of ParseResults<PushArgs>
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-cl")>] Clip of string
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-ps")>] Pushes of number:int option
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-m")>] Sms of device: string * number: string * body: string
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-d")>] Device of string
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-ds")>] Devices
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-del")>] Delete of ParseResults<DeleteArgs>

        | [<CliPrefix(CliPrefix.None);AltCommandLine("-cs")>] Chats
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-cc")>] Chat of ParseResults<ChatArgs>
                
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-s", "subs")>] Subscriptions
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-ci")>] ChannelInfo of tag:string
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-h");>] Help
        | [<CliPrefix(CliPrefix.None);AltCommandLine("-v");>] Version

        interface IArgParserTemplate with

            member this.Usage = 
                match this with
                | Key _ -> "Set API key with argument. Show API key without argument."
                | Profile -> "Get profile of configured API key."
                | Limits -> "Get rate limits."
                | Grants -> "Get grants that have access to your PB account."
                | PushInfo _ -> "Gets information of the given push id."
                | Push _ -> "Push text or note. Use push [device / -d] to push to a specific device."
                | Pushes _ -> "List [number] of pushes or else last push."
                | Clip _ -> "Pushes a clip."
                | Sms _ -> "Send sms to eligible device."
                | Device _ -> "Shows information about a device. Select with identifier or index shown in the [devices / -ds] command."
                | Devices -> "Lists devices of current account. Including identifiers and indexes to identify."
                | Chat _ -> "Create or update chat."
                | Chats -> "List chats of current account."
                | Delete _ -> "Delete an object"
                | Subscriptions -> "List subscriptions with channel tag of current account."
                | ChannelInfo _ -> "Show information about a specific channel with channel tag as shown in [subscriptions / subs / -s]."
                | Help -> "Display this help."
                | Version -> "Shows the actual pushbullet-cli version."


