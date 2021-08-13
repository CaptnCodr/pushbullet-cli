namespace Pushbullet

open System
open System.IO
open System.Reflection
open Utilities

module SystemCommands =
    
    let getKey() = 
        VariableAccess.getSystemKey()
        
    let setKey key =
        VariableAccess.setSystemKey key
        "Key set!"

    let deleteKey() =
        VariableAccess.setSystemKey ""
        "Key deleted!"

    let getProfile () =
        HttpService.GetRequest "users/me" []
        |> UserResponse.Parse
        |> fun user -> $"[{user.Iden}]:\nname: {user.Name}\nemail: {user.Email}\ncreated: {user.Created |> unixTimestampToDateTime}\nmodified: {user.Modified |> unixTimestampToDateTime}"

    let getLimits () =
        let response = HttpService.GetResponse "users/me" 
        match response with 
        | HttpService.Ok r ->
            $"API-Limit: {r.Limit},\nRemaining: {r.Remaining},\nReset at:  {(r.Reset |> decimal |> unixTimestampToDateTime)}"
        | HttpService.Error s -> s

    let listGrants () =
        HttpService.GetListRequest "grants"
        |> DataResponse.Parse
        |> fun r -> r.Grants 
        |> Array.map (fun grant -> $"[{grant.Iden}] {grant.Client.Name}, created: {grant.Created |> unixTimestampToDateTime}, modified: {grant.Modified |> unixTimestampToDateTime}")
        |> String.concat Environment.NewLine

    let getHelp () =
        Assembly.GetExecutingAssembly().GetManifestResourceStream("pushbullet-cli.Resources.Help.md")
        |> fun stream -> new StreamReader(stream)
        |> fun sr -> sr.ReadToEnd()