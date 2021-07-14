namespace Pushbullet

open System
open System.IO
open Newtonsoft.Json
open Newtonsoft.Json.Serialization

type LowercaseContractResolver () =
    inherit DefaultContractResolver ()
        override _.ResolvePropertyName (propertyName: string) =
            propertyName.ToLower()

module CommandHelper =

    [<Literal>]
    let BaseUrl = "https://api.pushbullet.com/v2"

    let toJson a =
        let settings = JsonSerializerSettings()
        settings.NullValueHandling <- NullValueHandling.Ignore
        settings.ContractResolver <- (LowercaseContractResolver() :> IContractResolver)
        JsonConvert.SerializeObject(a, settings)

    let toValue (value: string option) =
        if value.IsNone then null else value.Value

    let formatException (stream: Stream) =
        new StreamReader(stream) 
        |> fun r -> r.ReadToEnd() 
        |> ErrorResponse.Parse |> fun e -> $"{e.ErrorCode}: {e.Error.Message} {e.Error.Cat}"

    let unixTimestampToDateTime (timestamp: decimal) =
        DateTimeOffset.FromUnixTimeSeconds(timestamp |> int64).LocalDateTime