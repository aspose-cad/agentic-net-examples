using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadPolylineWidthDemo
{
    class Program
    {
        static void Main()
        {
            string inputPath = "input.dwg";
            string outputFolder = "output";
            string outputPath = Path.Combine(outputFolder, "output.pdf");

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            try
            {
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Process top‑level entities
                    ProcessEntityCollection(cadImage.Entities, cadImage);

                    // Process entities inside each block definition
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        ProcessEntityCollection(block.Entities, cadImage);
                    }

                    // Save the modified drawing as PDF
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = new CadRasterizationOptions
                        {
                            PageWidth = 1600,
                            PageHeight = 1100,
                            DrawType = CadDrawTypeMode.UseObjectColor
                        }
                    };

                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Processing completed. Output saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void ProcessEntityCollection(IEnumerable<CadEntityBase> entities, CadImage cadImage)
        {
            foreach (CadEntityBase entity in entities)
            {
                // Handle lightweight polylines
                if (entity is CadLwPolyline lwPolyline)
                {
                    ApplyWidthCriteria(lwPolyline);
                }
                // Recursively process nested block inserts
                else if (entity is CadInsertObject insert)
                {
                    // Resolve the block definition referenced by the insert
                    if (cadImage.BlockEntities.TryGetValue(insert.Name, out CadBlockEntity referencedBlock))
                    {
                        ProcessEntityCollection(referencedBlock.Entities, cadImage);
                    }
                }
            }
        }

        private static void ApplyWidthCriteria(CadLwPolyline polyline)
        {
            // Example criteria:
            // 1. If the polyline has more than 5 vertices, set width to 1.5
            // 2. Otherwise, set width to 0.5

            double width = 0.5; // default

            if (polyline.Coordinates != null && polyline.Coordinates.Count > 5)
            {
                width = 1.5;
            }

            polyline.ConstantWidth = width;
        }
    }
}