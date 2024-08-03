module ToolboxLiberator.Program

open System.IO
open Newtonsoft.Json
open ToolboxLiberator.JsonOutput
open ToolboxLiberator.ExcelParser

let liberate (inputFile, outputFile) =
    printfn $"Reading data from %s{inputFile}"
    
    inputFile
    |> readExcelFile
    |> printToFile outputFile

    printfn"Liberate complete."

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
