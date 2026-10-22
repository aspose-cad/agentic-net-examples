using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadEntityDemo
{
    class Program
    {
        static void Main()
        {
            string inputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            string outputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output");
            string inputFile = Path.Combine(inputDirectory, "input.dxf");
            string outputFile = Path.Combine(outputDirectory, "output.pdf");

            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                return;
            }

            if (!File.Exists(inputFile))
            {
                Console.Error.WriteLine($"Input CAD file not found: {inputFile}");
                return;
            }

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            try
            {
                // Load the CAD file
                using (Image image = Image.Load(inputFile))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // List all layers
                    Console.WriteLine("Layers in the drawing:");
                    foreach (CadLayerTable layer in cadImage.Layers)
                    {
                        string visibility = (layer.ColorId < 0) ? "Off" :
                                            ((layer.Flags & 1) != 0) ? "Frozen" :
                                            ((layer.Flags & 4) != 0) ? "Locked" : "On";
                        Console.WriteLine($"  Name: {layer.Name}, ColorId: {layer.ColorId}, Visibility: {visibility}");
                    }

                    // List all block definitions
                    Console.WriteLine("\nBlock definitions:");
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        Console.WriteLine($"  Block Name: {block.Name}, Entity Count: {block.Entities.Count}");
                    }

                    // Enumerate top‑level entities and modify color of entities on layer "0"
                    Console.WriteLine("\nTop‑level entities:");
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        string typeName = entity.GetType().Name;
                        Console.WriteLine($"  Type: {typeName}, Layer: {entity.LayerName}");

                        // Example modification: set color of entities on layer "0" to red (ColorId = 1)
                        if (string.Equals(entity.LayerName, "0", StringComparison.OrdinalIgnoreCase))
                        {
                            entity.ColorId = 1; // Red
                        }

                        // Detailed handling for specific entity types (optional)
                        if (entity is CadLine line)
                        {
                            // Example: output line endpoints
                            Console.WriteLine($"    Line from ({line.FirstPoint.X}, {line.FirstPoint.Y}, {line.FirstPoint.Z}) " +
                                              $"to ({line.SecondPoint.X}, {line.SecondPoint.Y}, {line.SecondPoint.Z})");
                        }
                        else if (entity is CadCircle circle)
                        {
                            Console.WriteLine($"    Circle center ({circle.CenterPoint.X}, {circle.CenterPoint.Y}, {circle.CenterPoint.Z}), Radius: {circle.Radius}");
                        }
                        else if (entity is CadArc arc)
                        {
                            Console.WriteLine($"    Arc center ({arc.CenterPoint.X}, {arc.CenterPoint.Y}, {arc.CenterPoint.Z}), " +
                                              $"Radius: {arc.Radius}, StartAngle: {arc.StartAngle}, EndAngle: {arc.EndAngle}");
                        }
                        else if (entity is CadInsertObject insert)
                        {
                            Console.WriteLine($"    Insert of block \"{insert.Name}\" at ({insert.InsertionPoint.X}, {insert.InsertionPoint.Y}, {insert.InsertionPoint.Z})");
                        }
                    }

                    // Save the modified drawing to PDF
                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(outputFile, pdfOptions);
                    Console.WriteLine($"\nModified drawing saved to: {outputFile}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
            }
        }
    }
}