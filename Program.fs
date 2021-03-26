namespace Pushbullet

open System

module Program =

    type System.String with
        member x.ToOption () =
            if String.IsNullOrWhiteSpace x then None else Some x

    type Errors =
        | NotEnoughArguments

        member this.GetMessage () =
            match this with
            | NotEnoughArguments -> "Not enough arguments!"

    type Command =
        | SetKey of string
        | GetKey
        | PushText of string
        | PushNote of string option * string option
        | PushLink of string * string option * string option
        | GetPushes of int
        | Error of Errors
        | None

        member this.IsSetKeyCommand = match this with | SetKey _ -> true | _ -> false

    let getLinkParams (args: string[]) =
        let url = args |> Array.tryItem 0
        let title = args |> Array.tryItem 1
        let body = args |> Array.tryItem 2
        (url.Value, title, body)

    let findBaseCommand (args: string[]) : Command =
        if args.Length > 0 then
            match args.[0] with
            | "-a" | "--api-key" -> GetKey
            | "-k" | "--set-key" ->
                if args.Length > 1 then
                    SetKey args.[1] else Error NotEnoughArguments
            | "push" | "-t" | "--text" | "-p" | "--push" ->
                if args.Length = 2 then
                    PushText args.[1]
                else if args.Length = 3 then
                    PushNote (args.[1].ToOption(), args.[2].ToOption())
                else Error NotEnoughArguments
            | "link" | "-u" | "--url" | "--link" ->
                if args.Length > 1 then
                    PushLink (args.[1..] |> getLinkParams)
                else Error NotEnoughArguments
            | "list" | "-l" | "--list" ->
                if args.Length > 1
                then GetPushes (args.[1] |> int) else GetPushes 0
            /// Add more commands.
            | _ -> None
        else
            None

    let followCommands command =
        match command with
        | SetKey k -> Commands.setKey k
        | GetKey -> Commands.getKey |> Console.WriteLine
        | PushText t -> Commands.pushText t
        | PushNote (t, b) -> Commands.pushNote t b
        | PushLink (u, t, b) -> Commands.pushLink u t b
        | GetPushes (l) -> Commands.listPushes l |> Console.WriteLine
        | Error e -> e.GetMessage() |> Console.WriteLine
        | _ -> Console.WriteLine("Command not found!")

    [<EntryPoint>]
    let main ([<ParamArray>] argv: string[]): int =

        let command = findBaseCommand argv

        let breakup = Commands.getKey = "" && not(command.IsSetKeyCommand)

        if not breakup then
            followCommands command
        else
            Console.WriteLine("You have to set your API key with: \"--set-key o.Abc12345xyz\" ")

        0