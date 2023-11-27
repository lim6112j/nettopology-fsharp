// For more information see https://aka.ms/fsharp-console-apps
open GeoAPI.Geometries
open GeoAPI.CoordinateSystems
open NetTopologySuite.Geometries
open NetTopologySuite.IO
open NetTopologySuite.Operation.Distance
open ProjNet.CoordinateSystems
open ProjNet.CoordinateSystems.Transformations

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

let projectGeom () =
    let csFact = CoordinateSystemFactory()
    let ctFact = CoordinateTransformationFactory()

    let utm35ETRS =
        csFact.CreateFromWkt(
            "PROJCS[\"ETRS89 / ETRS-TM35\",GEOGCS[\"ETRS89\",DATUM[\"D_ETRS_1989\",SPHEROID[\"GRS_1980\",6378137,298.257222101]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.017453292519943295]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"latitude_of_origin\",0],PARAMETER[\"central_meridian\",27],PARAMETER[\"scale_factor\",0.9996],PARAMETER[\"false_easting\",500000],PARAMETER[\"false_northing\",0],UNIT[\"Meter\",1]]"
        )

    let utm33 = ProjectedCoordinateSystem.WGS84_UTM(33, true)
    let wgs84 = GeographicCoordinateSystem.WGS84
    let wgs84Utm33 = ProjectedCoordinateSystem.WGS84_UTM(33, true)
    let trans = ctFact.CreateFromCoordinateSystems(utm35ETRS, wgs84)

    let ilist (x: _ array) =
        x :> System.Collections.Generic.IList<_>

    let points =
        ilist
            [| [| 290586.087; 6714000.0 |]
               [| 290586.392; 6713996.224 |]
               [| 290590.133; 6713973.772 |]
               [| 290594.111; 6713957.416 |]
               [| 290596.615; 6713943.567 |]
               [| 290596.701; 6713939.485 |] |]

    let spoints = trans.MathTransform.Transform(290586.087, 6714000)
    let tpoints = trans.MathTransform.TransformList(points)
    tpoints

printfn "%A" (projectGeom ())
