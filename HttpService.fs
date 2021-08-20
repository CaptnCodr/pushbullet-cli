namespace Pushbullet

open System.IO
open System.Net
open FsHttp
open FsHttp.DslCE
open Newtonsoft.Json

module HttpService =

    type Response = 
        | Ok of HeaderResponse 
        | Error of string
    and HeaderResponse = { Limit: string; Remaining: string; Reset: string }

    [<Literal>]
    let BaseUrl = "https://api.pushbullet.com/v2"

    let private formatException (stream: Stream) =
        new StreamReader(stream)
        |> fun r -> r.ReadToEnd() |> ErrorResponse.Parse
        |> fun e -> $"{e.ErrorCode}: {e.Error.Message} {e.Error.Cat}"

    let toJson value =
        (value, JsonExtensions.GetSettings()) |> JsonConvert.SerializeObject

    let private examineResponse (response: Domain.Response) =
        match response.statusCode with 
        | HttpStatusCode.OK -> Choice1Of2 response
        | _ -> Choice2Of2 response

    let private chooseGetResponse (response: Choice<Domain.Response, Domain.Response>) =
        match response with
        | Choice1Of2 r -> r |> Response.toString 16000
        | Choice2Of2 e -> e |> Response.toStream |> formatException

    let private chooseResponseWithMessage (successMessage: string) (response: Choice<Domain.Response, Domain.Response>) =
        match response with
        | Choice1Of2 r -> 
            match successMessage with
            | "" -> r |> Response.toString 16000
            | _ -> successMessage
        | Choice2Of2 e -> e |> Response.toStream |> formatException
        
    let private chooseHeaders (response: Choice<Domain.Response, Domain.Response>) =
        match response with 
        | Choice1Of2 r -> r.headers  |> (fun h -> { Limit = (h.GetValues("X-Ratelimit-Limit") |> Seq.toArray |> fun r -> r.[0]);
                                            Remaining = (h.GetValues("X-Ratelimit-Remaining") |> Seq.toArray |> fun r -> r.[0]);
                                            Reset = (h.GetValues("X-Ratelimit-Reset") |> Seq.toArray |> fun r -> r.[0]) }) |> Ok
        | Choice2Of2 e -> e |> Response.toStream |> formatException |> Error


    let GetRequest (path: string) (query': (string * string) list) : string =
        http {
            GET $"{BaseUrl}/{path}"
            query query'
            Header ("Access-Token") (VariableAccess.getSystemKey())
        } |> (examineResponse >> chooseGetResponse)
        
    let GetListRequest (path: string) : string = GetRequest path [("active", "true")]

    let PostRequest (path: string) (record: 't) (successMessage: string) =
        http {
            POST $"{BaseUrl}/{path}"
            Header ("Access-Token") (VariableAccess.getSystemKey())
            body
            json (record |> toJson)
        } |> examineResponse |> chooseResponseWithMessage successMessage

    let DeleteRequest (path: string) (successMessage: string) =
        http {
            DELETE $"{BaseUrl}/{path}"
            Header ("Access-Token") (VariableAccess.getSystemKey())
        } |> examineResponse |> chooseResponseWithMessage successMessage

    let GetResponse (path: string) =
        http {
            GET $"{BaseUrl}/{path}"
            Header ("Access-Token") (VariableAccess.getSystemKey())
        } |> (examineResponse >> chooseHeaders)
