using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        try
        {
            string sourcePath = "input.dwg";
            string exportedPath = "output.dxf";

            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            using (Image sourceImage = Image.Load(sourcePath))
            {
                if (sourceImage is not CadImage cadSource)
                {
                    Console.WriteLine("Failed to load the source CAD file.");
                    return;
                }

                string[] allowedLayers = { "Layer1", "Layer2" };

                var rasterOptions = new CadRasterizationOptions
                {
                    Layers = allowedLayers
                };

                var dxfOptions = new DxfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadSource.Save(exportedPath, dxfOptions);
            }

            if (!File.Exists(exportedPath))
            {
                Console.WriteLine($"Exported file not created: {exportedPath}");
                return;
            }

            using (Image exportedImage = Image.Load(exportedPath))
            {
                if (exportedImage is not CadImage cadExported)
                {
                    Console.WriteLine("Failed to load the exported CAD file.");
                    return;
                }

                var exportedLayerNames = new List<string>();
                foreach (CadLayerTable layer in cadExported.Layers)
                {
                    exportedLayerNames.Add(layer.Name);
                }

                string[] expectedLayers = { "Layer1", "Layer2" };

                bool verificationPassed = true;

                foreach (string layerName in exportedLayerNames)
                {
                    if (Array.IndexOf(expectedLayers, layerName) < 0)
                    {
                        verificationPassed = false;
                        Console.WriteLine($"Unexpected layer found: {layerName}");
                    }
                }

                if (verificationPassed && exportedLayerNames.Count == expectedLayers.Length)
                {
                    Console.WriteLine("Verification succeeded: only the specified layers are present.");
                }
                else
                {
                    Console.WriteLine("Verification failed: layer mismatch.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}