namespace Pushbullet

open CommandTypes

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
        | DeleteChat c -> ChatCommands.delete c

        | ListSubscriptions -> SubscriptionCommands.list()
        | ChannelInfo t -> SubscriptionCommands.channelInfo t
        | DeleteSubscription d -> SubscriptionCommands.delete d

        | Error e -> e.GetMessage()
        | _ -> "Command not found!\n\nUse:\npb help | -h \nto show commands."

    let getLinkParams (args: string[]) =
        let url = args |> Array.tryItem 0
        let title = args |> Array.tryItem 1
        let body = args |> Array.tryItem 2
        (url.Value, title, body)

    let (|Int|_|) str =
       match System.Int32.TryParse(str: string) with
       | (true,int) -> Some(int)
       | _ -> Option.None

    let getDeviceFromIndexOrDeviceId (device: string) : string =
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
            | _ -> None
        | 2 -> match args.[1] with | "key" | "-k" -> DeleteKey | _ -> None
        | _ -> Error NotEnoughArguments

    let listArgument (args: string[]) =
        match args.[1] with
        | "pushes" | "-p" ->
            match args.Length with
            | 3 -> 
                match args.[2] with
                | Int i -> (if i < 1 then 1 else i) |> ListPushes
                | _ -> Error ParameterNotANumber
            | _ -> ListPushes 1
        | "devices" | "-d" -> ListDevices
        | "chats" | "-c" -> ListChats
        | "subscriptions" | "-s" -> ListSubscriptions
        | _ -> None

    let findBaseCommand (args: string[]) : Command =
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
                        | 4 -> PushText (args.[3], (args.[2] |> getDeviceFromIndexOrDeviceId).ToOption())
                        | 5 -> PushNote (args.[3].ToOption(), args.[4].ToOption(), (args.[2] |> getDeviceFromIndexOrDeviceId).ToOption())
                        | _ -> Error NotEnoughArguments
                    | _ -> 
                        match args.Length with
                        | 2 -> PushText (args.[1], Option.None)
                        | 3 -> PushNote (args.[1].ToOption(), args.[2].ToOption(), Option.None)
                                            | _ -> Error NotEnoughArguments
                | _ -> Error NotEnoughArguments
            | "link" | "-u" | "url" ->
                match args.Length with
                | x when x > 1 -> 
                    match args.[1] with
                    | "device" | "-d" -> let (a, b, c) = (args.[3..] |> getLinkParams) in PushLink (a, b, c, (GetDevice (args.[2] |> int) |> followCommands).ToOption())
                    | _ -> match args.Length with 
                            | 2 -> let (a, b, c) = (args.[1..] |> getLinkParams) in PushLink (a, b, c, Option.None) 
                            | _ -> Error NotEnoughArguments
                | _ -> Error NotEnoughArguments
            | "clip" | "-c" -> match args.Length with | 2 -> args.[1] |> PushClip | _ -> Error NotEnoughArguments
            | "pushes" | "-ps" -> match args.Length with | 2 -> args.[1] |> int |> ListPushes | _ -> Error NotEnoughArguments
            | "delete" | "-d" | "--del" -> args |> delArgument
            | "devices" | "-ds" -> ListDevices
            | "device" | "-di" -> match args.Length with | 2 -> args.[1] |> getDeviceFromIndexOrDeviceId |> GetDeviceInfo | _ -> Error NotEnoughArguments
            | "chats" | "-cs" -> ListChats
            | "subscriptions" | "subs" | "-s" -> ListSubscriptions
            | "channelinfo" | "-ci" -> match args.Length with | 2 -> args.[1] |> ChannelInfo | _ -> Error NotEnoughArguments
            | "list" | "-l" -> match args.Length with | x when x > 1 -> args |> listArgument | _ -> Error NotEnoughArguments
            | "help" | "-h" -> Help
            | _ -> None
        else
            None

    [<EntryPoint>]
    let main ([<System.ParamArray>] argv: string[]): int =

        let command = argv |> findBaseCommand

        let breakup = not(command.IsSetKeyCommand) && SystemCommands.getKey() = ""

        if not breakup then 
            printfn $"{command |> followCommands}"
        else 
            printfn "You have to set your API key with: \"key o.Abc12345xyz\""
        0