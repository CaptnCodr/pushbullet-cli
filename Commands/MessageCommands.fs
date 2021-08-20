namespace Pushbullet

module MessageCommands =

    let create (device: string) (number: string) (body: string) =
        {| Data = {| Target_Device_Iden = device; Addresses = [number]; Message = body |} |}
        |> fun json -> HttpService.PostRequest "texts" json ""
        |> MessageResponse.Parse
        |> fun r -> $"[{r.Iden}] Message sent!"

    let delete (id: string) = 
        {| Iden = id |}
        |> fun json -> HttpService.PostRequest "texts" json "Message deleted!"
