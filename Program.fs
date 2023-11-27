// For more information see https://aka.ms/fsharp-console-apps
open GeoAPI.Geometries
open NetTopologySuite.Geometries
open NetTopologySuite.IO
open NetTopologySuite.Operation.Distance

printfn "Hello from F#"

let intersection () =
    let g1 = WKTReader().Read("LINESTRING (0 0, 10 10, 20 20)")
    let g2 = WKTReader().Read("LINESTRING (-10 0, 10 10, 20 20)")
    let g3 = g1.Intersection g2
    g3.ToString()

let findClosestPoint (wktA: string, wktB: string) =
    let a = WKTReader().Read(wktA)
    let b = WKTReader().Read(wktB)
    let distanceOP = DistanceOp(a, b)
    let distance = distanceOP.Distance()
    printfn "distance from a to b : %f" distance
    let closestPt = distanceOP.NearestPoints()
    let closestPtLine = GeometryFactory().CreateLineString(closestPt)
    printfn "a ,b line closest point from pt : %A" closestPt
    printfn "a ,b line closest point from pt line : %A" closestPtLine
    closestPt

printfn "G1 intersection G2 : %s" (intersection ())

let distance =
    findClosestPoint ("LINESTRING (100 100, 200 200)", "LINESTRING (100 200, 200 100)")

let distance2 =
    findClosestPoint ("LINESTRING (100 100, 200 200)", "POINT (140 280)")

let distance3 =
    findClosestPoint ("LINESTRING (100 100, 300 300)", "POINT (140 280)")
