module ToolboxLiberator.ExcelParser

open System
open System.Data
open System.IO
open ExcelDataReader
open System.Text.RegularExpressions
open ToolboxLiberator.SammFormat

let parseExcelQuestionCode (key: string) : QuestionCode =
    match key.Split('-') with
    | [| businessFunction; practice; stream; level; _ |] -> // Last part is always "1"
        { businessFunction = businessFunction
          practice = practice
          stream = stream
          level = Int32.Parse(level) }
    | _ -> failwith "Invalid string format"

let extractScoreData (row: DataRow) : AssessmentAnswer option =
    let key = row.[0].ToString()

    let pattern = @"^[GDIVO]-[A-Z]{2}-[AB]-[1-3]-1$"

    if Regex.IsMatch(key, pattern) then
        let valueString = row.[6].ToString()

        let questionCode = parseExcelQuestionCode key

        match System.Double.TryParse(valueString) with
        | true, parsedValue ->
            Some
                { questionCode = questionCode
                  answerScore = parsedValue }
        | _ -> None
    else
        None

let getDataTable (filePath: string, sheetName: string) : DataTable =
    let encodingProvider = System.Text.CodePagesEncodingProvider.Instance
    System.Text.Encoding.RegisterProvider(encodingProvider)

    use stream = File.Open(filePath, FileMode.Open, FileAccess.Read)
    use reader = ExcelReaderFactory.CreateReader(stream)

    let conf =
        ExcelDataSetConfiguration(ConfigureDataTable = fun _ -> ExcelDataTableConfiguration(UseHeaderRow = true))

    let dataSet = reader.AsDataSet(conf)
    dataSet.Tables.[sheetName]

let extractInterviewAnswers (filePath: string) =
    let table = getDataTable (filePath, "Interview")

    table.Rows |> Seq.cast<DataRow> |> Seq.choose extractScoreData

let extractInterviewMetadata (filePath: string) : InterviewMetadata =
    let table = getDataTable (filePath, "Interview")
    
    let tryParseDate dateStr =
        match DateTime.TryParse(dateStr: string) with
        | true, date -> DateOnly.FromDateTime(date)
        | false, _ -> DateOnly.FromDateTime(DateTime.Today)
        
    // TODO: See if there is a better way to do this than using hardcoded indices
    { organisation = table.Rows.[8].[3].ToString()
      scope = table.Rows.[9].[3].ToString()
      date = table.Rows.[10].[3].ToString() |> tryParseDate }


let extractSammVersion (filePath: string) : string =
    let table = getDataTable (filePath, "Attribution and License")
    // TODO: See if there is a better way to do this than using hardcoded indices
    table.Rows.[1].[1].ToString()