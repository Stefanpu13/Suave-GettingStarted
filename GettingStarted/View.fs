﻿module SuaveMusicStore.View

open Suave.Html

open Path

let divId id = divAttr ["id", id]

let h1 xml = tag "h1" [] xml
let h2 s = tag "h2" [] (text s)

let aHref href = tag "a" ["href", href]
let cssLink href = linkAttr [ "href", href; " rel", "stylesheet"; " type", "text/css" ]

let ul xml = tag "ul" [] (flatten xml)
let li = tag "li" []

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

let browse genre = [
    h2 (sprintf "Genre: %s" genre)
]

let details id = [
    h2 (sprintf "Details %d" id)
]


let index container = 
    html [
        head [
            title "Suave Music Store"
            cssLink "/Site.css"
        ]

        body [
            divId "header" [
                h1 (aHref home (h2 "F# Suave Music Store"))
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