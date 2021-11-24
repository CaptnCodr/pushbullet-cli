namespace Pushbullet

open System.Reflection
open System.Resources

module Resources =

    [<Literal>]
    let ResourceFile = "pushbullet-cli.Resources.Strings"

    type ResourceTypes =
        | Info_SetupKey
        | Info_CommandNotFound

        | KeySet
        | KeyRemoved

        | ChatCreated
        | ChatMuted
        | ChatUnmuted
        | ChatDeleted

        | DeviceDeleted

        | MessageSent
        | MessageDeleted

        | PushSent
        | LinkSent
        | ClipSent
        | PushDeleted

        | SubscriptionDeleted

        | Errors_NotEnoughArguments
        | Errors_ParameterInvalid 
        | Errors_NoParametersGiven

        member this.ResourceString = 
            this.ToString() 
            |> ResourceManager(ResourceFile, Assembly.GetExecutingAssembly()).GetString