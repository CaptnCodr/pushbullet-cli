namespace Pushbullet

open System.Net
open FSharp.Data
open System
open CommandHelper

module SubscriptionCommands =

    let channelInfo (tag: string) =
        try 
            Http.RequestString($"{BaseUrl}/channel-info", httpMethod = "GET", headers = SystemCommands.getHeader(), query = [("tag", tag)]) 
            |> ChannelInfoResponse.Parse
            |> fun r -> $"[{r.Iden}]:\nTag: {r.Tag}\nSubscribers: {r.SubscriberCount}\nName: {r.Name}\nDescription: {r.Description}\nRecent push: {r.RecentPushes.[0].Created |> unixTimestampToDateTime}"
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException

    let list () =

        let formatSubscription (s: DataResponse.Subscription) =
            $"(Tag: {s.Channel.Tag}) {s.Channel.Name}: {s.Channel.Description}"

        try
            Http.RequestString($"{BaseUrl}/subscriptions", headers = SystemCommands.getHeader(), query = [("active", "true")]) 
            |> DataResponse.Parse
            |> fun r -> r.Subscriptions
            |> Array.map formatSubscription
            |> String.concat Environment.NewLine
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException

    let delete id =
        try
            Http.RequestString($"{BaseUrl}/subscriptions/{id}", httpMethod = "DELETE", headers = SystemCommands.getHeader()) |> ignore
            "Subscription deleted!"
        with
        | :? WebException as ex -> ex.Response.GetResponseStream() |> formatException