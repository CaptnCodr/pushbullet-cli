namespace Pushbullet

open System
open FSharp.Data
open Newtonsoft.Json
open Newtonsoft.Json.Serialization

type LowercaseContractResolver () =
    inherit DefaultContractResolver ()
        override _.ResolvePropertyName (propertyName: string) =
            propertyName.ToLower()

module Commands =

    [<Literal>]
    let PushbulletKey = "PUSHBULLET_KEY"

    let toJson a =
        let settings = JsonSerializerSettings()
        settings.NullValueHandling <- NullValueHandling.Ignore
        settings.ContractResolver <- (LowercaseContractResolver() :> IContractResolver)
        JsonConvert.SerializeObject(a, settings)

    let setKey key =
        Environment.SetEnvironmentVariable(PushbulletKey, key, EnvironmentVariableTarget.User)

    let getKey =
        Environment.GetEnvironmentVariable(PushbulletKey, EnvironmentVariableTarget.User)

    let push (a: string) =
        let pushUri = "https://api.pushbullet.com/v2/pushes"
        let header = [("Access-Token", getKey); (HttpRequestHeaders.ContentType "application/json")]
        Http.RequestString(pushUri, httpMethod = "POST", headers = header, body = TextRequest a ) |> ignore

    let pushText body =
        {| Type = "note"; Body = body |} |> toJson |> push

    let pushNote (title: string option) (body: string option) =
        let title = if title.IsNone
                    then null else title.Value
        let body = if body.IsNone
                    then null else body.Value
        {| Type = "note"; Title = title; Body = body |} |> toJson |> push

    let pushLink (url: string) (title: string option) (body: string option) =
        let title = if title.IsNone
                    then null else title.Value
        let body = if body.IsNone
                    then null else body.Value
        {| Type = "link"; Url = url; Title = title; Body = body |} |> toJson |> push