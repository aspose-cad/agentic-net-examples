using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadBackgroundPositioning
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input CAD file path
                string inputCadPath = "input.dwg";
                // Output CAD file path
                string outputCadPath = "output.dwg";

                // Verify input file exists
                if (!File.Exists(inputCadPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputCadPath}");
                    return;
                }

                // Load the CAD drawing
                using (Image cadImageBase = Image.Load(inputCadPath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)cadImageBase;

                    // Define the offset that aligns the drawing with the background image
                    Cad3DPoint offset = new Cad3DPoint(100.0, 50.0, 0.0);

                    // Iterate over top‑level entities and apply the offset
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        switch (entity)
                        {
                            case CadLine line:
                                // Move line start point
                                line.FirstPoint = new Cad3DPoint(
                                    line.FirstPoint.X + offset.X,
                                    line.FirstPoint.Y + offset.Y,
                                    line.FirstPoint.Z + offset.Z);

                                // Move line end point
                                line.SecondPoint = new Cad3DPoint(
                                    line.SecondPoint.X + offset.X,
                                    line.SecondPoint.Y + offset.Y,
                                    line.SecondPoint.Z + offset.Z);
                                break;

                            case CadLwPolyline polyline:
                                // Translate each vertex
                                IList<Cad2DPoint> translated = new List<Cad2DPoint>();
                                foreach (Cad2DPoint pt in polyline.Coordinates)
                                {
                                    translated.Add(new Cad2DPoint(pt.X + offset.X, pt.Y + offset.Y));
                                }
                                polyline.Coordinates.Clear();
                                foreach (Cad2DPoint pt in translated)
                                {
                                    polyline.Coordinates.Add(pt);
                                }
                                break;

                            case CadInsertObject insert:
                                // Adjust the insertion point of the block reference
                                Cad3DPoint ip = insert.InsertionPoint;
                                insert.InsertionPoint = new Cad3DPoint(
                                    ip.X + offset.X,
                                    ip.Y + offset.Y,
                                    ip.Z + offset.Z);
                                break;

                            // Additional entity types can be handled here as needed
                        }
                    }

                    // Save the modified drawing
                    cadImage.Save(outputCadPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}