using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadToObjExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input CAD file path (DWG, DXF, etc.)
                string inputPath = "input.dwg";
                // Output OBJ file path
                string outputPath = "output.obj";

                // Verify input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load CAD image
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Cast to CadImage for CAD-specific members if needed
                    CadImage cad = cadImage as CadImage;
                    if (cad == null)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // Prepare OBJ export options with default rasterization settings
                    ObjOptions objOptions = new ObjOptions();
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    objOptions.VectorRasterizationOptions = rasterOptions;

                    // Export to OBJ
                    cad.Save(outputPath, objOptions);
                }

                Console.WriteLine($"CAD file successfully exported to OBJ: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}