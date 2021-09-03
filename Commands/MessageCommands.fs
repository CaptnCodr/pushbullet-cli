namespace Pushbullet

module MessageCommands =

    [<Literal>]
    let private Texts = "texts"

    let create (device: string) (number: string) (body: string) =
        {| Data = {| Target_Device_Iden = device; Addresses = [number]; Message = body |} |}
        |> fun json -> HttpService.PostRequest Texts json ""
        |> MessageResponse.Parse
        |> fun r -> $"[{r.Iden}] Message sent!"

    let delete (id: string) = 
        {| Iden = id |}
        |> fun json -> HttpService.PostRequest Texts json "Message deleted!"
