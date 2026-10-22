using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image loadedImage = Image.Load(inputPath))
            {
                if (loadedImage is not CadImage cadImage)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                var externalFilePaths = new List<string>();

                foreach (var entity in cadImage.Entities)
                {
                    if (entity == null) continue;

                    Type entityType = entity.GetType();

                    // Look for types that likely represent external references
                    if (entityType.Name.Contains("ExternalReference", StringComparison.OrdinalIgnoreCase) ||
                        entityType.Name.Contains("Xref", StringComparison.OrdinalIgnoreCase))
                    {
                        // Try common property names that may hold the file path
                        PropertyInfo pathProp = entityType.GetProperty("FilePath") ??
                                                entityType.GetProperty("Path") ??
                                                entityType.GetProperty("ReferencePath");

                        if (pathProp != null)
                        {
                            string pathValue = pathProp.GetValue(entity) as string;
                            if (!string.IsNullOrWhiteSpace(pathValue))
                            {
                                externalFilePaths.Add(pathValue);
                            }
                        }
                    }
                }

                if (externalFilePaths.Count == 0)
                {
                    Console.WriteLine("No external file references found in the drawing.");
                }
                else
                {
                    Console.WriteLine($"Found {externalFilePaths.Count} external file reference(s):");
                    foreach (string refPath in externalFilePaths)
                    {
                        bool exists = File.Exists(refPath);
                        Console.WriteLine($"Reference: {refPath} - Exists: {exists}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}