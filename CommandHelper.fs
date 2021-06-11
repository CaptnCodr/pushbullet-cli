namespace Pushbullet

open System.IO
open FSharp.Data
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open Newtonsoft.Json.Serialization

type LowercaseContractResolver () =
    inherit DefaultContractResolver ()
        override _.ResolvePropertyName (propertyName: string) =
            propertyName.ToLower()

type DataResponse = JsonProvider<"./Data/ListData.json", ResolutionFolder=__SOURCE_DIRECTORY__>

type ErrorResponse = JsonProvider<"./Data/Error.json", ResolutionFolder=__SOURCE_DIRECTORY__>

module CommandHelper =

    [<Literal>]
    let BaseUrl = "https://api.pushbullet.com/v2"

    let toJson a =
        let settings = JsonSerializerSettings()
        settings.NullValueHandling <- NullValueHandling.Ignore
        settings.ContractResolver <- (LowercaseContractResolver() :> IContractResolver)
        JsonConvert.SerializeObject(a, settings)

    let prettifyJson json =
        json |> JToken.Parse |> string

    let formatException (stream: Stream) =
        new StreamReader(stream) 
        |> fun r -> r.ReadToEnd() 
        |> ErrorResponse.Parse |> fun e -> $"{e.ErrorCode}: {e.Error.Message} {e.Error.Cat}"