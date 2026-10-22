using System;
using System.Collections.Generic;
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
        const string outputPath = "selected_entities.pdf";
        const string selectedLayerName = "SelectedLayer";

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

                // Collect all line entities
                List<CadEntityBase> selectedEntities = new List<CadEntityBase>();
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadLine)
                    {
                        selectedEntities.Add(entity);
                    }
                }

                // Output information about selected lines
                Console.WriteLine("Selected line entities:");
                foreach (CadEntityBase lineEntity in selectedEntities)
                {
                    CadLine line = (CadLine)lineEntity;
                    Console.WriteLine($"Line from ({line.FirstPoint.X}, {line.FirstPoint.Y}) to ({line.SecondPoint.X}, {line.SecondPoint.Y})");
                }

                // Ensure the selected layer exists
                bool layerExists = false;
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    if (string.Equals(layer.Name, selectedLayerName, StringComparison.OrdinalIgnoreCase))
                    {
                        layerExists = true;
                        break;
                    }
                }

                if (!layerExists)
                {
                    CadLayerTable newLayer = new CadLayerTable
                    {
                        Name = selectedLayerName,
                        ColorId = 7, // default color
                        Flags = 0,
                        PlotFlag = true
                    };
                    cadImage.Layers.Add(newLayer);
                }

                // Move selected entities to the new layer
                foreach (CadEntityBase entity in selectedEntities)
                {
                    entity.LayerName = selectedLayerName;
                }

                // Hide all other layers (set ColorId to -1 to turn them off)
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    if (!string.Equals(layer.Name, selectedLayerName, StringComparison.OrdinalIgnoreCase))
                    {
                        layer.ColorId = -1; // turn off layer
                    }
                }

                // Export to PDF
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Export completed: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}