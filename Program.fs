namespace Pushbullet

open CommandTypes
open Patterns

module Program =

    let followCommands command =
        match command with
        | GetKey -> SystemCommands.getKey()
        | SetKey k -> SystemCommands.setKey k
        | DeleteKey -> SystemCommands.deleteKey()
        | GetProfile -> SystemCommands.getProfile()
        | GetLimits -> SystemCommands.getLimits()
        | ListGrants -> SystemCommands.listGrants()
        | GetHelp -> SystemCommands.getHelp()

        | PushText (t, d) -> PushCommands.pushText t d
        | PushNote (t, b, d) -> PushCommands.pushNote t b d
        | PushLink (u, t, b, d) -> PushCommands.pushLink u t b d
        | PushClip c -> PushCommands.pushClip c
        | ListPushes l -> PushCommands.list l
        | GetPush p -> PushCommands.getSinglePush p
        | DeletePush p -> PushCommands.delete p

        | SendMessage (d, n, m) -> MessageCommands.create d n m
        | DeleteMessage m -> MessageCommands.delete m

        | ListDevices -> DeviceCommands.list()
        | GetDeviceInfo s -> DeviceCommands.getDeviceInfo s
        | GetDevice i -> DeviceCommands.getDeviceId i
        | DeleteDevice d -> DeviceCommands.delete d

        | ListChats -> ChatCommands.list()
        | UpdateChat (i, s) -> ChatCommands.update i s
        | CreateChat c -> ChatCommands.create c
        | DeleteChat c -> ChatCommands.delete c

        | ListSubscriptions -> SubscriptionCommands.list()
        | GetChannelInfo t -> SubscriptionCommands.channelInfo t
        | DeleteSubscription d -> SubscriptionCommands.delete d

        | Error e -> e.GetMessage()
        | Other s -> $"Command '{s}' not found!\n\nUse:\npb help | -h \nto show commands."

    let toOption x =
        if System.String.IsNullOrWhiteSpace x then None else Some x

    let getLinkParams args =
        ((args |> Array.tryItem 0).Value, args |> Array.tryItem 1, args |> Array.tryItem 2)

    let (|Int|_|) str =
       match System.Int32.TryParse(str: string) with
       | (true, int) -> Some int
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

    let findBaseCommand (args: string[]) =
        if args.Length > 0 then
            match args.[0] with
            | Key _ -> match args.Length with | 2 -> args.[1] |> SetKey | _ -> GetKey
            | Profile _ -> GetProfile
            | Limits _ -> GetLimits
            | Grants _ -> ListGrants
            | PushInfo _ -> match args.Length with | 2 -> GetPush args.[1] | _ -> Error NotEnoughArguments
            | Push _ | Text _ ->
                match args.Length with
                | x when x > 1 -> 
                    match args.[1] with 
                    | Device _ -> 
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
            | Link _ | Url _ ->
                match args.Length with
                | x when x > 1 -> 
                    match args.[1] with
                    | Device _ -> let (a, b, c) = (args.[3..] |> getLinkParams) in PushLink (a, b, c, args.[2] |> int |> GetDevice |> followCommands |> toOption)
                    | _ -> 
                        match args.Length with 
                        | y when y > 2 -> let (a, b, c) = (args.[1..] |> getLinkParams) in PushLink (a, b, c, None) 
                        | _ -> Error NotEnoughArguments
                | _ -> Error NotEnoughArguments
            | Clip _ -> match args.Length with | 2 -> args.[1] |> PushClip | _ -> Error NotEnoughArguments
            | Pushes _ -> match args.Length with | 1 -> ListPushes 0 | 2 -> args.[1] |> int |> ListPushes | _ -> Error NotEnoughArguments
            | Delete _ -> 
                match args.Length with
                | 3 -> 
                    match args.[1] with
                    | Push _ -> args.[2] |> DeletePush
                    | Chat _ -> args.[2] |> DeleteChat
                    | Device _ -> args.[2] |> DeleteDevice
                    | Subscription _ -> args.[2] |> DeleteSubscription
                    | Sms _ -> args.[2] |> DeleteMessage
                    | e -> Other e
                | 2 -> match args.[1] with | Key _ -> DeleteKey | e -> Other e
                | _ -> Error NotEnoughArguments
            | Sms _ -> 
                match args.Length with 
                | 4 -> (args.[1] |> getDeviceFromIndexOrDeviceId, args.[2], args.[3]) |> SendMessage
                | _ -> Error NotEnoughArguments
            | Devices _ -> ListDevices
            | Device _ -> 
                match args.Length with 
                | 2 -> match args.[1] |> getDeviceFromIndexOrDeviceId |> toOption with | Some x -> x |> GetDeviceInfo | None -> Error ParameterInvalid
                | _ -> Error NotEnoughArguments
            | Chats _ -> ListChats
            | Chat _ ->
                match args.Length with
                | 3 -> match args.[2] |> valueToBool with | Some b -> (args.[1], b) |> UpdateChat | None -> Error ParameterInvalid
                | 2 -> args.[1] |> CreateChat
                | _ -> Error NotEnoughArguments
            | Subscriptions _ -> ListSubscriptions
            | ChannelInfo _ -> match args.Length with | 2 -> args.[1] |> GetChannelInfo | _ -> Error NotEnoughArguments
            | Help _ -> GetHelp
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