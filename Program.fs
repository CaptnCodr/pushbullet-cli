namespace Pushbullet

open System
open CommandTypes
open Patterns
open Resources
open Argu
open Arguments

module Program =

    let dispatchCommand command =
        match command with
        | GetKey -> SystemCommands.getKey()
        | SetKey k -> SystemCommands.setKey k
        | DeleteKey -> SystemCommands.deleteKey()
        | GetProfile -> SystemCommands.getProfile()
        | GetLimits -> SystemCommands.getLimits()
        | ListGrants -> SystemCommands.listGrants()
        | GetHelp -> SystemCommands.getHelp()
        | GetVersion -> SystemCommands.getVersion()

        | PushText p -> PushCommands.pushText p
        | PushNote n -> PushCommands.pushNote n
        | PushLink l -> PushCommands.pushLink l
        | PushClip c -> PushCommands.pushClip c
        | ListPushes l -> PushCommands.list l
        | GetPush p -> PushCommands.getSinglePush p
        | DeletePush p -> PushCommands.delete p

        | SendMessage m -> MessageCommands.create m
        | DeleteMessage m -> MessageCommands.delete m

        | ListDevices -> DeviceCommands.list()
        | GetDeviceInfo s -> DeviceCommands.getDeviceInfo s
        | GetDevice i -> DeviceCommands.getDeviceId i
        | DeleteDevice d -> DeviceCommands.delete d

        | ListChats -> ChatCommands.list()
        | UpdateChat c -> ChatCommands.update c
        | CreateChat c -> ChatCommands.create c
        | DeleteChat c -> ChatCommands.delete c

        | ListSubscriptions -> SubscriptionCommands.list()
        | GetChannelInfo t -> SubscriptionCommands.channelInfo t
        | DeleteSubscription d -> SubscriptionCommands.delete d

        | Error e -> e.GetMessage()
        | Other s -> s// System.String.Format(Info_CommandNotFound.ResourceString, s)

    let toOption x =
        if String.IsNullOrWhiteSpace x then None else Some x

    let getLinkParams args =
        ((args |> Array.tryItem 0).Value, args |> Array.tryItem 1, args |> Array.tryItem 2)

    let (|Int|_|) str =
       match System.Int32.TryParse(str: string) with
       | (true, int) -> Some int
       | _ -> None

    let (|Positive|Negative|Neither|) x =
        match x with 
        | "true" | "1" | "mute" -> Positive
        | "false" | "0" | "unmute" -> Negative
        | _ -> Neither

    let valueToBool value = 
        match value with
        | Positive -> Some true
        | Negative -> Some false
        | Neither -> None

    let getDeviceFromIndexOrDeviceId device =
        match device with
        | Int i -> i |> DeviceCommands.GetDeviceCommand |> GetDevice |> dispatchCommand
        | _ -> device

    let findCommand (args: string[]) =
        if args.Length > 0 then
            match args.[0] with
            | Patterns.Key _ -> match args.Length with | 2 -> args.[1] |> SystemCommands.SetKeyCommand |> SetKey | _ -> GetKey
            | Patterns.Profile _ -> GetProfile
            | Patterns.Limits _ -> GetLimits
            | Patterns.Grants _ -> ListGrants
            | Patterns.PushInfo _ -> match args.Length with | 2 -> args.[1] |> PushCommands.GetPushCommand |> GetPush | _ -> Error NotEnoughArguments
            | Patterns.Push _ | Patterns.Text _ ->
                match args.Length with
                | x when x > 1 -> 
                    match args.[1] with 
                    | Patterns.Device _ -> 
                        match args.Length with
                        | 4 -> (args.[3], args.[2] |> getDeviceFromIndexOrDeviceId |> toOption) |> PushCommands.PushTextCommand |> PushText
                        | 5 -> (args.[3] |> toOption, args.[4] |> toOption, args.[2] |> getDeviceFromIndexOrDeviceId |> toOption) |> PushCommands.PushNoteCommand |> PushNote 
                        | _ -> Error NotEnoughArguments
                    | _ -> 
                        match args.Length with
                        | 2 -> (args.[1], None) |> PushCommands.PushTextCommand |> PushText 
                        | 3 -> (args.[1] |> toOption, args.[2] |> toOption, None) |> PushCommands.PushNoteCommand |> PushNote
                        | _ -> Error NotEnoughArguments
                | _ -> Error NotEnoughArguments
            | Patterns.Link _ | Patterns.Url _ ->
                match args.Length with
                | x when x > 1 -> 
                    match args.[1] with
                    | Patterns.Device _ -> let (a, b, c) = (args.[3..] |> getLinkParams) in (a, b, c, args.[2] |> int |> DeviceCommands.GetDeviceCommand |> GetDevice |> dispatchCommand |> toOption) |> PushCommands.PushLinkCommand |> PushLink 
                    | _ -> 
                        match args.Length with 
                        | y when y > 2 -> let (a, b, c) = (args.[1..] |> getLinkParams) in (a, b, c, None) |> PushCommands.PushLinkCommand |> PushLink 
                        | _ -> Error NotEnoughArguments
                | _ -> Error NotEnoughArguments
            | Patterns.Clip _ -> match args.Length with | 2 -> args.[1] |> PushCommands.PushClipCommand |> PushClip | _ -> Error NotEnoughArguments
            | Patterns.Pushes _ -> match args.Length with | 1 -> 0 |> PushCommands.ListPushesCommand |> ListPushes | 2 -> args.[1] |> int |> PushCommands.ListPushesCommand |> ListPushes | _ -> Error NotEnoughArguments
            | Patterns.Delete _ -> 
                match args.Length with
                | 3 -> 
                    match args.[1] with
                    | Patterns.Push _ -> args.[2] |> PushCommands.DeletePushCommand |> DeletePush
                    | Patterns.Chat _ -> args.[2] |> ChatCommands.DeleteChatCommand |> DeleteChat
                    | Patterns.Device _ -> args.[2] |> DeviceCommands.DeleteDeviceCommand |> DeleteDevice
                    | Patterns.Subscription _ -> args.[2] |> SubscriptionCommands.DeleteSubscriptionCommand |> DeleteSubscription
                    | Patterns.Sms _ -> args.[2] |> MessageCommands.DeleteMessageCommand |> DeleteMessage
                    | e -> Other e
                | 2 -> match args.[1] with | Patterns.Key _ -> DeleteKey | e -> Other e
                | _ -> Error NotEnoughArguments
            | Patterns.Sms _ -> 
                match args.Length with 
                | 4 -> (args.[1] |> getDeviceFromIndexOrDeviceId, args.[2], args.[3]) |> MessageCommands.SendMessageCommand |> SendMessage
                | _ -> Error NotEnoughArguments
            | Patterns.Devices _ -> ListDevices
            | Patterns.Device _ -> 
                match args.Length with 
                | 2 -> match args.[1] |> getDeviceFromIndexOrDeviceId |> toOption with | Some x -> x |> DeviceCommands.GetDeviceInfoCommand |> GetDeviceInfo | None -> Error ParameterInvalid
                | _ -> Error NotEnoughArguments
            | Patterns.Chats _ -> ListChats
            | Patterns.Chat _ ->
                match args.Length with
                | 3 -> match args.[2] |> valueToBool with | Some b -> (args.[1], b) |> ChatCommands.UpdateChatCommand |> UpdateChat | None -> Error ParameterInvalid
                | 2 -> args.[1] |> ChatCommands.CreateChatCommand |> CreateChat
                | _ -> Error NotEnoughArguments
            | Patterns.Subscriptions _ -> ListSubscriptions
            | Patterns.ChannelInfo _ -> match args.Length with | 2 -> args.[1] |> SubscriptionCommands.GetChannelInfoCommand |> GetChannelInfo | _ -> Error NotEnoughArguments
            | Patterns.Help _ -> GetHelp
            | Patterns.Version _ -> GetVersion
            | e -> Other e
        else
            Error NoParametersGiven

    let runCommands (parser: ArgumentParser<CliArguments>) (args: string array) =
        let parsingResult = parser.Parse args

        match parsingResult.GetAllResults() with 
        | [ Key arg ] -> match arg.IsSome with | true -> arg.Value |> SystemCommands.SetKeyCommand |> SetKey | _ -> GetKey
        | [ Profile ] -> GetProfile
        | [ Limits ] -> GetLimits
        | [ Grants ] -> ListGrants
        | [ PushInfo arg ] -> arg |> PushCommands.GetPushCommand |> GetPush
        | [ Push subCommand ] -> 
            let deviceResult = subCommand.TryGetResult(PushArgs.Device)
            if subCommand.Contains(Text) then
                if deviceResult.IsSome then
                    (subCommand.GetResult(Text), deviceResult.Value |> getDeviceFromIndexOrDeviceId |> toOption) |> PushCommands.PushTextCommand |> PushText
                else (subCommand.GetResult(Text), None) |> PushCommands.PushTextCommand |> PushText
            elif subCommand.Contains(Note) then
                let (title, body) = subCommand.GetResult(Note)
                if deviceResult.IsSome then
                    (title |> toOption, body |> toOption, deviceResult.Value |> getDeviceFromIndexOrDeviceId |> toOption) |> PushCommands.PushNoteCommand |> PushNote
                else (title |> toOption, body |> toOption, None) |> PushCommands.PushNoteCommand |> PushNote
            else 
                Other $"{subCommand.Parser.PrintUsage()}"
        | [ Pushes arg ] -> match arg.IsNone with | true -> 0 |> PushCommands.ListPushesCommand |> ListPushes | _ -> arg.Value |> PushCommands.ListPushesCommand |> ListPushes 
        | [ Delete subCommand ] -> 
            if subCommand.Contains(DeleteArgs.Push) then subCommand.GetResult(DeleteArgs.Push) |> PushCommands.DeletePushCommand |> DeletePush
            elif subCommand.Contains(DeleteArgs.Chat) then subCommand.GetResult(DeleteArgs.Chat) |> ChatCommands.DeleteChatCommand |> DeleteChat
            elif subCommand.Contains(DeleteArgs.Device) then subCommand.GetResult(DeleteArgs.Device) |> DeviceCommands.DeleteDeviceCommand |> DeleteDevice
            elif subCommand.Contains(DeleteArgs.Subscription) then subCommand.GetResult(DeleteArgs.Subscription) |> SubscriptionCommands.DeleteSubscriptionCommand |> DeleteSubscription
            elif subCommand.Contains(DeleteArgs.Sms) then subCommand.GetResult(DeleteArgs.Sms) |> MessageCommands.DeleteMessageCommand |> DeleteMessage
            elif subCommand.Contains(DeleteArgs.Key) then DeleteKey
            else Other $"{subCommand.Parser.PrintUsage()}"
        | [ Clip arg ] -> arg |> PushCommands.PushClipCommand |> PushClip
        | [ Sms (device, number, body) ] -> (device |> getDeviceFromIndexOrDeviceId, number, body) |> MessageCommands.SendMessageCommand |> SendMessage
        | [ Device arg ] -> arg |> getDeviceFromIndexOrDeviceId |> DeviceCommands.GetDeviceInfoCommand |> GetDeviceInfo
        | [ Devices ] -> ListDevices
        | [ Chat subCommand ] -> 
            if subCommand.Contains(ChatArgs.Create) then
                subCommand.GetResult(ChatArgs.Create) |> ChatCommands.CreateChatCommand |> CreateChat
            elif subCommand.Contains(ChatArgs.Update) then
                let (id, status) = subCommand.GetResult(ChatArgs.Update)
                match status |> valueToBool with | Some b -> (id, b) |> ChatCommands.UpdateChatCommand |> UpdateChat | None -> Error ParameterInvalid
            else 
                Other $"{subCommand.Parser.PrintUsage()}"
        | [ Chats ] -> ListChats

        | [ Subscriptions ] -> ListSubscriptions
        | [ ChannelInfo arg ] -> arg |> SubscriptionCommands.GetChannelInfoCommand |> GetChannelInfo
        | [ Help ] -> Other $"{parser.PrintUsage()}"
        | [ Version ] -> GetVersion
        | _ -> Other $"{parser.PrintUsage()}"


    [<EntryPoint>]
    let main ([<ParamArray>] argv: string[]): int =

        //let command = argv |> findCommand

        try 
            let parser = ArgumentParser.Create<CliArguments>()
            let command = runCommands parser argv
            
            if command.IsSetKeyCommand || SystemCommands.getKey() <> ""
            then printfn $"{command |> dispatchCommand}"
            else printf $"{Info_SetupKey.ResourceString}"
        with
        | ex -> eprintfn $"{ex.Message}"


        0