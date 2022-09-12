namespace Pushbullet

open System
open System.Reflection
open FSharp.Data
open Resources
open Utilities

module SystemCommands =

    type UserResponse = JsonProvider<"./../Data/UserData.json", ResolutionFolder=__SOURCE_DIRECTORY__>
    type GrantListResponse = JsonProvider<"./../Data/GrantList.json", ResolutionFolder=__SOURCE_DIRECTORY__>

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
        |> fun user -> 
            GetProfileOutput.FormattedString(user.Iden, user.Name, user.Email, user.Created |> unixTimestampToDateTime, user.Modified |> unixTimestampToDateTime)

    let getLimits () =
        let response = HttpService.GetResponse "users/me" 
        match response with 
        | HttpService.Ok r -> 
            GetLimitsOutput.FormattedString(r.Limit, r.Remaining, (r.Reset |> decimal |> unixTimestampToDateTime))
        | HttpService.Error s -> s

    let listGrants () =
        HttpService.GetListRequest "grants"
        |> GrantListResponse.Parse
        |> fun r -> r.Grants 
        |> Array.map (fun grant -> ListGrantsOutput.FormattedString(grant.Iden, grant.Client.Name, grant.Created |> unixTimestampToDateTime, grant.Modified |> unixTimestampToDateTime))
        |> String.concat Environment.NewLine

    let getVersion () =
        Assembly.GetExecutingAssembly().GetName().Version |> string