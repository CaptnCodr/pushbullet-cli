namespace Pushbullet

open System
open Utilities

module SubscriptionCommands =

    [<Literal>]
    let private Subscriptions = "subscriptions"

    let channelInfo tag =
        let formatInfo (info: ChannelInfoResponse.Root) =
            if info.RecentPushes.Length > 0 then
                $"[{info.Iden}]:\nTag: {info.Tag}\nSubscribers: {info.SubscriberCount}\nName: {info.Name}\nDescription: {info.Description}\nRecent push: {info.RecentPushes.[0].Created |> unixTimestampToDateTime}"
            else 
                $"[{info.Iden}]:\nTag: {info.Tag}\nSubscribers: {info.SubscriberCount}\nName: {info.Name}\nDescription: {info.Description}"

        HttpService.GetRequest "channel-info" [("tag", tag)]
        |> ChannelInfoResponse.Parse
        |> formatInfo

    let list () =
        HttpService.GetListRequest Subscriptions
        |> DataResponse.Parse
        |> fun r -> r.Subscriptions
        |> Array.map (fun s ->  $"(Tag: {s.Channel.Tag}) {s.Channel.Name}: {s.Channel.Description}")
        |> String.concat Environment.NewLine

    let delete id =
        HttpService.DeleteRequest $"{Subscriptions}/{id}" "Subscription deleted!"