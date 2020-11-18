open System

[<EntryPoint>]
let main (command : string) ([<ParamArray>] argv: string[]): int =
    // match command with
    // | "push" ->


    let message = argv |> String.concat "" // Call the function
    printfn "Hello world %s" message
    0 // return an integer exit code