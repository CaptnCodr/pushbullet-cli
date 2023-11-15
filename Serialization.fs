namespace Pushbullet

module Serialization =

    open System.Text.Json
    open System.Text.Json.Serialization
    open System.Text.Json.Serialization.Metadata

    let options =
        JsonSerializerOptions(PropertyNamingPolicy = JsonNamingPolicy.CamelCase, TypeInfoResolver = DefaultJsonTypeInfoResolver())

    options.Converters.Add(JsonFSharpConverter())

    let serialize (obj: 't) : string = JsonSerializer.Serialize(obj, options)
