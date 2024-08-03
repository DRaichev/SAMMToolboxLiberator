module ToolboxLiberator.JsonOutput

open System
open System.IO
open Newtonsoft.Json
open ToolboxLiberator.SammFormat

type QuestionCodeConverter() =
    inherit JsonConverter()

    override _.CanConvert(objectType: Type) = objectType = typeof<QuestionCode>

    override _.WriteJson(writer: JsonWriter, value: obj, _) =
        let code = value :?> QuestionCode
        writer.WriteValue(serializeQuestionCode code)

    override _.ReadJson(reader: JsonReader, _, _, _) =
        let s = reader.Value :?> string
        parseQuestionCode s

let saveDataToJson (data: seq<AssessmentAnswer>) (outputPath: string) =
    let json = JsonConvert.SerializeObject(data, Formatting.Indented)
    File.WriteAllText(outputPath, json)

let printToFile (outputPath: string) (data: seq<'T>) =
    printfn $"Writing data to %s{outputPath}"
    use writer = new StreamWriter(outputPath)
    let settings = JsonSerializerSettings()
    settings.Converters.Add(QuestionCodeConverter())
    let jsonData = JsonConvert.SerializeObject(data, Formatting.Indented, settings)
    writer.Write(jsonData)
