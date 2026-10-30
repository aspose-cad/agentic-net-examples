using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using Aspose.CAD.ImageOptions;

namespace AddPolylinesExample
{
    class Program
    {
        static void Main()
        {
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Create a new polyline entity
                    CadPolyline polyline = new CadPolyline();

                    // Define points
                    var points = new[]
                    {
                        new Cad3DPoint(0.0, 0.0, 0.0),
                        new Cad3DPoint(100.0, 0.0, 0.0),
                        new Cad3DPoint(100.0, 100.0, 0.0),
                        new Cad3DPoint(0.0, 100.0, 0.0),
                        new Cad3DPoint(0.0, 0.0, 0.0) // close the polyline
                    };

                    // Add each point as a vertex to the polyline
                    foreach (var pt in points)
                    {
                        CadVertexBase vertex = new CadVertexBase();
                        vertex.LocationPoint = pt;
                        polyline.ChildObjects.Add(vertex);
                    }

                    // Optionally add the polyline to the drawing (replace entity collection)
                    cadImage.Entities = cadImage.Entities.Concat(new[] { polyline }).ToArray();

                    // Save the drawing as PDF
                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Successfully saved drawing to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}