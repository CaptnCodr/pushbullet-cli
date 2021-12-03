namespace Pushbullet

open System
open Resources
open Argu
open Arguments
open CommandTypes

module Program =

    let dispatchCommand command =
        match command with
        | GetKey -> SystemCommands.getKey()
        | SetKey k -> k |> SystemCommands.setKey
        | DeleteKey -> SystemCommands.deleteKey()
        | GetProfile -> SystemCommands.getProfile()
        | GetLimits -> SystemCommands.getLimits()
        | ListGrants -> SystemCommands.listGrants()
        | GetHelp -> SystemCommands.getHelp()
        | GetVersion -> SystemCommands.getVersion()

        | PushText p -> p |> PushCommands.pushText
        | PushNote n -> n |> PushCommands.pushNote
        | PushLink l -> l |> PushCommands.pushLink
        | PushClip c -> c |> PushCommands.pushClip
        | ListPushes l -> l |> PushCommands.list
        | GetPush p -> p |> PushCommands.getSinglePush
        | DeletePush p -> p |> PushCommands.delete

        | SendMessage m -> m |> MessageCommands.create
        | DeleteMessage m -> m |> MessageCommands.delete

        | ListDevices -> DeviceCommands.list()
        | GetDeviceInfo s -> s |> DeviceCommands.getDeviceInfo
        | GetDevice i -> i |> DeviceCommands.getDeviceId
        | DeleteDevice d -> d |> DeviceCommands.delete

        | ListChats -> ChatCommands.list()
        | UpdateChat c -> c |> ChatCommands.update
        | CreateChat c -> c |> ChatCommands.create
        | DeleteChat c -> c |> ChatCommands.delete

        | ListSubscriptions -> SubscriptionCommands.list()
        | GetChannelInfo t -> SubscriptionCommands.channelInfo t
        | DeleteSubscription d -> SubscriptionCommands.delete d

        | Error e -> e.GetMessage()
        | Other s -> s// System.String.Format(Info_CommandNotFound.ResourceString, s)

    let toOption x =
        if String.IsNullOrWhiteSpace x then None else Some x

    let (|Int|_|) str =
       match System.Int32.TryParse(str: string) with
       | (true, int) -> Some int
       | _ -> None

    let (|Positive|Negative|Neither|) x =
        match x with 
        | "true" | "1" | "mute" -> Positive
        | "false" | "0" | "unmute" -> Negative
        | _ -> Neither

    let valueToBool = function
        | Positive -> Some true
        | Negative -> Some false
        | Neither -> None

    let getDeviceFromIndexOrDeviceId device =
        match device with
        | Int i -> i |> DeviceCommands.GetDeviceCommand |> GetDevice |> dispatchCommand
        | _ -> device

    let resultToValue (cmd: Option<string option>) = 
        if cmd.IsSome then cmd.Value else None

    let runCommands (parser: ArgumentParser<CliArguments>) (args: string array) =
        let parsingResult = parser.Parse args

        match parsingResult.GetAllResults() with 
        | [ Key arg ] -> if arg.IsSome then arg.Value |> SystemCommands.SetKeyCommand |> SetKey else GetKey
        | [ Profile ] -> GetProfile
        | [ Limits ] -> GetLimits
        | [ Grants ] -> ListGrants
        | [ PushInfo arg ] -> arg |> PushCommands.GetPushCommand |> GetPush
        | [ Push subCommand ] -> 
            let deviceResult = subCommand.TryGetResult(PushArgs.Device)
            let dev = if deviceResult.IsSome then deviceResult.Value |> getDeviceFromIndexOrDeviceId |> toOption else None

            match subCommand.GetAllResults() with 
            | [ Text t ] -> (t, dev) |> PushCommands.PushTextCommand |> PushText
            | [ Note (title, body) ] -> (title |> toOption, body |> toOption, dev) |> PushCommands.PushNoteCommand |> PushNote
            | _ -> Other $"{subCommand.Parser.PrintUsage()}"

        | [ Link subCommand ] ->
            let deviceResult = subCommand.TryGetResult(LinkArgs.Device)
            let dev = if deviceResult.IsSome then deviceResult.Value |> getDeviceFromIndexOrDeviceId |> toOption else None
            if subCommand.Contains(Url) then
                (subCommand.GetResult(Url), subCommand.TryGetResult(Title) |> resultToValue, subCommand.TryGetResult(Body) |> resultToValue, dev) |> PushCommands.PushLinkCommand |> PushLink
            else
                Other $"{subCommand.Parser.PrintUsage()}"

        | [ Pushes arg ] -> if arg.IsNone then 0 |> PushCommands.ListPushesCommand |> ListPushes else arg.Value |> PushCommands.ListPushesCommand |> ListPushes 

        | [ Delete subCommand ] -> 
            match subCommand.GetAllResults() with 
            | [ DeleteArgs.Push p ] -> p |> PushCommands.DeletePushCommand |> DeletePush
            | [ DeleteArgs.Chat c ] -> c |> ChatCommands.DeleteChatCommand |> DeleteChat
            | [ DeleteArgs.Device d ] -> d |> DeviceCommands.DeleteDeviceCommand |> DeleteDevice
            | [ Subscription sub ] -> sub |> SubscriptionCommands.DeleteSubscriptionCommand |> DeleteSubscription
            | [ DeleteArgs.Sms s ] -> s |> MessageCommands.DeleteMessageCommand |> DeleteMessage
            | [ DeleteArgs.Key _ ] -> DeleteKey
            | _ -> Other $"{subCommand.Parser.PrintUsage()}"

        | [ Clip arg ] -> arg |> PushCommands.PushClipCommand |> PushClip
        | [ Sms (device, number, body) ] -> (device |> getDeviceFromIndexOrDeviceId, number, body) |> MessageCommands.SendMessageCommand |> SendMessage
        | [ Device arg ] -> arg |> getDeviceFromIndexOrDeviceId |> DeviceCommands.GetDeviceInfoCommand |> GetDeviceInfo
        | [ Devices ] -> ListDevices
        | [ Chat subCommand ] -> 
            match subCommand.GetAllResults() with 
            | [ Create c ] -> c |> ChatCommands.CreateChatCommand |> CreateChat
            | [ Update (id, status) ] -> 
                match status |> valueToBool with 
                | Some b -> (id, b) |> ChatCommands.UpdateChatCommand |> UpdateChat 
                | None -> Error ParameterInvalid
            | _ -> Other $"{subCommand.Parser.PrintUsage()}"
        | [ Chats ] -> ListChats

        | [ Subscriptions ] -> ListSubscriptions
        | [ ChannelInfo arg ] -> arg |> SubscriptionCommands.GetChannelInfoCommand |> GetChannelInfo
        | [ Help ] -> Other $"{parser.PrintUsage()}"
        | [ Version ] -> GetVersion
        | _ -> Other $"{parser.PrintUsage()}"


    [<EntryPoint>]
    let main ([<ParamArray>] argv: string[]): int =

        try 
            let parser = ArgumentParser.Create<CliArguments>()
            let command = runCommands parser argv
            
            if command.IsSetKeyCommand || SystemCommands.getKey() <> ""
            then printfn $"{command |> dispatchCommand}"
            else printf $"{Info_SetupKey.ResourceString}"
        with
        | ex -> eprintfn $"{ex.Message}"

        0