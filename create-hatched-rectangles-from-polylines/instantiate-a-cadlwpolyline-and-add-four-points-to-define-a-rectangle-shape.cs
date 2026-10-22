using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using Aspose.CAD.ImageOptions;

public class Program
{
    public static void Main()
    {
        // Define placeholder paths. Adjust them to point to real files when testing.
        string inputPath = "input.dxf";
        string outputPath = "output.dxf";

        // Guard file‑system access.
        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Load an existing CAD drawing. The loaded image will be used only to demonstrate
            // the standard load‑save lifecycle; the new polyline is created independently.
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // -----------------------------------------------------------------
                // STEP 1: Create a lightweight polyline (CadLwPolyline) instance.
                // -----------------------------------------------------------------
                CadLwPolyline rectangle = new CadLwPolyline();

                // -----------------------------------------------------------------
                // STEP 2: Define the four corner points of the rectangle.
                // The points are added in order; the polyline will be closed later.
                // -----------------------------------------------------------------
                // Bottom‑left corner (0,0)
                rectangle.Coordinates.Add(new Cad2DPoint(0.0, 0.0));
                // Bottom‑right corner (200,0)
                rectangle.Coordinates.Add(new Cad2DPoint(200.0, 0.0));
                // Top‑right corner (200,100)
                rectangle.Coordinates.Add(new Cad2DPoint(200.0, 100.0));
                // Top‑left corner (0,100)
                rectangle.Coordinates.Add(new Cad2DPoint(0.0, 100.0));

                // -----------------------------------------------------------------
                // STEP 3: Close the polyline to form a proper rectangle.
                // Setting the Flag to Closed tells the renderer to connect the last
                // vertex back to the first one.
                // -----------------------------------------------------------------
                rectangle.Flag = CadLwPolylineFlag.Closed;

                // -----------------------------------------------------------------
                // OPTIONAL: Output diagnostic information to the console.
                // -----------------------------------------------------------------
                Console.WriteLine("Created CadLwPolyline representing a rectangle:");
                Console.WriteLine($"  Number of vertices : {rectangle.PointCount}");
                Console.WriteLine($"  IsClosed flag      : {rectangle.Flag == CadLwPolylineFlag.Closed}");
                Console.WriteLine("  Vertex coordinates:");
                int idx = 0;
                foreach (Cad2DPoint pt in rectangle.Coordinates)
                {
                    Console.WriteLine($"    [{idx}] X={pt.X}, Y={pt.Y}");
                    idx++;
                }

                // -----------------------------------------------------------------
                // STEP 4: (Demonstration only) Save the originally loaded image.
                // The newly created rectangle is not inserted into the drawing
                // because CadImage.Entities is immutable in this version.
                // This step shows the correct usage of save options.
                // -----------------------------------------------------------------
                if (!Directory.Exists(Path.GetDirectoryName(outputPath)))
                {
                    Console.Error.WriteLine($"Output directory does not exist: {Path.GetDirectoryName(outputPath)}");
                    return;
                }

                DxfOptions saveOptions = new DxfOptions();
                cadImage.Save(outputPath, saveOptions);
                Console.WriteLine($"Input drawing saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            // Emit a concise error message and exit gracefully.
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}