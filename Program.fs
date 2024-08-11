module ToolboxLiberator.Program

open ToolboxLiberator.JsonOutput
open ToolboxLiberator.ExcelParser
open ToolboxLiberator.SammFormat
open ToolboxLiberator.SchemaValidator

let liberate (inputFile, outputFile) =
    printfn $"Reading data from %s{inputFile}"

    let version = inputFile |> extractSammVersion
    let metadata = inputFile |> extractInterviewMetadata
    let answers = inputFile |> extractInterviewAnswers
    
    let assessment =
        { version = version
          date = metadata.date
          organisation = metadata.organisation
          scope = metadata.scope
          answers = answers }

    let sammFormat =
        { formatVersion = "0.1.0"
          assessment = assessment }

    sammFormat |> printToFile outputFile

    printfn "Liberate complete."

[<EntryPoint>]
let main argv =
    match argv with
    | [| "liberate"; inputFile; outputFile |] ->
        liberate (inputFile, outputFile)
        0
    | [| "validate"; jsonFile |] ->
        validateJsonFromFile jsonFile None
        0
    | [| "validate"; jsonFile; schemaFile |] ->
        validateJsonFromFile jsonFile (Some(schemaFile))
        0
    | _ ->
        printfn "Usage:"
        printfn "liberate <input.xlsx> <output> - Read data from Excel and write to .samm format"
        printfn "validate <input.samm> (<schemaFile>) - Validate file against samm schema (optional custom schema)"
        1
