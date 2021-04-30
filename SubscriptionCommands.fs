namespace Pushbullet

open System.Net
open FSharp.Data

module SubscriptionCommands =

    [<Literal>]
    let SubscriptionUrl = "https://api.pushbullet.com/v2/subscriptions"

    let list =
        try
            Http.RequestString(SubscriptionUrl, headers = SystemCommands.header, query = [("active", "true")]) |> CommandHelper.prettifyJson
        with
        | :? WebException as ex -> $"{ex.Message}"

    let delete id =
        try
            Http.RequestString($"{SubscriptionUrl}/{id}", httpMethod = "DELETE", headers = SystemCommands.header) |> ignore
            "Subscription deleted!"
        with
        | :? WebException as ex -> $"{ex.Message}"