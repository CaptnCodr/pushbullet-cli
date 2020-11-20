namespace Pushbullet

open System

module Program =

    let findCommands (args: string[]) =
        match args.[0] with
        | "-k" | "--set-key" -> Commands.setKey args.[1]
        | "-a" | "--api-key" -> Commands.getKey |> Console.WriteLine
        | "-t" | "--text" -> Commands.pushText args.[1]
        /// Add more commands.
        | _ -> Console.WriteLine("Command not found!")

    [<EntryPoint>]
    let main ([<ParamArray>] argv: string[]): int =
        let command = argv.[0]

        let breakup = Commands.getKey = "" && command <> "--set-key"

        if not breakup then
            findCommands argv
        else
            Console.WriteLine("You have to set your API key with: \"--set-key o.Abc12345xyz\" ")

        0