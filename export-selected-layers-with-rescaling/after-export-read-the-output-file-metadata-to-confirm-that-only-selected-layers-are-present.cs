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
        const string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Define the layers that should be exported
        string[] selectedLayers = new string[] { "Layer1", "Layer2" };

        try
        {
            // Load the source CAD file
            using (Image sourceImage = Image.Load(inputPath))
            {
                CadImage cadSource = sourceImage as CadImage;
                if (cadSource == null)
                {
                    Console.Error.WriteLine("Failed to load CAD image.");
                    return;
                }

                // Configure rasterization options to include only the selected layers
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layers = selectedLayers
                };

                // Set up DXF save options with the rasterization settings
                DxfOptions dxfOptions = new DxfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Export the CAD drawing to DXF, containing only the selected layers
                cadSource.Save(outputPath, dxfOptions);
            }

            // Verify the exported DXF file
            if (!File.Exists(outputPath))
            {
                Console.Error.WriteLine($"Exported file not found: {outputPath}");
                return;
            }

            using (Image exportedImage = Image.Load(outputPath))
            {
                CadImage cadExported = exportedImage as CadImage;
                if (cadExported == null)
                {
                    Console.Error.WriteLine("Failed to load exported CAD image.");
                    return;
                }

                Console.WriteLine("Layers present in the exported file:");
                foreach (CadLayerTable layer in cadExported.Layers)
                {
                    Console.WriteLine("- " + layer.Name);
                }

                bool onlySelected = true;
                foreach (CadLayerTable layer in cadExported.Layers)
                {
                    bool isSelected = false;
                    foreach (string sel in selectedLayers)
                    {
                        if (string.Equals(layer.Name, sel, StringComparison.OrdinalIgnoreCase))
                        {
                            isSelected = true;
                            break;
                        }
                    }
                    if (!isSelected)
                    {
                        onlySelected = false;
                        break;
                    }
                }

                Console.WriteLine("Export contains only selected layers: " + onlySelected);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}