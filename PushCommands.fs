namespace Pushbullet

open System
open System.Net
open FSharp.Data
open Newtonsoft.Json
open Newtonsoft.Json.Serialization

type LowercaseContractResolver () =
    inherit DefaultContractResolver ()
        override _.ResolvePropertyName (propertyName: string) =
            propertyName.ToLower()

module PushCommands =

    [<Literal>]
    let PushUrl = "https://api.pushbullet.com/v2/pushes"

    let header = [("Access-Token", SystemCommands.getKey); (HttpRequestHeaders.ContentType "application/json")]

    let toJson a =
        let settings = JsonSerializerSettings()
        settings.NullValueHandling <- NullValueHandling.Ignore
        settings.ContractResolver <- (LowercaseContractResolver() :> IContractResolver)
        JsonConvert.SerializeObject(a, settings)

    let get (parameters: list<string * string>) =
        try
            Http.RequestString(PushUrl, headers = header, query = parameters)
        with
        | :? WebException as ex -> $"{ex.Message}"


    let push (json: string) =
        try
            Http.RequestString(PushUrl, httpMethod = "POST", headers = header, body = TextRequest json) |> ignore
            "Push sent."
        with
        | :? WebException as ex -> $"{ex.Message}"

    let delete (id: string, message: string) =
        try
            Http.RequestString($"{PushUrl}/{id}", httpMethod = "DELETE", headers = header) |> ignore
            message
        with
        | :? WebException as ex -> $"{ex.Message}"

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

    let listPushes (limit: int) =
        let limit = if limit <= 0 then "1" else $"{limit}"
        [("limit", limit)] |> get

    let deletePush (pushId: string) =
        (pushId, "Push deleted!") |> delete