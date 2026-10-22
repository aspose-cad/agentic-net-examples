using System;
using System.IO;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input CAD file path (DRC)
                string inputPath = "input.drc";
                // Output PNG file path
                string outputPath = "output.png";

                // Validate input file
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file does not exist: {inputPath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load CAD image
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage for block/entity inspection
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // Retrieve and display metadata (author, creation date) via reflection
                    Console.WriteLine("Metadata:");
                    var header = cadImage.Header;
                    if (header != null)
                    {
                        string author = GetHeaderProperty(header, "Author") ?? "Unknown";
                        string creationDate = GetHeaderProperty(header, "CreationDate") ?? "Unknown";
                        Console.WriteLine($"  Author: {author}");
                        Console.WriteLine($"  Creation Date: {creationDate}");
                    }
                    else
                    {
                        Console.WriteLine("  No header information available.");
                    }

                    // Iterate top‑level entities
                    Console.WriteLine("\nTop‑level entities:");
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        PrintEntityInfo(entity);
                    }

                    // Iterate block definitions
                    Console.WriteLine("\nBlock definitions:");
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        Console.WriteLine($"Block: {block.Name}");
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            PrintEntityInfo(entity, "  ");
                        }
                    }

                    // Prepare rasterization options for PNG output
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200
                    };

                    // Set PNG options
                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save rendered image
                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"\nRendered image saved to: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        // Helper to get header property value via reflection
        private static string GetHeaderProperty(CadHeader header, string propertyName)
        {
            PropertyInfo prop = header.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null)
            {
                object value = prop.GetValue(header);
                return value?.ToString();
            }
            return null;
        }

        // Helper method to print basic entity information
        private static void PrintEntityInfo(CadEntityBase entity, string indent = "")
        {
            string entityType = entity.GetType().Name;
            string layerName = entity.LayerName ?? "None";

            if (entity is CadLine line)
            {
                Console.WriteLine($"{indent}Line - Layer: {layerName}, Start: ({line.FirstPoint.X}, {line.FirstPoint.Y}), End: ({line.SecondPoint.X}, {line.SecondPoint.Y})");
            }
            else if (entity is CadArc arc)
            {
                Console.WriteLine($"{indent}Arc - Layer: {layerName}, Center: ({arc.CenterPoint.X}, {arc.CenterPoint.Y}), StartAngle: {arc.StartAngle}, EndAngle: {arc.EndAngle}");
            }
            else if (entity is CadCircle circle)
            {
                Console.WriteLine($"{indent}Circle - Layer: {layerName}, Center: ({circle.CenterPoint.X}, {circle.CenterPoint.Y}), Radius: {circle.Radius}");
            }
            else if (entity is CadLwPolyline polyline)
            {
                Console.WriteLine($"{indent}LwPolyline - Layer: {layerName}, Vertex count: {polyline.Coordinates.Count}");
            }
            else if (entity is CadInsertObject insert)
            {
                Console.WriteLine($"{indent}Insert - Layer: {layerName}, Block Name: {insert.Name}, Insertion Point: ({insert.InsertionPoint.X}, {insert.InsertionPoint.Y})");
            }
            else
            {
                Console.WriteLine($"{indent}{entityType} - Layer: {layerName}");
            }
        }
    }
}