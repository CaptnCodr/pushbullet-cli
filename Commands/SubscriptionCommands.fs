namespace Pushbullet

open System
open Resources
open Utilities

module SubscriptionCommands =

    type GetChannelInfoCommand = GetChannelInfoCommand of string
    type DeleteSubscriptionCommand = DeleteSubscriptionCommand of string

    [<Literal>]
    let private Subscriptions = "subscriptions"
    
    let list () =
        HttpService.GetListRequest Subscriptions
        |> DataResponse.Parse
        |> fun r -> r.Subscriptions
        |> Array.map (fun s ->  $"(Tag: {s.Channel.Tag}) {s.Channel.Name}: {s.Channel.Description}")
        |> String.concat Environment.NewLine

    let channelInfo (GetChannelInfoCommand tag) =
        HttpService.GetRequest "channel-info" [("tag", tag)]
        |> ChannelInfoResponse.Parse
        |> fun info ->  $"""[{info.Iden}]:{"\n"}Tag: {info.Tag}{"\n"}Subscribers: {info.SubscriberCount}{"\n"}Name: {info.Name}{"\n"}Description: {info.Description}{if info.RecentPushes.Length > 0 then $"\nRecent push: {info.RecentPushes.[0].Created |> unixTimestampToDateTime}" else "" }"""

    let delete (DeleteSubscriptionCommand id) =
        HttpService.DeleteRequest $"{Subscriptions}/{id}" SubscriptionDeleted.ResourceString