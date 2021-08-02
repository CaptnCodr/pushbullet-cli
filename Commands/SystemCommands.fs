namespace Pushbullet

open System
open System.Net
open FSharp.Data
open CommandHelper

module SystemCommands =

    
    let getKey() = getSystemKey()
        
    let setKey key =
        Environment.SetEnvironmentVariable(PushbulletKey, key, EnvironmentVariableTarget.User)
        "Key set!"

    let deleteKey() =
        Environment.SetEnvironmentVariable(PushbulletKey, "", EnvironmentVariableTarget.User)
        "Key deleted!"

    let getHeader() = 
        [("Access-Token", getKey()); (HttpRequestHeaders.ContentType "application/json")]

    let getProfile () =
        HttpService.GetRequest "users/me" []
        |> UserResponse.Parse
        |> fun user -> $"[{user.Iden}]:\nname: {user.Name}\nemail: {user.Email}\ncreated: {user.Created |> unixTimestampToDateTime}\nmodified: {user.Modified |> unixTimestampToDateTime}"

    let getLimits () =
        try
            let response = HttpService.GetResponse "users/me" 
            let limit = response.Headers.["X-Ratelimit-Limit"]
            let remaining = response.Headers.["X-Ratelimit-Remaining"]
            let dt = response.Headers.["X-Ratelimit-Reset"] |> int64 |> DateTimeOffset.FromUnixTimeSeconds |> fun d -> d.ToString("dd.MM.yyyy HH:mm")
            $"API-Limit: {limit},\nRemaining: {remaining},\nReset at:  {dt}"
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException

    let listGrants () =
        HttpService.GetRequest "grants" [Actives]
        |> DataResponse.Parse
        |> fun r -> r.Grants 
        |> Array.map (fun grant -> $"[{grant.Iden}] {grant.Client.Name}, created: {grant.Created |> unixTimestampToDateTime}, modified: {grant.Modified |> unixTimestampToDateTime}")
        |> String.concat Environment.NewLine

    let getHelp () =
        "Syntax: pb [command] [subcommand] [arguments]
        
Commands:\n
key | -k [api key]                          Set API key with argument. Show API key without argument.
me | -i                                     Get profile of configured API key.
limits | -x                                 Get rate limits.
grants | -g                                 Get grants that have access to your PB account.
push | -p | text | -t [arguments]           Push text or note. Use push [device / -d] to push to a specific device.
link | -l | url | -u [arguments]            Push a link to device(s). Use push [device / -d] to push to a specific device.
pushes | -ps [number]                       List [number] of pushes or else last push.
devices | -ds                               Lists devices of current account. Including identifiers and indexes to identify.
device | -d [iden / index]                  Shows information about a device. Select with identifier or index shown in the [devices / -ds] command.
chats | -cs                                 List chats of current account.
subscriptions | subs | -s                   List subscriptions with channel tag of current account.
channelinfo | -ci [tag]                     Show information about a specific channel with channel tag as shown in [subscriptions / subs / -s].

delete | --del | remove | -r [subcommand]   Deletes an object:
    push | -p [iden]                        Deletes a push using its iden.
    device | -d [iden]                      Deletes a device using its iden.
    chat | -c [iden]                        Deletes a chat using its iden.
    subscription | -s [iden]                Deletes a subscription using its iden.
    key | -k                                Deletes the current configured API key

help | -h                                   This help."