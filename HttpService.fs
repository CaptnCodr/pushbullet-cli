namespace Pushbullet

open System.Net
open FSharp.Data
open Pushbullet
open CommandHelper

module HttpService =

    [<Literal>]
    let BaseUrl = "https://api.pushbullet.com/v2"
    
    let private getHeader() = 
        [("Access-Token", getSystemKey()); (HttpRequestHeaders.ContentType "application/json")]

    let GetRequest (path: string) (query: (string * string) list): string =
        try
            Http.RequestString($"{BaseUrl}/{path}", httpMethod = "GET", headers = getHeader(), query = query)
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException

    let PostRequest (path: string) (json: string) (successMessage: string) =
        try 
            Http.RequestString($"{BaseUrl}{path}", httpMethod = "POST", headers = getHeader(), body = TextRequest json) |> ignore
            successMessage
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException

    let DeleteRequest (path: string) (successMessage: string)  =
        try
            Http.RequestString($"{BaseUrl}/{path}", httpMethod = "DELETE", headers = getHeader()) |> ignore
            successMessage
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException


    let GetResponse (path: string) =
        Http.Request($"{BaseUrl}/{path}", headers = getHeader())