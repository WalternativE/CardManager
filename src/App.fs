module App

open Elmish
open Elmish.React
open Fable.Helpers.React
open Fable.Helpers.React.Props

// MODEL

type Draft =
    | NewDraft of string
    | BumpedDraft of string * int
    | RejectedDRaft of string

type Model =
    { DraftForm : string
      Drafts : string list }

type Msg =
| UpdateDraftForm of string
| CreateDraft

let init() : Model =
    { DraftForm = ""
      Drafts = [] }

// UPDATE

let update (msg:Msg) (model:Model) =
    match msg with
    | UpdateDraftForm content ->
        { model with DraftForm = content }
    | CreateDraft ->
        { model with
            DraftForm = ""
            Drafts = model.DraftForm::model.Drafts }

// VIEW (rendered with React)

open Fulma

let toCards (title : string) =
    Tile.tile [ Tile.IsChild; Tile.Size Tile.Is4; Tile.CustomClass "content-card" ]
        [ Card.card [ ]
            [ Card.header []
                [ Card.Header.title [] [ str title ] ]
              Card.content []
                [ Content.content [] [ str "A generic conent" ] ]
              Card.footer []
                [ Card.Footer.a [] [ str "Do It" ] ] ] ]

let toCardRow row =
    Tile.tile [ Tile.IsParent; Tile.Size Tile.Is12 ] row

let rec chunkByThree soFar l =
    match l with
    | x1::x2::[x3] ->
        [x1; x2; x3]::soFar
    | x1::x2::x3::xs ->
        chunkByThree ([x1; x2; x3]::soFar) xs
    | xs ->
        xs::soFar

let toCardRows (titles : string list) =
    titles
    |> chunkByThree []
    |> List.rev
    |> List.map ((List.map toCards) >> toCardRow)

let view (model:Model) dispatch =

    div []
      [ Navbar.navbar [ Navbar.Color IsBlack ]
            [ Navbar.Brand.div []
                [ Navbar.Item.a [ Navbar.Item.Props [ Href "#" ] ]
                    [ str "Card Manager" ] ] ]
        Container.container [ Container.IsFluid ]
          [ h1 [ Class "is-size-1" ] [ str "Manage your Cards" ]
            Tile.tile [ Tile.IsAncestor; Tile.IsVertical ]
                [ Tile.tile [ Tile.IsParent; Tile.Size Tile.Is12 ]
                    [ Tile.tile [ Tile.IsChild ]
                        [ Card.card []
                            [ Card.header []
                                [ Card.Header.title [] [ str "Write a draft!" ] ]
                              Card.content []
                                [ Input.text [ Input.Placeholder "Your draft"
                                               Input.Value model.DraftForm
                                               Input.OnChange (fun ev -> UpdateDraftForm ev.Value |> dispatch) ] ]
                              Card.footer []
                                [ Card.Footer.a [ GenericOption.Props [ OnClick (fun _ -> dispatch CreateDraft) ] ]
                                    [ str "Submit" ] ] ] ] ]
                  model.Drafts |> toCardRows |> ofList ] ] ]

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
