namespace Pushbullet

open System.Net
open FSharp.Data
open System

module SubscriptionCommands =

    let list =

        let formatSubscription (s: DataResponse.Subscription) =
            $"{s.Channel.Name}: {s.Channel.Description}"

        try
            Http.RequestString($"{CommandHelper.BaseUrl}/subscriptions", headers = SystemCommands.header, query = [("active", "true")]) 
            |> DataResponse.Parse
            |> fun r -> r.Subscriptions
            |> Array.map formatSubscription
            |> String.concat Environment.NewLine
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> CommandHelper.formatException

    let delete id =
        try
            Http.RequestString($"{CommandHelper.BaseUrl}/subscriptions/{id}", httpMethod = "DELETE", headers = SystemCommands.header) |> ignore
            "Subscription deleted!"
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> CommandHelper.formatException