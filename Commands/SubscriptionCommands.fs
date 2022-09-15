namespace Pushbullet

open System
open FSharp.Data
open Resources
open Extensions.DateTimeExtension

module SubscriptionCommands =

    type ChannelInfoResponse = JsonProvider<"./../Data/ChannelInfoData.json", ResolutionFolder=__SOURCE_DIRECTORY__>
    type SubscriptionListResponse = JsonProvider<"./../Data/SubscriptionList.json", ResolutionFolder=__SOURCE_DIRECTORY__>

    type GetChannelInfoCommand = GetChannelInfoCommand of string
    type DeleteSubscriptionCommand = DeleteSubscriptionCommand of string

    [<Literal>]
    let private Subscriptions = "subscriptions"
    
    let list () =
        HttpService.GetListRequest Subscriptions
        |> SubscriptionListResponse.Parse
        |> fun r -> r.Subscriptions
        |> Array.map (fun s -> ListSubscriptionOutput.FormattedString(s.Channel.Tag, s.Channel.Name, s.Channel.Description))
        |> String.concat Environment.NewLine

    let channelInfo (GetChannelInfoCommand tag) =
        HttpService.GetRequest "channel-info" [("tag", tag)]
        |> ChannelInfoResponse.Parse
        |> fun info -> $"""[{info.Iden}]:{"\n"}Tag: {info.Tag}{"\n"}Subscribers: {info.SubscriberCount}{"\n"}Name: {info.Name}{"\n"}Description: {info.Description}{if info.RecentPushes.Length > 0 then $"\nRecent push: {info.RecentPushes.[0].Created.ofUnixTimeToDateTime}" else "" }"""

    let delete (DeleteSubscriptionCommand id) =
        HttpService.DeleteRequest $"{Subscriptions}/{id}" SubscriptionDeleted