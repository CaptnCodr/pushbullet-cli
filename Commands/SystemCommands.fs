namespace Pushbullet

open System
open System.Net
open FSharp.Data
open CommandHelper

module SystemCommands =

    [<Literal>]
    let PushbulletKey = "PUSHBULLET_KEY"
    
    let getKey() =
        Environment.GetEnvironmentVariable(PushbulletKey, EnvironmentVariableTarget.User)
        
    let setKey key =
        Environment.SetEnvironmentVariable(PushbulletKey, key, EnvironmentVariableTarget.User)
        "Key set!"

    let deleteKey() =
        Environment.SetEnvironmentVariable(PushbulletKey, "", EnvironmentVariableTarget.User)
        "Key deleted!"

    let getHeader() = 
        [("Access-Token", getKey()); (HttpRequestHeaders.ContentType "application/json")]

    let getMe () =
        try
            Http.RequestString($"{BaseUrl}/users/me", httpMethod = "GET", headers = getHeader()) 
            |> UserResponse.Parse
            |> fun user -> $"[{user.Iden}]:\nname: {user.Name}\nemail: {user.EmailNormalized})\ncreated: {user.Created |> unixTimestampToDateTime}\nmodified: {user.Modified |> unixTimestampToDateTime}"
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException

    let getLimits () =
        try
            let response = Http.Request($"{BaseUrl}/users/me", httpMethod = "GET", headers = getHeader())
            let limit = response.Headers.["X-Ratelimit-Limit"]
            let remaining = response.Headers.["X-Ratelimit-Remaining"]
            let dt = response.Headers.["X-Ratelimit-Reset"] |> int64 |> DateTimeOffset.FromUnixTimeSeconds |> fun d -> d.ToString("dd.MM.yyyy HH:mm")
            $"API-Limit: {limit},\nRemaining: {remaining},\nReset at:  {dt}"
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException
