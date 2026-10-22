using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Define the list of entity types to keep
                var allowedTypes = new HashSet<Type>
                {
                    typeof(CadLine),
                    typeof(CadCircle)
                };

                // Filter entities inside each block
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    if (block.Entities is IList<CadEntityBase> entityList)
                    {
                        for (int i = entityList.Count - 1; i >= 0; i--)
                        {
                            CadEntityBase entity = entityList[i];
                            if (!allowedTypes.Contains(entity.GetType()))
                            {
                                entityList.RemoveAt(i);
                            }
                        }
                    }
                }

                // Filter top‑level entities (if any)
                if (cadImage.Entities is IList<CadEntityBase> topEntityList)
                {
                    for (int i = topEntityList.Count - 1; i >= 0; i--)
                    {
                        CadEntityBase entity = topEntityList[i];
                        if (!allowedTypes.Contains(entity.GetType()))
                        {
                            topEntityList.RemoveAt(i);
                        }
                    }
                }

                // Save the filtered drawing to PDF
                var pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"Filtered CAD saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}