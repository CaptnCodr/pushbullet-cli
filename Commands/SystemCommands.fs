namespace Pushbullet

open System
open System.IO
open System.Reflection
open FSharp.Data
open CommandHelper

module SystemCommands =
    
    let getKey() = 
        VariableAccess.getSystemKey()
        
    let setKey key =
        VariableAccess.setSystemKey key
        "Key set!"

    let deleteKey() =
        VariableAccess.setSystemKey ""
        "Key deleted!"

    let getHeader() = 
        [("Access-Token", getKey()); (HttpRequestHeaders.ContentType "application/json")]

    let getProfile () =
        HttpService.GetRequest "users/me" []
        |> UserResponse.Parse
        |> fun user -> $"[{user.Iden}]:\nname: {user.Name}\nemail: {user.Email}\ncreated: {user.Created |> unixTimestampToDateTime}\nmodified: {user.Modified |> unixTimestampToDateTime}"

    let getLimits () =
        let response = HttpService.GetResponse "users/me" 
        match response with 
        | HttpService.Ok r ->
            let limit = r.Headers.["X-Ratelimit-Limit"]
            let remaining = r.Headers.["X-Ratelimit-Remaining"]
            let dt = r.Headers.["X-Ratelimit-Reset"] |> decimal |> unixTimestampToDateTime
            $"API-Limit: {limit},\nRemaining: {remaining},\nReset at:  {dt}"
        | HttpService.Error s -> s

    let listGrants () =
        HttpService.GetRequest "grants" [Actives]
        |> DataResponse.Parse
        |> fun r -> r.Grants 
        |> Array.map (fun grant -> $"[{grant.Iden}] {grant.Client.Name}, created: {grant.Created |> unixTimestampToDateTime}, modified: {grant.Modified |> unixTimestampToDateTime}")
        |> String.concat Environment.NewLine

    let getHelp () =
        Assembly.GetExecutingAssembly().GetManifestResourceStream("pushbullet-cli.Resources.Help.md")
        |> fun stream -> new StreamReader(stream)
        |> fun sr -> sr.ReadToEnd()