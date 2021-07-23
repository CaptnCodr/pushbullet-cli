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

    let getHelp () =
        "Syntax: pb [command] [subcommand] [arguments]
        
Commands:\n
key | -k [api key]                    Set API key with argument. Show API key without argument.
me | -i                               Get profile of configured API key.
limits | -x                           Get rate limits.
push | -p | text | -t [arguments]     Push text or note. Use push [device / -d] to push to a specific device.
link | url | -u [arguments]           Push a link to device(s). Use push [device / -d] to push to a specific device.
pushes | -ps [number]                 List [number] of pushes or else last push.
devices | -ds                         Lists devices of current account. Including identifiers and indexes to identify.
device | -di [iden / index]           Shows information about a device. Select with identifier or index shown in the [devices / -ds] command.
chats | -cs                           List chats of current account.
subscriptions | subs | -s             List subscriptions with channel tag of current account.
channelinfo | -ci [tag]               Show information about a specific channel with channel tag as shown in [subscriptions / subs / -s].

delete | -d | --del [subcommand]      Deletes an object:
    push | -p [iden]                  Deletes a push using its iden.
    device | -d [iden]                Deletes a device using its iden.
    chat | -c [iden]                  Deletes a chat using its iden.
    subscription | -s [iden]          Deletes a subscription using its iden.
    key | -k                          Deletes the current configured API key

list | -l [subcommand]                List objects:
    pushes | -p                       Pushes. Same as [pushes / -ps]
    devices | -d                      Devices. Same as [devices / -ds]
    chats | -c                        Chats. Sames as [chats / -cs]
    subscription | -s                 Subscriptions. Sames as [subscriptions / subs / -s]

help | -h                             This list.
"