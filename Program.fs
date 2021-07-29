namespace Pushbullet

open CommandTypes
open CommandHelper

module Program =

    let followCommands command =
        match command with
        | GetKey -> SystemCommands.getKey()
        | SetKey k -> SystemCommands.setKey k
        | DeleteKey -> SystemCommands.deleteKey()
        | GetMe -> SystemCommands.getMe()
        | GetLimits -> SystemCommands.getLimits()
        | Help -> SystemCommands.getHelp()

        | PushText (t, d) -> PushCommands.pushText t d
        | PushNote (t, b, d) -> PushCommands.pushNote t b d
        | PushLink (u, t, b, d) -> PushCommands.pushLink u t b d
        | PushClip c -> PushCommands.pushClip c
        | ListPushes l -> PushCommands.list l
        | DeletePush p -> PushCommands.delete p

        | ListDevices -> DeviceCommands.list()
        | GetDeviceInfo s -> DeviceCommands.getDeviceInfo s
        | GetDevice i -> DeviceCommands.getDeviceId i
        | DeleteDevice d -> DeviceCommands.delete d

        | ListChats -> ChatCommands.list()
        | UpdateChat (i, s) -> ChatCommands.update i s
        | CreateChat c -> ChatCommands.create c
        | DeleteChat c -> ChatCommands.delete c

        | ListSubscriptions -> SubscriptionCommands.list()
        | ChannelInfo t -> SubscriptionCommands.channelInfo t
        | DeleteSubscription d -> SubscriptionCommands.delete d

        | Error e -> e.GetMessage()
        | Other s -> $"Command '{s}' not found!\n\nUse:\npb help | -h \nto show commands."

    let getLinkParams args =
        let url = args |> Array.tryItem 0
        let title = args |> Array.tryItem 1
        let body = args |> Array.tryItem 2
        (url.Value, title, body)

    let (|Int|_|) str =
       match System.Int32.TryParse(str: string) with
       | (true,int) -> Some int
       | _ -> None

    let valueToBool value = 
        match value with
        | "true" | "1" | "mute" -> Some true
        | "false" | "0" | "unmute" -> Some false
        | _ -> None

    let getDeviceFromIndexOrDeviceId device =
        match device with
        | Int i -> i |> GetDevice |> followCommands
        | _ -> device

    let delArgument (args: string[]) =
        match args.Length with
        | 3 -> 
            match args.[1] with
            | "push" | "-p" -> args.[2] |> DeletePush
            | "chat" | "-c" -> args.[2] |> DeleteChat
            | "device" | "-d" -> args.[2] |> DeleteDevice
            | "subscription" | "-s" -> args.[2] |> DeleteSubscription
            | e -> Other e
        | 2 -> match args.[1] with | "key" | "-k" -> DeleteKey | e -> Other e
        | _ -> Error NotEnoughArguments

    let listArgument (args: string[]) =
        match args.[1] with
        | "pushes" | "-p" ->
            match args.Length with
            | 3 -> 
                match args.[2] with
                | Int i -> (if i < 1 then 1 else i) |> ListPushes
                | _ -> Error ParameterInvalid
            | _ -> ListPushes 1
        | "devices" | "-d" -> ListDevices
        | "chats" | "-c" -> ListChats
        | "subscriptions" | "-s" -> ListSubscriptions
        | e -> Other e

    let findBaseCommand (args: string[]) =
        if args.Length > 0 then
            match args.[0] with
            | "key" | "-k" -> match args.Length with | 2 -> args.[1] |> SetKey | _ -> GetKey
            | "me" | "-i" -> GetMe
            | "limits" | "-x" -> GetLimits
            | "push" | "-p" | "text" | "-t" ->
                match args.Length with
                | x when x > 1 -> 
                    match args.[1] with 
                    | "device" | "-d" -> 
                        match args.Length with
                        | 4 -> (args.[3], args.[2] |> getDeviceFromIndexOrDeviceId |> toOption) |> PushText
                        | 5 -> (args.[3] |> toOption, args.[4] |> toOption, args.[2] |> getDeviceFromIndexOrDeviceId |> toOption) |> PushNote 
                        | _ -> Error NotEnoughArguments
                    | _ -> 
                        match args.Length with
                        | 2 -> (args.[1], None) |> PushText 
                        | 3 -> (args.[1] |> toOption, args.[2] |> toOption, None) |> PushNote
                        | _ -> Error NotEnoughArguments
                | _ -> Error NotEnoughArguments
            | "link" | "-u" | "url" ->
                match args.Length with
                | x when x > 1 -> 
                    match args.[1] with
                    | "device" | "-d" -> let (a, b, c) = (args.[3..] |> getLinkParams) in PushLink (a, b, c, args.[2] |> int |> GetDevice |> followCommands |> toOption)
                    | _ -> match args.Length with 
                            | 2 -> let (a, b, c) = (args.[1..] |> getLinkParams) in PushLink (a, b, c, None) 
                            | _ -> Error NotEnoughArguments
                | _ -> Error NotEnoughArguments
            | "clip" | "-cl" -> match args.Length with | 2 -> args.[1] |> PushClip | _ -> Error NotEnoughArguments
            | "pushes" | "-ps" -> match args.Length with | 2 -> args.[1] |> int |> ListPushes | _ -> Error NotEnoughArguments
            | "delete" | "-d" | "--del" -> args |> delArgument
            | "devices" | "-ds" -> ListDevices
            | "device" | "-di" -> match args.Length with | 2 -> args.[1] |> getDeviceFromIndexOrDeviceId |> GetDeviceInfo | _ -> Error NotEnoughArguments
            | "chats" | "-cs" -> ListChats
            | "chat" | "-c" ->
                match args.Length with
                | 3 -> match args.[2] |> valueToBool with | Some b -> (args.[1], b) |> UpdateChat | None -> Error ParameterInvalid
                | 2 -> args.[1] |> CreateChat
                | _ -> Error NotEnoughArguments
            | "subscriptions" | "subs" | "-s" -> ListSubscriptions
            | "channelinfo" | "-ci" -> match args.Length with | 2 -> args.[1] |> ChannelInfo | _ -> Error NotEnoughArguments
            | "list" | "-l" -> match args.Length with | x when x > 1 -> args |> listArgument | _ -> Error NotEnoughArguments
            | "help" | "-h" -> Help
            | e -> Other e
        else
            Error NoParametersGiven

    [<EntryPoint>]
    let main ([<System.ParamArray>] argv: string[]): int =

        let command = argv |> findBaseCommand

        if command.IsSetKeyCommand || SystemCommands.getKey() <> ""
        then printfn $"{command |> followCommands}"
        else printfn "You have to set your API key with:\n\">pb key o.Abc12345xyz\""

        0