namespace Pushbullet

open System
open FSharp.Data

module SystemCommands =

    [<Literal>]
    let PushbulletKey = "PUSHBULLET_KEY"

    let getKey =
        Environment.GetEnvironmentVariable(PushbulletKey, EnvironmentVariableTarget.User)

    let setKey key =
        Environment.SetEnvironmentVariable(PushbulletKey, key, EnvironmentVariableTarget.User)

    let getMe () =
        let header = [("Access-Token", getKey); (HttpRequestHeaders.ContentType "application/json")]
        Http.RequestString("https://api.pushbullet.com/v2/users/me", httpMethod = "GET", headers = header)
