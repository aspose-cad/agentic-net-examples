using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadHatchOriginAdjuster
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Specify the directory containing CAD drawings
                string inputDirectory = @"C:\InputDrawings";
                // Specify the directory where adjusted drawings will be saved
                string outputDirectory = @"C:\OutputDrawings";

                // Ensure the output directory exists
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Process each CAD file in the input directory
                string[] cadFiles = Directory.GetFiles(inputDirectory, "*.*", SearchOption.TopDirectoryOnly);
                foreach (string inputFilePath in cadFiles)
                {
                    string extension = Path.GetExtension(inputFilePath).ToLowerInvariant();
                    // Consider only supported CAD formats (DWG, DXF)
                    if (extension != ".dwg" && extension != ".dxf")
                    {
                        continue;
                    }

                    // Load the CAD drawing
                    using (Aspose.CAD.Image image = Aspose.CAD.Image.Load(inputFilePath))
                    {
                        CadImage cadImage = image as CadImage;
                        if (cadImage == null)
                        {
                            continue;
                        }

                        // Adjust hatch origins in the main drawing entities
                        AdjustHatchOriginsInEntities(cadImage.Entities);

                        // Adjust hatch origins inside block definitions
                        foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                        {
                            AdjustHatchOriginsInEntities(block.Entities);
                        }

                        // Prepare the output file path
                        string outputFilePath = Path.Combine(outputDirectory, Path.GetFileName(inputFilePath));

                        // Save the modified drawing (format inferred from extension)
                        cadImage.Save(outputFilePath);
                    }
                }

                Console.WriteLine("Hatch origin adjustment completed.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }

        // Adjusts the PatternOrigin of all hatch-like entities within a collection using reflection
        private static void AdjustHatchOriginsInEntities(IEnumerable<CadEntityBase> entities)
        {
            foreach (CadEntityBase entity in entities)
            {
                if (entity == null) continue;

                Type entityType = entity.GetType();
                PropertyInfo patternOriginProp = entityType.GetProperty("PatternOrigin", BindingFlags.Public | BindingFlags.Instance);
                if (patternOriginProp != null && patternOriginProp.CanWrite)
                {
                    // Set the hatch origin to (0,0,0)
                    patternOriginProp.SetValue(entity, new Cad3DPoint(0, 0, 0));
                }
            }
        }
    }
}