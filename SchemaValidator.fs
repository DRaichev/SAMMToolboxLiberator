module ToolboxLiberator.SchemaValidator

open System
open System.IO
open System.Reflection
open NJsonSchema

// Function to read the embedded resource
let readEmbeddedResource resourceName =
    let assembly = Assembly.GetExecutingAssembly()
    use stream = assembly.GetManifestResourceStream(resourceName)
    if stream = null then
        invalidOp $"Resource '{resourceName}' not found."
    use reader = new StreamReader(stream)
    reader.ReadToEnd()

let defaultSchema = readEmbeddedResource("ToolboxLiberator.samm.schema.json")

let validateJsonFromFile jsonFilePath (schemaFilePath: string option) =
    let jsonContent = File.ReadAllText(jsonFilePath)
    
    let schemaContent = 
        match schemaFilePath with
        | Some(path) -> File.ReadAllText(path)
        | None -> defaultSchema

    let schema = JsonSchema.FromJsonAsync(schemaContent).Result
    let errors = schema.Validate(jsonContent)
    if errors.Count = 0 then
        printfn "JSON is valid."
    else
        printfn "JSON is invalid."
        errors |> Seq.iter (fun error -> printfn $"%s{error.ToString()}")