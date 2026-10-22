using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Paths to the input CAD file and the output PDF file
            string inputPath = "input.dwg";   // TODO: replace with actual path
            string outputPath = "output.pdf"; // TODO: replace with desired output path

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the CAD drawing
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD‑specific members
                    CadImage cad = (CadImage)cadImage;

                    // Inspect top‑level entities for 2D and 3D polylines
                    foreach (CadEntityBase entity in cad.Entities)
                    {
                        // 2D lightweight polyline
                        if (entity is CadLwPolyline lwPolyline)
                        {
                            Console.WriteLine($"Found 2D polyline with {lwPolyline.Coordinates.Count} points.");
                            foreach (Cad2DPoint point in lwPolyline.Coordinates)
                            {
                                Console.WriteLine($"  Point: X={point.X}, Y={point.Y}");
                            }
                        }
                        // 3D polyline (direct type if available)
                        else if (entity.GetType().Name == "Cad3DPolyline")
                        {
                            var coordinatesProp = entity.GetType().GetProperty("Coordinates");
                            if (coordinatesProp != null)
                            {
                                var points = (IList<object>)coordinatesProp.GetValue(entity);
                                Console.WriteLine($"Found 3D polyline with {points.Count} points.");
                                foreach (object pt in points)
                                {
                                    var xProp = pt.GetType().GetProperty("X");
                                    var yProp = pt.GetType().GetProperty("Y");
                                    var zProp = pt.GetType().GetProperty("Z");
                                    double x = (double)xProp.GetValue(pt);
                                    double y = (double)yProp.GetValue(pt);
                                    double z = (double)zProp.GetValue(pt);
                                    Console.WriteLine($"  Point: X={x}, Y={y}, Z={z}");
                                }
                            }
                        }
                        else
                        {
                            // Log other entity types if needed
                            Console.WriteLine($"Entity type: {entity.TypeName}");
                        }
                    }

                    // Export the drawing to PDF to verify representation of polylines
                    try
                    {
                        PdfOptions pdfOptions = new PdfOptions();
                        cadImage.Save(outputPath, pdfOptions);
                        Console.WriteLine($"Drawing exported to PDF: {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error saving PDF: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing CAD file: {ex.Message}");
            }
        }
    }
}
