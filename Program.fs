namespace Pushbullet

open System

module Program =

    [<EntryPoint>]
    let main ([<ParamArray>] argv: string[]): int =
        let command = argv.[0]

        let breakup = Commands.getKey = "" && command <> "--set-key"

        if not breakup then
            match command with
            | "-k" | "--set-key" -> Commands.setKey argv.[1]
            | "-t" | "--text" -> Commands.pushText argv.[1]
            /// Add more commands.
            | _ -> Console.WriteLine("Command not found!")
        else
            Console.WriteLine("You have to set your API key with: \"--set-key o.Abc12345xyz\" ")

        0