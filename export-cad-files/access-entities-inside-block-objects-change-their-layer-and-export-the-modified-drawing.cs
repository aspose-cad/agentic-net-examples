using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.pdf";
        const string targetLayerName = "ModifiedLayer";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Ensure the target layer exists
                bool layerExists = false;
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    if (string.Equals(layer.Name, targetLayerName, StringComparison.OrdinalIgnoreCase))
                    {
                        layerExists = true;
                        break;
                    }
                }

                if (!layerExists)
                {
                    CadLayerTable newLayer = new CadLayerTable
                    {
                        Name = targetLayerName,
                        ColorId = 7, // default color
                        Flags = 0,
                        PlotFlag = true
                    };
                    cadImage.Layers.Add(newLayer);
                }

                // Change layer of entities inside each block
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        entity.LayerName = targetLayerName;
                    }
                }

                // Optionally change top‑level entities as well
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    entity.LayerName = targetLayerName;
                }

                // Save the modified drawing as PDF
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}