﻿module SuaveMusicStore.View

open Suave.Html
open System

let divId id = divAttr ["id", id]

let h1 xml = tag "h1" [] xml
let h2 s = tag "h2" [] (text s)

let aHref href = tag "a" ["href", href]
let cssLink href = linkAttr [ "href", href; " rel", "stylesheet"; " type", "text/css" ]

let ul xml = tag "ul" [] (flatten xml)
let li = tag "li" []

let imgSrc src= imgAttr ["src", src]
let em s = tag "em" [] (text s)

let formatDec (d : Decimal) = d.ToString(Globalization.CultureInfo.InvariantCulture)

let homeContainer = [
    h2 "Home"
]

let store genres = [
    h2 "Browse Genres"
    p [
        h2 (sprintf "Select from %d genres:" (List.length genres))
    ]
    ul [
        for g in genres -> 
            li (
                aHref (Path.Store.browse |> Path.withParam (Path.Store.browseKey, g)) (text g)
            )
    ]
]

let browse genre (albums: Db.Album list)= [
    h2 (sprintf "Genre: %s" genre)
    ul [
        for a in albums ->
            li (aHref (sprintf Path.Store.details a.AlbumId) (text a.Title))
    ]
]

let details (album : Db.AlbumDetails) = [
    h2 album.Title
    p [ imgSrc album.AlbumArtUrl ]
    divId "album-details" [
        for (caption,t) in ["Genre:",album.Genre;"Artist:",album.Artist;"Price:",formatDec album.Price] ->
            p [
                em caption
                text t
            ]
    ]
]

let index container = 
    html [
        head [
            title "Suave Music Store"
            cssLink "/Site.css"
        ]

        body [
            divId "header" [
                h1 (aHref Path.home (h2 "F# Suave Music Store"))
            ]

            divId "main" container

            divId "footer" [                       
                    text "built with "
                    aHref "http://fsharp.org" (text "F#")
                    text " and "
                    aHref "http://suave.io" (text "Suave.IO")                
            ]
        ]
    ]
    |> xmlToString  