namespace Pushbullet

open System.IO
open System.Net
open FSharp.Data
open Newtonsoft.Json

module HttpService =

    type Response =
        | Ok of HttpResponse
        | Error of string

    [<Literal>]
    let BaseUrl = "https://api.pushbullet.com/v2"

    let private getHeader () =
        [ ("Access-Token", VariableAccess.getSystemKey ()); (HttpRequestHeaders.ContentType "application/json") ]

    let private formatException (stream: Stream) =
        new StreamReader(stream)
        |> fun r -> r.ReadToEnd() |> ErrorResponse.Parse
        |> fun e -> $"{e.ErrorCode}: {e.Error.Message} {e.Error.Cat}"

    let toJson value =
        (value, JsonExtensions.GetSettings()) |> JsonConvert.SerializeObject

    let GetRequest (path: string) (query: (string * string) list) : string =
        try
            Http.RequestString($"{BaseUrl}/{path}", httpMethod = "GET", headers = getHeader (), query = query)
        with :? WebException as ex -> ex.Response.GetResponseStream() |> formatException

    let PostRequest (path: string) (record: 't) (successMessage: string) =
        try
            Http.RequestString($"{BaseUrl}/{path}", httpMethod = "POST", headers = getHeader (), body = TextRequest (record |> toJson)) |> ignore
            successMessage
        with :? WebException as ex -> ex.Response.GetResponseStream() |> formatException

    let DeleteRequest (path: string) (successMessage: string) =
        try
            Http.RequestString($"{BaseUrl}/{path}", httpMethod = "DELETE", headers = getHeader ()) |> ignore
            successMessage
        with :? WebException as ex -> ex.Response.GetResponseStream() |> formatException

    let GetResponse (path: string) =
        try
            Http.Request($"{BaseUrl}/{path}", headers = getHeader ()) |> Ok
        with :? WebException as ex -> ex.Response.GetResponseStream() |> formatException |> Error
