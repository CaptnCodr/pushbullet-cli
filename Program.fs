namespace Pushbullet

open System

module Program =

    type System.String with
        member x.ToOption () =
            if String.IsNullOrWhiteSpace x then None else Some x

    type Errors =
        | NotEnoughArguments

        member this.GetMessage () =
            match this with
            | NotEnoughArguments -> "Not enough arguments!"

    type Command =
        | GetKey
        | SetKey of string
        | GetMe
        | GetLimits
        | PushText of string
        | PushNote of string option * string option
        | PushLink of string * string option * string option
        | ListPushes of int
        | DeletePush of string
        | ListDevices
        | DeleteDevice of string
        | ListChats
        | DeleteChat of string
        | ListSubscriptions
        | DeleteSubscription of string
        | Error of Errors
        | None

        member this.IsSetKeyCommand = match this with | SetKey _ -> true | _ -> false

    let getLinkParams (args: string[]) =
        let url = args |> Array.tryItem 0
        let title = args |> Array.tryItem 1
        let body = args |> Array.tryItem 2
        (url.Value, title, body)

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
            String.Empty |> SetKey
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
                if args.Length = 2 then
                    PushText args.[1]
                else if args.Length = 3 then
                    PushNote (args.[1].ToOption(), args.[2].ToOption())
                else Error NotEnoughArguments
            | "link" | "-u" | "url" | "link" ->
                if args.Length > 1 then
                    PushLink (args.[1..] |> getLinkParams)
                else Error NotEnoughArguments
            | "pushes" | "-ps" ->
                if args.Length > 1
                then ListPushes (args.[1] |> int) else ListPushes 0
            | "delete" | "-d" | "--del" ->
                delArgument args
            | "devices" | "-ds" ->
                ListDevices
            | "chats" | "-cs" ->
                ListChats
            | "subscriptions" | "subs" | "-s" ->
                ListSubscriptions
            /// Add more commands.
            | _ -> None
        else
            None

    let followCommands command =
        match command with
        | GetKey -> SystemCommands.getKey
        | SetKey k -> SystemCommands.setKey k; "Key set!"
        | GetMe -> SystemCommands.getMe()
        | GetLimits -> SystemCommands.getLimits()

        | PushText t -> PushCommands.pushText t
        | PushNote (t, b) -> PushCommands.pushNote t b
        | PushLink (u, t, b) -> PushCommands.pushLink u t b
        | ListPushes l -> PushCommands.list l
        | DeletePush p -> PushCommands.delete p

        | ListDevices -> DeviceCommands.list
        | DeleteDevice d -> DeviceCommands.delete d

        | ListChats -> ChatCommands.list
        | DeleteChat c -> ChatCommands.delete c

        | ListSubscriptions -> SubscriptionCommands.list
        | DeleteSubscription -> SubscriptionCommands.delete

        | Error e -> e.GetMessage()
        | _ -> "Command not found!"

    [<EntryPoint>]
    let main ([<ParamArray>] argv: string[]): int =

        let command = findBaseCommand argv

        let breakup = SystemCommands.getKey = "" && not(command.IsSetKeyCommand)

        if not breakup then
            followCommands command |> Console.WriteLine
        else
            Console.WriteLine("You have to set your API key with: \"key o.Abc12345xyz\" ")

        0