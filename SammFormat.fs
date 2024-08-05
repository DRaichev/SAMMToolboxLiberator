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

type InterviewMetadata =
    { organisation: string
      scope: string
      date: DateOnly }

type Assessment =
    { version: string
      organisation: string
      scope: string
      date: DateOnly
      answers: seq<AssessmentAnswer> }

type SammFormat =
    { formatVersion: string
      assessment: Assessment }

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
