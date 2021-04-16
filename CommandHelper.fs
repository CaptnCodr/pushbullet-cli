namespace Pushbullet

open Newtonsoft.Json
open Newtonsoft.Json.Linq
open Newtonsoft.Json.Serialization

type LowercaseContractResolver () =
    inherit DefaultContractResolver ()
        override _.ResolvePropertyName (propertyName: string) =
            propertyName.ToLower()

module CommandHelper =

    let toJson a =
        let settings = JsonSerializerSettings()
        settings.NullValueHandling <- NullValueHandling.Ignore
        settings.ContractResolver <- (LowercaseContractResolver() :> IContractResolver)
        JsonConvert.SerializeObject(a, settings)

    let prettifyJson json =
        json |> JToken.Parse |> string