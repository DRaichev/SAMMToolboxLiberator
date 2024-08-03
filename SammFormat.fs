module ToolboxLiberator.SammFormat

open System

type QuestionCode =
    { businessFunction: string
      practice: string
      stream: string
      level: int }

type AssessmentAnswer =
    { questionCode: QuestionCode
      answerScore: float }

let serializeQuestionCode (code: QuestionCode) =
    $"%s{code.businessFunction}-%s{code.practice}-%s{code.stream}-%s{code.level |> string}"

let parseQuestionCode (key: string) : QuestionCode =
    match key.Split('-') with
    | [| businessFunction; practice; stream; level |] ->
        { businessFunction = businessFunction
          practice = practice
          stream = stream
          level = Int32.Parse(level) }
    | _ -> failwith "Invalid string format"
