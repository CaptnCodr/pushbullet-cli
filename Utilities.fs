namespace Pushbullet

module Utilities =

    let unixTimestampToDateTime (timestamp: decimal) =
        timestamp |> int64 |> System.DateTimeOffset.FromUnixTimeSeconds |> fun d -> d.LocalDateTime