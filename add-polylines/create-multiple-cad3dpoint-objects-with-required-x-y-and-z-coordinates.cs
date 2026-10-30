using Aspose.CAD;
using System;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Create Cad3DPoint objects with X, Y, Z coordinates
            Cad3DPoint point1 = new Cad3DPoint(10.0, 20.0, 30.0);
            Cad3DPoint point2 = new Cad3DPoint(-5.5, 0.0, 12.3);
            Cad3DPoint point3 = new Cad3DPoint(0.0, 0.0, 0.0);

            // Output the coordinates of each point
            Console.WriteLine($"Point1: X={point1.X}, Y={point1.Y}, Z={point1.Z}");
            Console.WriteLine($"Point2: X={point2.X}, Y={point2.Y}, Z={point2.Z}");
            Console.WriteLine($"Point3: X={point3.X}, Y={point3.Y}, Z={point3.Z}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}