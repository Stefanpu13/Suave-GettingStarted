module SuaveMusicStore.App
open Suave                 // always open suave
open Suave.Successful      // for OK-result
open Suave.RequestErrors
open Suave.Filters
open Suave.Operators

open Path
open View
open Db

let html container =
    OK (View.index container)


let overview = warbler (fun _ ->
    Db.getContext() 
    |> Db.getGenres 
    |> List.map (fun g -> g.Name) 
    |> View.store 
    |> html)

let browse =
    request (fun r ->
        match r.queryParam "genre" with
        | Choice1Of2 genre -> html (View.browse genre)
        | Choice2Of2 msg -> BAD_REQUEST msg)

let webPart = 
    choose [
        pathRegex "(.*)\.(css|png)" >=> Files.browseHome

        path home >=> html homeContainer
        path Store.overview >=> overview
        path Store.browse >=> browse
        pathScan Store.details (fun id -> html (details id))
    ]


startWebServer defaultConfig webPart