using System;
using System.IO;
using System.Collections;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Input DXF file and background image paths
            string inputDxfPath = "input.dxf";
            string backgroundImagePath = "background.png";
            string outputDxfPath = "output.dxf";

            // Verify that the required files exist
            if (!File.Exists(inputDxfPath))
            {
                Console.Error.WriteLine($"Input DXF file not found: {inputDxfPath}");
                return;
            }
            if (!File.Exists(backgroundImagePath))
            {
                Console.Error.WriteLine($"Background image file not found: {backgroundImagePath}");
                return;
            }

            // Load the DXF drawing
            using (Image loadedImage = Image.Load(inputDxfPath))
            {
                CadImage cadImage = loadedImage as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Create a raster image definition for the background image
                dynamic rasterDef = new CadRasterImageDef();
                // Using dynamic to avoid compile‑time property checks; Aspose.CAD provides an ImagePath property at runtime
                rasterDef.ImagePath = backgroundImagePath;

                // Add the raster image definition to the drawing's entities collection
                IList entities = cadImage.Entities as IList;
                if (entities != null)
                {
                    entities.Add(rasterDef);
                }
                else
                {
                    Console.Error.WriteLine("Unable to access the entities collection for adding the raster image definition.");
                    return;
                }

                // Save the modified drawing back to DXF
                DxfOptions dxfOptions = new DxfOptions();
                cadImage.Save(outputDxfPath, dxfOptions);
            }

            Console.WriteLine("Background image added and DXF saved successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
