namespace Pushbullet

open System
open System.Reflection
open Resources
open Utilities

module SystemCommands =
    
    [<Literal>]
    let HelpFile = "pushbullet-cli.Resources.Help.md"

    type SetKeyCommand = SetKeyCommand of string

    let getKey() = 
        VariableAccess.getSystemKey()
        
    let setKey (SetKeyCommand key) =
        VariableAccess.setSystemKey key
        KeySet.ResourceString

    let deleteKey() =
        VariableAccess.setSystemKey ""
        KeyRemoved.ResourceString

    let getProfile () =
        HttpService.GetRequest "users/me" []
        |> UserResponse.Parse
        |> fun user -> $"[{user.Iden}]:\nname: {user.Name}\nemail: {user.Email}\ncreated: {user.Created |> unixTimestampToDateTime}\nmodified: {user.Modified |> unixTimestampToDateTime}"

    let getLimits () =
        let response = HttpService.GetResponse "users/me" 
        match response with 
        | HttpService.Ok r -> $"API-Limit: {r.Limit}\nRemaining: {r.Remaining}\nReset at:  {(r.Reset |> decimal |> unixTimestampToDateTime)}"
        | HttpService.Error s -> s

    let listGrants () =
        HttpService.GetListRequest "grants"
        |> DataResponse.Parse
        |> fun r -> r.Grants 
        |> Array.map (fun grant -> $"[{grant.Iden}] {grant.Client.Name}, created: {grant.Created |> unixTimestampToDateTime}, modified: {grant.Modified |> unixTimestampToDateTime}")
        |> String.concat Environment.NewLine

    let getVersion () =
        Assembly.GetExecutingAssembly().GetName().Version |> string