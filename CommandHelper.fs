namespace Pushbullet

open System

module CommandHelper =

    let Actives = ("active", "true")
        
    let unixTimestampToDateTime (timestamp: decimal) =
        DateTimeOffset.FromUnixTimeSeconds(timestamp |> int64).LocalDateTime