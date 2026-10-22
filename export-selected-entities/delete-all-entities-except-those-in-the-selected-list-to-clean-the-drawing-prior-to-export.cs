using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

namespace CadEntityCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dwg";
            string outputPath = "cleaned_output.pdf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                // Load the CAD drawing
                Image image = Image.Load(inputPath);
                CadImage cadImage = (CadImage)image;

                // Define the list of entity types that should be retained
                List<Type> keepEntityTypes = new List<Type>
                {
                    typeof(CadLine),
                    typeof(CadCircle)
                };

                // Filter entities to keep
                List<CadEntityBase> keptEntities = new List<CadEntityBase>();
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (keepEntityTypes.Contains(entity.GetType()))
                    {
                        keptEntities.Add(entity);
                    }
                }

                // Replace the drawing's objects with the filtered list
                cadImage.Objects = keptEntities.Cast<CadBaseObject>().ToArray();

                // Save the cleaned drawing to PDF
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}