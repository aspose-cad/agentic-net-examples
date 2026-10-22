using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;

class Program
{
    static void Main()
    {
        try
        {
                // Path to the input CAD file
                string inputPath = "input.dwg";

        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Identify all LWPOLYLINE entities
                    var lwPolylines = cadImage.Entities
                        .Where(e => e.TypeName == CadEntityTypeName.LWPOLYLINE)
                        .Cast<CadLwPolyline>();

                    foreach (CadLwPolyline polyline in lwPolylines)
                    {
                        Console.WriteLine("LWPOLYLINE - Points: {0}, Closed: {1}",
                            polyline.PointCount,
                            polyline.Flag.HasFlag(CadLwPolylineFlag.Closed));

                        int pointIndex = 0;
                        foreach (Cad2DPoint point in polyline.Coordinates)
                        {
                            Console.WriteLine($"  Point {pointIndex++}: X={point.X}, Y={point.Y}");
                        }
                    }

                    // Export the CAD drawing to PDF
                    string outputPath = "output.pdf";
                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(outputPath, pdfOptions);
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}