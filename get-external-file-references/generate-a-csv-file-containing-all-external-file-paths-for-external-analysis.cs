using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace GetExternalFileReferences
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Verify input argument
                if (args.Length == 0)
                {
                    Console.WriteLine("Usage: GetExternalFileReferences <input-cad-file>");
                    return;
                }

                string inputPath = args[0];

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Prepare output directory
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
                Directory.CreateDirectory(outputDir);
                string csvPath = Path.Combine(outputDir, "ExternalReferences.csv");

                // List to hold CSV rows
                List<string> csvLines = new List<string>();
                csvLines.Add("EntityType,ExternalPath");

                // Load CAD image
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD‑specific members
                    CadImage cad = cadImage as CadImage;
                    if (cad == null)
                    {
                        Console.WriteLine("Failed to load CAD image.");
                        return;
                    }

                    // Process top‑level entities
                    foreach (CadEntityBase entity in cad.Entities)
                    {
                        string path = GetXrefPath(entity);
                        csvLines.Add($"{entity.TypeName},{path}");
                    }

                    // Process block entities
                    foreach (CadBlockEntity block in cad.BlockEntities.ValuesTyped)
                    {
                        foreach (CadEntityBase innerEntity in block.Entities)
                        {
                            string path = GetXrefPath(innerEntity);
                            csvLines.Add($"{innerEntity.TypeName},{path}");
                        }
                    }
                }

                // Write CSV file
                using (StreamWriter writer = new StreamWriter(csvPath, false))
                {
                    foreach (string line in csvLines)
                    {
                        writer.WriteLine(line);
                    }
                }

                Console.WriteLine($"External references CSV generated at: {csvPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }

        // Helper to retrieve XrefPath via reflection (avoids direct dependency on CadInsert type)
        private static string GetXrefPath(CadEntityBase entity)
        {
            PropertyInfo prop = entity.GetType().GetProperty("XrefPath", BindingFlags.Public | BindingFlags.Instance);
            if (prop != null)
            {
                object value = prop.GetValue(entity);
                return value?.ToString() ?? string.Empty;
            }
            return string.Empty;
        }
    }
}