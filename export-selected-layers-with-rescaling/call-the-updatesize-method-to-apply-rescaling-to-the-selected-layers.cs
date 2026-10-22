using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main(string[] args)
    {
        // Input / output handling
        string sourcePath = args.Length > 0 ? args[0] : "sample.dwg";

        if (!File.Exists(sourcePath))
        {
            Console.Error.WriteLine($"Source CAD file not found: {sourcePath}");
            return;
        }

        string outputFolder = "Output";
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        string resultPath = Path.Combine(outputFolder, "RescaledLayers.pdf");

        // Define which layers should be considered for export
        string[] layersToExport = new string[] { "Layer1", "Layer2" };
        // Desired dimensions for the exported page (used for scaling)
        float targetWidth = 250.0f;
        float targetHeight = 180.0f;

        try
        {
            // Load the CAD file
            using (Image genericImage = Image.Load(sourcePath))
            {
                CadImage cadImage = genericImage as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // The Aspose.CAD API does not provide a direct UpdateSize method for layers.
                // Rescaling can be achieved by adjusting the rasterization options (page size, line scale, etc.).
                // Here we simply log the layers that would be processed.
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    foreach (string targetName in layersToExport)
                    {
                        if (string.Equals(layer.Name, targetName, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"Layer '{layer.Name}' will be included in the export.");
                            break;
                        }
                    }
                }

                // Prepare PDF export options
                PdfOptions pdfOptions = new PdfOptions();

                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Set page size to match the desired dimensions (in the chosen unit)
                    PageWidth = targetWidth,
                    PageHeight = targetHeight,
                    // Use the SDK's unit spelling
                    UnitType = UnitType.Centimenter,
                    // Optional: adjust line scale if needed
                    LineScale = 1.0f,
                    // Define margins (all zero by default)
                    Margins = new Margins()
                };

                // Export only the selected layers
                rasterOptions.Layers = layersToExport;

                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save the result
                cadImage.Save(resultPath, pdfOptions);
                Console.WriteLine($"File saved successfully to: {resultPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}