namespace Pushbullet

open System.IO
open System.Net
open FsHttp
open Newtonsoft.Json
open FSharp.Data
open Resources

module HttpService =

    type ErrorResponse = JsonProvider<"./Data/Error.json", ResolutionFolder=__SOURCE_DIRECTORY__>

    type Response =
        | Ok of HeaderResponse
        | Error of string

    and HeaderResponse =
        { Limit: string
          Remaining: string
          Reset: string }

    [<Literal>]
    let BaseUrl = "https://api.pushbullet.com/v2"

    let private formatException (stream: Stream) =
        new StreamReader(stream)
        |> fun r -> r.ReadToEnd()
        |> ErrorResponse.Parse
        |> fun e -> $"{e.ErrorCode}: {e.Error.Message} {e.Error.Cat}"

    let toJson value =
        (value, JsonExtensions.GetSettings()) |> JsonConvert.SerializeObject

    let private examineResponse (response: Domain.Response) =
        match response.statusCode with
        | HttpStatusCode.OK -> Choice1Of2 response
        | _ -> Choice2Of2 response

    let private chooseGetResponse (response: Choice<Domain.Response, Domain.Response>) =
        match response with
        | Choice1Of2 r -> r |> Response.toText
        | Choice2Of2 e -> e |> Response.toStream |> formatException

    let private chooseResponseWithMessage
        (successMessage: ResourceTypes)
        (response: Choice<Domain.Response, Domain.Response>)
        =
        match response with
        | Choice1Of2 r ->
            match successMessage.ResourceString with
            | "" -> r |> Response.toText
            | _ -> successMessage.ResourceString
        | Choice2Of2 e -> e |> Response.toStream |> formatException

    let private chooseHeaders (response: Choice<Domain.Response, Domain.Response>) =
        match response with
        | Choice1Of2 r ->
            r.headers
            |> (fun h ->
                { Limit = (h.GetValues("X-Ratelimit-Limit") |> Seq.head)
                  Remaining = (h.GetValues("X-Ratelimit-Remaining") |> Seq.head)
                  Reset = (h.GetValues("X-Ratelimit-Reset") |> Seq.head) })
            |> Ok
        | Choice2Of2 e -> e |> Response.toStream |> formatException |> Error

    let GetRequest (path: string) (query': (string * obj) list) : string =
        http {
            GET $"{BaseUrl}/{path}"
            query query'
            header ("Access-Token") (VariableAccess.getSystemKey ())
        }
        |> Request.send
        |> (examineResponse >> chooseGetResponse)

    let GetListRequest (path: string) : string = GetRequest path [ ("active", "true") ]

    let PostRequest (path: string) (record: 't) (successMessage: ResourceTypes) =
        http {
            POST $"{BaseUrl}/{path}"
            header ("Access-Token") (VariableAccess.getSystemKey ())
            body
            json (record |> toJson)
            ContentType("application/json")
        }
        |> Request.send
        |> examineResponse
        |> chooseResponseWithMessage successMessage

    let DeleteRequest (path: string) (successMessage: ResourceTypes) =
        http {
            DELETE $"{BaseUrl}/{path}"
            header ("Access-Token") (VariableAccess.getSystemKey ())
        }
        |> Request.send
        |> examineResponse
        |> chooseResponseWithMessage successMessage

    let GetResponse (path: string) =
        http {
            GET $"{BaseUrl}/{path}"
            header ("Access-Token") (VariableAccess.getSystemKey ())
        }
        |> Request.send
        |> (examineResponse >> chooseHeaders)
