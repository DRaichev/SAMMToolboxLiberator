module ToolboxLiberator.Program

open System.IO
open Newtonsoft.Json
open ToolboxLiberator.JsonOutput
open ToolboxLiberator.ExcelParser
open ToolboxLiberator.SammFormat

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
    | _ ->
        printfn "Usage:"
        printfn "liberate <inputFile> <outputFile> - Liberate data from Excel and write to JSON"
        1
