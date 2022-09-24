namespace Pushbullet

open System
open Microsoft.FSharp.Reflection
open Newtonsoft.Json
open Newtonsoft.Json.Serialization

module JsonExtensions =

    type OptionConverter() =
        inherit JsonConverter()

        override _.CanConvert t =
            t.IsGenericType && typedefof<option<_>>.Equals (t.GetGenericTypeDefinition())

        override _.WriteJson(writer, value, serializer) =
            let value =
                if value = null then
                    null
                else
                    let _, fields = FSharpValue.GetUnionFields(value, value.GetType())
                    fields.[0]

            serializer.Serialize(writer, value)

        override _.ReadJson(reader, t, _, serializer) =
            let innerType = t.GetGenericArguments().[0]

            let innerType =
                if innerType.IsValueType then
                    typedefof<Nullable<_>>.MakeGenericType ([| innerType |])
                else
                    innerType

            let value = serializer.Deserialize(reader, innerType)

            let cases = FSharpType.GetUnionCases t

            if value = null then
                FSharpValue.MakeUnion(cases.[0], [||])
            else
                FSharpValue.MakeUnion(cases.[1], [| value |])

    type LowercaseContractResolver() =
        inherit DefaultContractResolver()
        override _.ResolvePropertyName(propertyName: string) = propertyName.ToLower()

    let GetSettings () =
        JsonSerializerSettings(
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = (LowercaseContractResolver() :> IContractResolver),
            Converters = [| OptionConverter() |]
        )
