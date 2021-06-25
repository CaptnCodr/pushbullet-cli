namespace Pushbullet

open System.Net
open FSharp.Data
open System

module SubscriptionCommands =

    let channelInfo (tag: string) =
        try 
            Http.RequestString($"{CommandHelper.BaseUrl}/channel-info", httpMethod = "GET", headers = SystemCommands.getHeader(), query = [("tag", tag)]) 
            |> ChannelInfoResponse.Parse
            |> fun r -> $"(Tag: {r.Tag}, Subscribers: {r.SubscriberCount}) {r.Name}: {r.Description}"
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> CommandHelper.formatException

    let list =

        let formatSubscription (s: DataResponse.Subscription) =
            $"(Tag: {s.Channel.Tag}) {s.Channel.Name}: {s.Channel.Description}"

        try
            Http.RequestString($"{CommandHelper.BaseUrl}/subscriptions", headers = SystemCommands.getHeader(), query = [("active", "true")]) 
            |> DataResponse.Parse
            |> fun r -> r.Subscriptions
            |> Array.map formatSubscription
            |> String.concat Environment.NewLine
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> CommandHelper.formatException

    let delete id =
        try
            Http.RequestString($"{CommandHelper.BaseUrl}/subscriptions/{id}", httpMethod = "DELETE", headers = SystemCommands.getHeader()) |> ignore
            "Subscription deleted!"
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> CommandHelper.formatException