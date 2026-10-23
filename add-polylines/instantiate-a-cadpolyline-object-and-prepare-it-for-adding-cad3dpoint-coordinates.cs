using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD;
using System;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;

namespace AddPolylinesExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define 3D points for the polyline
                var points = new Cad3DPoint[]
                {
                    new Cad3DPoint(0.0, 0.0, 0.0),
                    new Cad3DPoint(100.0, 0.0, 0.0),
                    new Cad3DPoint(100.0, 100.0, 0.0),
                    new Cad3DPoint(0.0, 100.0, 0.0)
                };

                // Instantiate a CadPolyline with the 3D points
                var polyline = new CadPolyline(points);

                // Optionally set the polyline as closed
                polyline.Flag = CadPolylineFlag.CLOSED_POLY;

                Console.WriteLine($"CadPolyline created with {points.Length} points. Closed flag set: {polyline.Flag == CadPolylineFlag.CLOSED_POLY}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}