namespace Pushbullet

module CommandTypes =
    
    type Errors =
        | NotEnoughArguments
        | ParameterInvalid
        | NoParametersGiven

        member x.GetMessage () =
            match x with
            | NotEnoughArguments -> "Not enough arguments!\n\nShow commands with:\npb help | -h"
            | ParameterInvalid -> "Parameter is invalid!"
            | NoParametersGiven -> "No parameters given.\n\nShow commands with:\npb help | -h"

    type Commands =

        // System commands
        | GetKey
        | SetKey of string
        | DeleteKey
        | GetProfile
        | GetLimits
        | ListGrants
        | GetHelp

        // Push commands
        | PushText of string * string option
        | PushNote of string option * string option * string option
        | PushLink of string * string option * string option * string option
        | PushClip of string
        | ListPushes of int
        | GetPush of string
        | DeletePush of string

        // Sms commands
        | SendMessage of string * string * string
        | DeleteMessage of string

        // Device commands
        | ListDevices
        | GetDeviceInfo of string
        | GetDevice of int
        | DeleteDevice of string

        // Chat commands
        | ListChats
        | UpdateChat of string * bool
        | CreateChat of string
        | DeleteChat of string

        // Subscription commands
        | ListSubscriptions
        | GetChannelInfo of string
        | DeleteSubscription of string

        // misc
        | Error of Errors
        | Other of string

        member this.IsSetKeyCommand = match this with | SetKey _ -> true | _ -> false
