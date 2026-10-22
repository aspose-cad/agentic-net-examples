using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadIfcLineWeightExample
{
    class Program
    {
        static void Main(string[] args)
        {
            const string sourceFile = "input.ifc";
            const string outputFile = "output.png";

            if (!File.Exists(sourceFile))
            {
                Console.Error.WriteLine($"Source file not found: {sourceFile}");
                return;
            }

            try
            {
                // Load the IFC drawing
                using (Image loadedImage = Image.Load(sourceFile))
                {
                    // Cast to CadImage to access CAD-specific members
                    if (loadedImage is not CadImage cadImage)
                    {
                        Console.Error.WriteLine("Loaded image is not a CAD image.");
                        return;
                    }

                    // Adjust default line weight for all layers.
                    // Note: The current Aspose.CAD API does not expose a LineWeight property on CadLayerTable.
                    // Therefore, line weight cannot be modified directly via this SDK.
                    // We log this limitation for each layer.
                    foreach (CadLayerTable layerObj in cadImage.Layers)
                    {
                        if (layerObj is CadLayerTable layer)
                        {
                            Console.WriteLine($"Layer \"{layer.Name}\" encountered. LineWeight modification is not supported by the current SDK.");
                        }
                    }

                    // Prepare PNG export options
                    var pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = new CadRasterizationOptions
                        {
                            PageWidth = 1600,
                            PageHeight = 1200
                        }
                    };

                    // Ensure output directory exists
                    string outputDir = Path.GetDirectoryName(outputFile);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    // Save the modified drawing as PNG
                    cadImage.Save(outputFile, pngOptions);
                    Console.WriteLine($"PNG file saved to {outputFile}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}