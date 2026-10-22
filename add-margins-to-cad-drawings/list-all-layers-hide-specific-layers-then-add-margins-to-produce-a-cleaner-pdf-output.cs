using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file \"{inputPath}\" not found.");
            return;
        }

        try
        {
            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // List all layers in the drawing
                Console.WriteLine("Layers in the drawing:");
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    Console.WriteLine("- " + layer.Name);
                }

                // Hide specific layers (example: "Layer1" and "Layer2")
                string[] layersToHide = { "Layer1", "Layer2" };
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    if (Array.IndexOf(layersToHide, layer.Name) >= 0)
                    {
                        // Mark layer as off by setting a negative ColorId
                        layer.ColorId = -1;
                    }
                }

                // Verify that at least one layer remains visible
                bool anyVisible = false;
                foreach (CadLayerTable layer in cadImage.Layers)
                {
                    if (layer.ColorId >= 0)
                    {
                        anyVisible = true;
                        break;
                    }
                }

                if (!anyVisible)
                {
                    Console.WriteLine("All layers are hidden; nothing to export.");
                    return;
                }

                // Configure PDF export options
                PdfOptions pdfOptions = new PdfOptions();
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();

                // Note: Aspose.CAD does not expose a direct margin API.
                // Margins can be simulated by increasing the page size if needed.
                // Here we use default page settings.

                pdfOptions.VectorRasterizationOptions = rasterOptions;

                // Save the modified drawing as PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"PDF saved to \"{outputPath}\".");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}