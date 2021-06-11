namespace Pushbullet

open System
open System.Net
open FSharp.Data
open System.Net

module SystemCommands =

    [<Literal>]
    let PushbulletKey = "PUSHBULLET_KEY"

    let getKey =
        Environment.GetEnvironmentVariable(PushbulletKey, EnvironmentVariableTarget.User)

    let setKey key =
        Environment.SetEnvironmentVariable(PushbulletKey, key, EnvironmentVariableTarget.User)

    let header = [("Access-Token", getKey); (HttpRequestHeaders.ContentType "application/json")]

    let getMe () =
        try
            Http.RequestString($"{CommandHelper.BaseUrl}/users/me", httpMethod = "GET", headers = header) |> CommandHelper.prettifyJson
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> CommandHelper.formatException

    let getLimits () =
        try
            let response = Http.Request($"{CommandHelper.BaseUrl}/users/me", httpMethod = "GET", headers = header)
            let limit = response.Headers.["X-Ratelimit-Limit"]
            let remaining = response.Headers.["X-Ratelimit-Remaining"]
            let dt = response.Headers.["X-Ratelimit-Reset"] |> int64 |> DateTimeOffset.FromUnixTimeSeconds |> fun d -> d.ToString("dd.MM.yyyy HH:mm")
            $"API-Limit: {limit},\nRemaining: {remaining},\nReset at:  {dt}"
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> CommandHelper.formatException
