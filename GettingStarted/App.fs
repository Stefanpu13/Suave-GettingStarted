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
    >=> Writers.setMimeType "text/html; charset=utf-8"

let overview = warbler (fun _ ->
    Db.getContext() 
    |> Db.getGenres 
    |> List.map (fun g -> g.Name) 
    |> View.store 
    |> html)

let browse =
    request (fun r ->
        match r.queryParam "genre" with
        | Choice1Of2 genre -> 
            Db.getContext()
            |> Db.getAlbumsForGenre genre
            |> View.browse genre
            |> html
        | Choice2Of2 msg -> BAD_REQUEST msg)

let details id =
    match Db.getAlbumDetails id (Db.getContext()) with
    | Some album ->
        html (View.details album)
    | None ->
        never

let webPart = 
    choose [
        pathRegex "(.*)\.(css|png)" >=> Files.browseHome

        path home >=> html homeContainer
        path Store.overview >=> overview
        path Store.browse >=> browse
        pathScan Store.details details
    ]

startWebServer defaultConfig webPart