namespace Pushbullet

open System
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
       match Int32.TryParse(str:string) with
       | (true,int) -> Some(int)
       | _ -> Option.None

    let getDeviceFromIndexOrDeviceId (device: string) : string =
        match device with
        | Int i -> GetDevice (i |> int) |> followCommands
        | _ -> device

    let delArgument (args: string[]) =
        match args.[1] with
        | "push" | "-p" ->
            if args.Length > 2
            then DeletePush (args.[2])
            else Error NotEnoughArguments
        | "chat" | "-c" ->
            if args.Length > 2
            then DeleteChat (args.[2])
            else Error NotEnoughArguments
        | "device" | "-d" ->
            if args.Length > 2
            then DeleteDevice (args.[2])
            else Error NotEnoughArguments
        | "subscription" | "-s" ->
            if args.Length > 2
            then DeleteSubscription (args.[2])
            else Error NotEnoughArguments
        | "key" | "-k" ->
            DeleteKey
        | _ -> None

    let listArgument (args: string[]) =
        match args.[1] with
        | "pushes" | "-p" ->
            if args.Length > 1 
            then ListPushes (args.[2] |> int) 
            else ListPushes 0
        | "devices" | "-d" ->
            ListDevices
        | "chats" | "-c" ->
            ListChats
        | "subscriptions" | "-s" ->
            ListSubscriptions
        | _ -> None

    let findBaseCommand (args: string[]) : Command =
        if args.Length > 0 then
            match args.[0] with
            | "key" | "-k" ->
                if args.Length > 1
                then SetKey args.[1]
                else GetKey
            | "me" | "-i" -> GetMe
            | "limits" | "-x" -> GetLimits
            | "push" | "-p" | "-t" | "text" ->
                if args.[1] = "-d" || args.[1] = "device" then
                    let id = args.[2] |> getDeviceFromIndexOrDeviceId
                    if args.Length = 4 then
                        PushText (args.[3], id.ToOption())
                    else if args.Length = 5 then
                        PushNote (args.[3].ToOption(), args.[4].ToOption(), id.ToOption())
                    else Error NotEnoughArguments
                else
                    if args.Length = 2 then
                        PushText (args.[1], Option.None)
                    else if args.Length = 3 then
                        PushNote (args.[1].ToOption(), args.[2].ToOption(), Option.None)
                    else Error NotEnoughArguments
            | "link" | "-u" | "url" ->
                if args.[1] = "-d" || args.[1] = "device" then
                    let id = GetDevice (args.[2] |> int) |> followCommands
                    let (a, b, c) = (args.[3..] |> getLinkParams)
                    PushLink (a, b, c, id.ToOption())
                else
                    if args.Length > 1 then
                        let (a, b, c) = (args.[1..] |> getLinkParams)
                        PushLink (a, b, c, Option.None)
                    else Error NotEnoughArguments

            | "clip" | "-c" ->
                if args.Length > 1
                then PushClip args.[1]
                else Error NotEnoughArguments
            | "pushes" | "-ps" ->
                if args.Length > 1
                then ListPushes (args.[1] |> int) 
                else ListPushes 0
            | "delete" | "-d" | "--del" ->
                delArgument args
            | "devices" | "-ds" ->
                ListDevices
            | "device" | "-di" ->
                if args.Length > 1 then 
                    let id = args.[1] |> getDeviceFromIndexOrDeviceId
                    GetDeviceInfo id
                else
                    Error NotEnoughArguments
            | "chats" | "-cs" ->
                ListChats
            | "subscriptions" | "subs" | "-s" ->
                ListSubscriptions
            | "channelinfo" | "-ci" ->
                if args.Length > 1
                then ChannelInfo args.[1]
                else Error NotEnoughArguments
            | "list" | "-l" ->
                listArgument args
            | "help" | "-h" ->
                Help
            /// Add more commands.
            | _ -> None
        else
            None


    [<EntryPoint>]
    let main ([<ParamArray>] argv: string[]): int =

        let command = findBaseCommand argv

        let breakup = not(command.IsSetKeyCommand) && SystemCommands.getKey() = ""

        if not breakup 
        then followCommands command |> Console.WriteLine
        else Console.WriteLine("You have to set your API key with: \"key o.Abc12345xyz\" ")

        0