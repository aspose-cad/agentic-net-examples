using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine("Input file not found.");
                return;
            }

            // Load the CAD file
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Select only line entities
                CadEntityBase[] lineEntities = cadImage.Entities
                    .Where(e => e is CadLine)
                    .ToArray();

                Console.WriteLine($"Total entities in source: {cadImage.Entities.Count()}");
                Console.WriteLine($"Line entities selected: {lineEntities.Length}");

                // Create a new block to hold the filtered entities
                var filteredBlock = new CadBlockEntity();
                foreach (var line in lineEntities)
                {
                    filteredBlock.AddEntity(line);
                }

                // Add the block to the image's block collection
                const string filteredBlockName = "FilteredBlock";
                cadImage.BlockEntities.Add(filteredBlockName, filteredBlock);

                // Configure rasterization options to export only the filtered block
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    UnitType = UnitType.Millimeter,
                    Layouts = new[] { filteredBlockName }
                };

                // Set PDF export options
                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the result
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine("Export completed.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}