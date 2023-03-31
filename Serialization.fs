namespace Pushbullet

module Serialization =

    open System.Text.Json
    open System.Text.Json.Serialization

    let options =
        JsonSerializerOptions(PropertyNamingPolicy = JsonNamingPolicy.CamelCase)

    options.Converters.Add(JsonFSharpConverter())

    let serialize (obj: 't) : string = JsonSerializer.Serialize(obj, options)
