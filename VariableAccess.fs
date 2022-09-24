namespace Pushbullet

open System

module VariableAccess =

    [<Literal>]
    let PushbulletKey = "PUSHBULLET_KEY"

    let getSystemKey () =
        (Environment.GetEnvironmentVariable(PushbulletKey, EnvironmentVariableTarget.User): string)
        |> Option.ofObj
        |> Option.defaultValue ""

    let setSystemKey key =
        Environment.SetEnvironmentVariable(PushbulletKey, key, EnvironmentVariableTarget.User)
