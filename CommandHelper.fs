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

//Remove when fixed in dotnet-sdk
module Workaround =
    [<Literal>]
    let refDir = __SOURCE_DIRECTORY__

type DataResponse = JsonProvider<"./Data/DataLists.json", ResolutionFolder=Workaround.refDir>

type ChannelInfoResponse = JsonProvider<"./Data/ChannelInfoData.json", ResolutionFolder=Workaround.refDir>

type ErrorResponse = JsonProvider<"./Data/Error.json", ResolutionFolder=Workaround.refDir>

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