namespace Pushbullet

open System
open Resources
open Argu
open Arguments

module Program =

    let toOption x =
        match x with 
        | null | "" -> None 
        | _ -> Some x

    let (|Int|_|) str =
       match System.Int32.TryParse(str: string) with
       | (true, int) -> Some int
       | _ -> None

    let (|Positive|Negative|Neither|) = function
        | "true" | "1" | "mute" -> Positive
        | "false" | "0" | "unmute" -> Negative
        | _ -> Neither

    let valueToBool = function
        | Positive -> Some true
        | Negative -> Some false
        | Neither -> None

    let getDeviceFromIndexOrDeviceId device =
        match device with
        | Int i -> i |> DeviceCommands.GetDeviceCommand |> DeviceCommands.getDeviceId
        | _ -> device

    let getDeviceIdFromResult result = 
        match result with 
        | Some v -> v |> getDeviceFromIndexOrDeviceId |> toOption
        | None -> None


    let runCommands (parser: ArgumentParser<CliArguments>) (args: string array) =
        let parsingResult = parser.Parse args

        if SystemCommands.getKey() <> "" then
            match parsingResult.GetAllResults() with 
            | [ Key arg ] -> 
                match arg with 
                | Some a -> a |> SystemCommands.SetKeyCommand |> SystemCommands.setKey 
                | None -> SystemCommands.getKey()
            | [ Profile ] -> SystemCommands.getProfile()
            | [ Limits ] -> SystemCommands.getLimits()
            | [ Grants ] -> SystemCommands.listGrants()
            | [ PushInfo arg ] -> arg |> PushCommands.GetPushCommand |> PushCommands.getSinglePush
            | [ Push subCommand ] -> 
                let deviceId =  subCommand.TryGetResult(PushArgs.Device) |> getDeviceIdFromResult

                if subCommand.Contains(Text) then
                    (subCommand.GetResult(Text), deviceId) |> PushCommands.PushTextCommand |> PushCommands.pushText
                else if subCommand.Contains(Note) then
                    let (title, body) = subCommand.GetResult(Note)
                    (title |> toOption, body |> toOption, deviceId) |> PushCommands.PushNoteCommand |> PushCommands.pushNote
                else subCommand.Parser.PrintUsage()

            | [ Link subCommand ] ->
                let deviceId =  subCommand.TryGetResult(LinkArgs.Device) |> getDeviceIdFromResult

                if subCommand.Contains(Url) then
                    (subCommand.GetResult(Url), subCommand.TryGetResult(Title) |> Option.bind id, subCommand.TryGetResult(Body) |> Option.bind id, deviceId) |> PushCommands.PushLinkCommand |> PushCommands.pushLink
                else
                    subCommand.Parser.PrintUsage()

            | [ Pushes arg ] -> 
                match arg with
                | Some v -> v |> PushCommands.ListPushesCommand |> PushCommands.list
                | None -> 0 |> PushCommands.ListPushesCommand |> PushCommands.list
            | [ Delete subCommand ] -> 
                match subCommand.GetAllResults() with 
                | [ DeleteArgs.Push p ] -> p |> PushCommands.DeletePushCommand |> PushCommands.delete
                | [ DeleteArgs.Chat c ] -> c |> ChatCommands.DeleteChatCommand |> ChatCommands.delete
                | [ DeleteArgs.Device d ] -> d |> DeviceCommands.DeleteDeviceCommand |> DeviceCommands.delete
                | [ Subscription sub ] -> sub |> SubscriptionCommands.DeleteSubscriptionCommand |> SubscriptionCommands.delete
                | [ DeleteArgs.Sms s ] -> s |> MessageCommands.DeleteMessageCommand |> MessageCommands.delete
                | [ DeleteArgs.Key _ ] -> SystemCommands.deleteKey()
                | _ -> subCommand.Parser.PrintUsage()

            | [ Clip arg ] -> arg |> PushCommands.PushClipCommand |> PushCommands.pushClip
            | [ Sms (device, number, body) ] -> (device |> getDeviceFromIndexOrDeviceId, number, body) |> MessageCommands.SendMessageCommand |> MessageCommands.create
            | [ Device arg ] -> arg |> getDeviceFromIndexOrDeviceId |> DeviceCommands.GetDeviceInfoCommand |> DeviceCommands.getDeviceInfo
            | [ Devices ] -> DeviceCommands.list()
            | [ Chat subCommand ] -> 
                match subCommand.GetAllResults() with 
                | [ Create c ] -> c |> ChatCommands.CreateChatCommand |> ChatCommands.create
                | [ Update (id, status) ] -> 
                    match status |> valueToBool with 
                    | Some b -> (id, b) |> ChatCommands.UpdateChatCommand |> ChatCommands.update 
                    | None -> Errors_ParameterInvalid.ResourceString
                | _ -> subCommand.Parser.PrintUsage()
            | [ Chats ] -> ChatCommands.list()

            | [ Subscriptions ] -> SubscriptionCommands.list()
            | [ ChannelInfo arg ] -> arg |> SubscriptionCommands.GetChannelInfoCommand |> SubscriptionCommands.channelInfo
            | [ Version ] -> SystemCommands.getVersion()
            | [ Help ] -> parser.PrintUsage()
            | _ -> parser.PrintUsage()
        else 
            match parsingResult.GetAllResults() with 
            | [ Key arg ] -> 
                match arg with 
                | Some a -> a |> SystemCommands.SetKeyCommand |> SystemCommands.setKey 
                | None -> SystemCommands.getKey()
            | _ -> Info_SetupKey.ResourceString

    [<EntryPoint>]
    let main ([<ParamArray>] argv: string[]): int =

        try 
            (ArgumentParser.Create<CliArguments>(), argv)
            ||> runCommands 
            |> printfn "%s"
        with
        | ex -> eprintfn $"{ex.Message}"

        0