using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadMarginExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file
                string inputPath = "input.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Path for the output CAD file
                string outputPath = "output.dwg";

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD‑specific members
                    CadImage cadImage = (CadImage)image;

                    // List all layer names
                    Console.WriteLine("Layers in the drawing:");
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        Console.WriteLine($"- {layer.Name}");
                    }

                    // Define a uniform margin (in drawing units)
                    double margin = 10.0;

                    // Apply margin to top‑level entities
                    ApplyMarginToEntities(cadImage.Entities, margin);

                    // Apply margin to entities inside each block definition
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        ApplyMarginToEntities(block.Entities, margin);
                    }

                    // Save the modified drawing
                    cadImage.Save(outputPath);
                }

                Console.WriteLine("Margins applied and drawing saved.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }

        // Helper method to offset supported entity types
        private static void ApplyMarginToEntities(IEnumerable<CadEntityBase> entities, double margin)
        {
            foreach (CadEntityBase entity in entities)
            {
                switch (entity)
                {
                    case CadLine line:
                        line.FirstPoint = new Cad3DPoint(
                            line.FirstPoint.X + margin,
                            line.FirstPoint.Y + margin,
                            line.FirstPoint.Z);
                        line.SecondPoint = new Cad3DPoint(
                            line.SecondPoint.X + margin,
                            line.SecondPoint.Y + margin,
                            line.SecondPoint.Z);
                        break;

                    case CadLwPolyline polyline:
                        for (int i = 0; i < polyline.Coordinates.Count; i++)
                        {
                            var pt = polyline.Coordinates[i];
                            polyline.Coordinates[i] = new Cad2DPoint(pt.X + margin, pt.Y + margin);
                        }
                        break;

                    case Cad3DFace face:
                        face.FirstCorner = new Cad3DPoint(
                            face.FirstCorner.X + margin,
                            face.FirstCorner.Y + margin,
                            face.FirstCorner.Z);
                        face.SecondCorner = new Cad3DPoint(
                            face.SecondCorner.X + margin,
                            face.SecondCorner.Y + margin,
                            face.SecondCorner.Z);
                        face.ThirdCorner = new Cad3DPoint(
                            face.ThirdCorner.X + margin,
                            face.ThirdCorner.Y + margin,
                            face.ThirdCorner.Z);
                        face.FourthCorner = new Cad3DPoint(
                            face.FourthCorner.X + margin,
                            face.FourthCorner.Y + margin,
                            face.FourthCorner.Z);
                        break;

                    case CadInsertObject insert:
                        insert.InsertionPoint = new Cad3DPoint(
                            insert.InsertionPoint.X + margin,
                            insert.InsertionPoint.Y + margin,
                            insert.InsertionPoint.Z);
                        break;

                    // Add handling for other entity types as needed
                }
            }
        }
    }
}