namespace Pushbullet

open System

module VariableAccess =
    
    [<Literal>]
    let PushbulletKey = "PUSHBULLET_KEY"
    
    let getSystemKey () =
        match Environment.GetEnvironmentVariable(PushbulletKey, EnvironmentVariableTarget.User) with
        | null -> ""
        | value -> value

    let setSystemKey key =
        Environment.SetEnvironmentVariable(PushbulletKey, key, EnvironmentVariableTarget.User)
