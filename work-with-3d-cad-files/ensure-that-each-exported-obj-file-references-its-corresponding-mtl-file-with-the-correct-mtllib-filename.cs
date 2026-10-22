using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace AsposeCadObjExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder input CAD file path
                string inputPath = "input.dwg";

                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Prepare output directory
                string outputDir = "Output";
                Directory.CreateDirectory(outputDir);

                // Define output OBJ file path and corresponding MTL file name
                string outputObjPath = Path.Combine(outputDir, "output.obj");
                string mtlFileName = "output.mtl";

                // Load the CAD image
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Configure OBJ export options
                    ObjOptions objOptions = new ObjOptions
                    {
                        MtlFileName = mtlFileName // Reference the correct material library
                    };

                    // Save as OBJ
                    cadImage.Save(outputObjPath, objOptions);
                }

                Console.WriteLine($"OBJ file saved to: {outputObjPath}");
                Console.WriteLine($"Material library referenced as: {mtlFileName}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}