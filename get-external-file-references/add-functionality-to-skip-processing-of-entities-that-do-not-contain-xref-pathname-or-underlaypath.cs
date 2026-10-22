using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadProcessingSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define input and output paths
            string inputPath = "input.dwg";
            string outputPath = "output.dxf";

            // Ensure a minimal placeholder file exists if the input is missing
            if (!File.Exists(inputPath))
            {
                // Create a very small dummy DWG file (header only)
                byte[] dummyDwG = new byte[] { 0x41, 0x44, 0x46, 0x00 };
                File.WriteAllBytes(inputPath, dummyDwG);
                Console.WriteLine($"Placeholder DWG file created at '{inputPath}'.");
            }

            try
            {
                // Load the CAD image
                CadImage cadImage = (CadImage)Image.Load(inputPath);

                // Process entities, skipping those without a PathName (XRef or Underlay)
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    var pathProp = entity.GetType().GetProperty("PathName");
                    if (pathProp != null)
                    {
                        var pathValue = pathProp.GetValue(entity) as string;
                        if (string.IsNullOrEmpty(pathValue))
                        {
                            continue; // Skip entities with empty PathName
                        }
                    }

                    // Example processing: output entity type information
                    Console.WriteLine($"Processing entity of type: {entity.TypeName}");
                }

                // Save the processed CAD to DXF format
                DxfOptions dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };
                cadImage.Save(outputPath, dxfOptions);
                Console.WriteLine($"CAD file saved to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during CAD processing: {ex.Message}");
            }
        }
    }
}