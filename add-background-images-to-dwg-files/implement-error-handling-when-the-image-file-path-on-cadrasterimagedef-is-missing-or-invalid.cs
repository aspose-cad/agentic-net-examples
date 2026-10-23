using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadRasterImageErrorHandling
{
    class Program
    {
        static void Main(string[] args)
        {
            string cadFilePath = "input.dwg";

            // Verify that the CAD file exists before attempting to load
            try
            {
                if (!File.Exists(cadFilePath))
                {
                    Console.WriteLine($"CAD file not found: {cadFilePath}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking CAD file existence: {ex.Message}");
                return;
            }

            Image cadImage = null;
            try
            {
                cadImage = Image.Load(cadFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load CAD file: {ex.Message}");
                return;
            }

            using (cadImage)
            {
                // Cast the generic Image to a CadImage
                CadImage cadSpecificImage = cadImage as CadImage;
                if (cadSpecificImage == null)
                {
                    Console.WriteLine("Loaded image is not a CAD image.");
                    return;
                }

                // Raster image definitions are stored in the Objects collection
                if (cadSpecificImage.Objects == null)
                {
                    Console.WriteLine("No objects found in the CAD file.");
                    return;
                }

                foreach (var obj in cadSpecificImage.Objects)
                {
                    if (obj is CadRasterImageDef rasterDef)
                    {
                        string rasterPath = rasterDef.FileName;

                        // Check for missing or empty file path
                        if (string.IsNullOrWhiteSpace(rasterPath))
                        {
                            Console.WriteLine("Raster image definition has a missing or empty file path.");
                            continue;
                        }

                        // Verify the referenced raster image file exists
                        try
                        {
                            if (!File.Exists(rasterPath))
                            {
                                Console.WriteLine($"Raster image file not found: {rasterPath}");
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error checking raster image file '{rasterPath}': {ex.Message}");
                            continue;
                        }

                        // Valid raster image path – further processing can be done here
                        Console.WriteLine($"Valid raster image found: {rasterPath}");
                    }
                }
            }
        }
    }
}