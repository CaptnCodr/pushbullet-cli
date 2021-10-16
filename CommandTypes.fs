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
        | SetKey of SystemCommands.SetKeyCommand
        | DeleteKey
        | GetProfile
        | GetLimits
        | ListGrants
        | GetHelp

        // Push commands
        | PushText of PushCommands.PushTextCommand
        | PushNote of PushCommands.PushNoteCommand
        | PushLink of PushCommands.PushLinkCommand
        | PushClip of PushCommands.PushClipCommand
        | ListPushes of PushCommands.ListPushesCommand
        | GetPush of PushCommands.GetPushCommand
        | DeletePush of PushCommands.DeletePushCommand

        // Sms commands
        | SendMessage of MessageCommands.SendMessageCommand
        | DeleteMessage of MessageCommands.DeleteMessageCommand

        // Device commands
        | ListDevices
        | GetDeviceInfo of DeviceCommands.GetDeviceInfoCommand
        | GetDevice of DeviceCommands.GetDeviceCommand
        | DeleteDevice of DeviceCommands.DeleteDeviceCommand

        // Chat commands
        | ListChats
        | UpdateChat of ChatCommands.UpdateChatCommand
        | CreateChat of ChatCommands.CreateChatCommand
        | DeleteChat of ChatCommands.DeleteChatCommand

        // Subscription commands
        | ListSubscriptions
        | GetChannelInfo of SubscriptionCommands.GetChannelInfoCommand
        | DeleteSubscription of SubscriptionCommands.DeleteSubscriptionCommand

        // misc
        | Error of Errors
        | Other of string

        member this.IsSetKeyCommand = match this with | SetKey _ -> true | _ -> false
