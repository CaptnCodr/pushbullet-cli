namespace Pushbullet

open System.Net
open FSharp.Data

module SubscriptionCommands =

    let list =
        try
            Http.RequestString($"{CommandHelper.BaseUrl}/subscriptions", headers = SystemCommands.header, query = [("active", "true")]) |> CommandHelper.prettifyJson
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> CommandHelper.formatException

    let delete id =
        try
            Http.RequestString($"{CommandHelper.BaseUrl}/subscriptions/{id}", httpMethod = "DELETE", headers = SystemCommands.header) |> ignore
            "Subscription deleted!"
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> CommandHelper.formatException