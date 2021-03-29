namespace Pushbullet

open System


module SystemCommands =

    [<Literal>]
    let PushbulletKey = "PUSHBULLET_KEY"

    let getKey =
        Environment.GetEnvironmentVariable(PushbulletKey, EnvironmentVariableTarget.User)

    let setKey key =
        Environment.SetEnvironmentVariable(PushbulletKey, key, EnvironmentVariableTarget.User)