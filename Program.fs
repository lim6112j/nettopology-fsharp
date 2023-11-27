// For more information see https://aka.ms/fsharp-console-apps
open GeoAPI.Geometries
open NetTopologySuite.Geometries
open NetTopologySuite.IO
open NetTopologySuite.Operation.Distance

printfn "Hello from F#"

let closestPoint () =
    let g1 = WKTReader().Read("LINESTRING (0 0, 10 10, 20 20)")
    let g2 = WKTReader().Read("LINESTRING (0 0, 10 10, 20 20)")
    let g3 = g1.Intersection g2
    g3.ToString()

printfn "G1 intersection G2 : %s" (closestPoint ())
