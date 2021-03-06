﻿module SuaveMusicStore.Db

#if INTERACTIVE
#r "System.Data.dll"
#r "../packages/SQLProvider.1.1.7/lib/FSharp.Data.SqlProvider.dll"
#endif


open FSharp.Data.Sql
[<Literal>]
let connectionString = "Server=.;Database=SuaveMusicStore;Trusted_Connection=True;MultipleActiveResultSets=true"

type Sql = 
    SqlDataProvider<
        ConnectionString=connectionString, 
        DatabaseVendor=Common.DatabaseProviderTypes.MSSQLSERVER
        >

type DbContext = Sql.dataContext
type Album = DbContext.``dbo.AlbumsEntity``
type Genre = DbContext.``dbo.GenresEntity``
type AlbumDetails = DbContext.``dbo.AlbumDetailsEntity``

let firstOrNone s = s |> Seq.tryFind (fun _ -> true)

let getGenres (ctx : DbContext) : Genre list = 
    ctx.Dbo.Genres |> Seq.toList

let getAlbumsForGenre genreName (ctx : DbContext) : Album list = 
    query { 
        for album in ctx.Dbo.Albums do
            join genre in ctx.Dbo.Genres on (album.GenreId = genre.GenreId)
            where (genre.Name = genreName)
            select album
    }
    |> Seq.toList

let getAlbumDetails id (ctx : DbContext) : AlbumDetails option = 
    query { 
        for album in ctx.Dbo.AlbumDetails do
            where (album.AlbumId = id)
            select album
    } |> firstOrNone

let getContext() = Sql.GetDataContext()
