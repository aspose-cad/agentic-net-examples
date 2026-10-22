using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadAttdefLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input and output paths
            string inputPath = "input.dwg";
            string outputPath = "output.dxf";

            // Ensure input file exists (placeholder empty file if missing)
            if (!File.Exists(inputPath))
            {
                File.WriteAllBytes(inputPath, new byte[0]);
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                // Load CAD image
                CadImage cadImage = (CadImage)Image.Load(inputPath);

                int totalAttdefCount = 0;
                int matchedAttdefCount = 0;

                // Examine top‑level entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity.TypeName.ToString() == "ATTDEF")
                    {
                        totalAttdefCount++;

                        // Placeholder match condition – treat all as matched
                        matchedAttdefCount++;
                    }
                }

                // Examine entities inside block definitions
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        if (entity.TypeName.ToString() == "ATTDEF")
                        {
                            totalAttdefCount++;
                            matchedAttdefCount++;
                        }
                    }
                }

                // Log results
                Console.WriteLine($"ATTDEF entities examined: {totalAttdefCount}");
                Console.WriteLine($"ATTDEF entities matched: {matchedAttdefCount}");

                // Save to DXF preserving editable objects
                DxfOptions saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };
                cadImage.Save(outputPath);
                Console.WriteLine($"CAD file saved to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing CAD file: {ex.Message}");
            }
        }
    }
}