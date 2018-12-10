module App

open Elmish
open Elmish.React
open Fable.Helpers.React
open Fable.Helpers.React.Props

// MODEL

type Model = int

type Msg =
| Increment
| Decrement

let init() : Model = 0

// UPDATE

let update (msg:Msg) (model:Model) =
    match msg with
    | Increment -> model + 1
    | Decrement -> model - 1

// VIEW (rendered with React)

open Fulma

let view (model:Model) dispatch =

    div []
      [ Hero.hero [ Hero.Color IsSuccess; Hero.IsHalfHeight ]
          [ Hero.body []
              [ Container.container [ Container.IsFullHD
                                      Container.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                  [ Heading.h1 []
                      [ str "Glory to the Counter!" ] ] ] ]
        Container.container [ Container.IsFluid ]
          [ Content.content []
              [ button [ OnClick (fun _ -> dispatch Increment) ] [ str "PLUS" ]
                div [] [ str (string model) ]
                button [ OnClick (fun _ -> dispatch Decrement) ] [ str "MINUS" ] ] ] ]

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

// App
Program.mkSimple init update view
|> Program.withReactUnoptimized "elmish-app"
#if DEBUG
|> Program.withConsoleTrace
|> Program.withDebugger
#endif
|> Program.run
