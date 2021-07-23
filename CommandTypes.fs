namespace Pushbullet

open System

module CommandTypes =

    type System.String with
        member x.ToOption () =
            if String.IsNullOrWhiteSpace x then None else Some x

    type Errors =
        | NotEnoughArguments
        | ArrayIndexNotFound

        member this.GetMessage () =
            match this with
            | NotEnoughArguments -> "Not enough arguments!"
            | ArrayIndexNotFound -> "Index of array not found!"

    type Command =
        | GetKey
        | SetKey of string
        | DeleteKey
        | GetMe
        | GetLimits

        | PushText of string * string option
        | PushNote of string option * string option * string option
        | PushLink of string * string option * string option * string option
        | PushClip of string
        | ListPushes of int
        | DeletePush of string

        | ListDevices
        | GetDeviceInfo of string
        | GetDevice of int
        | DeleteDevice of string

        | ListChats
        | DeleteChat of string

        | ListSubscriptions
        | ChannelInfo of string
        | DeleteSubscription of string

        | Error of Errors
        | None

        member this.IsSetKeyCommand = match this with | SetKey _ -> true | _ -> false
