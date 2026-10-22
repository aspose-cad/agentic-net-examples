using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            // Define input CAD file path (replace with an actual file path)
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Prepare output directory
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
            Directory.CreateDirectory(outputDir);

            // Define output OBJ file path
            string objPath = Path.Combine(outputDir, "model.obj");

            // Load CAD image
            using (Image cadImage = Image.Load(inputPath))
            {
                // Set OBJ export options (default options generate .mtl file)
                ObjOptions objOptions = new ObjOptions();

                // Save as OBJ
                cadImage.Save(objPath, objOptions);
            }

            // Verify that the OBJ file contains an mtllib line referencing the .mtl file
            if (!File.Exists(objPath))
            {
                Console.Error.WriteLine($"OBJ file was not created: {objPath}");
                return;
            }

            string[] objLines = File.ReadAllLines(objPath);
            string mtllibLine = null;
            foreach (string line in objLines)
            {
                if (line.StartsWith("mtllib ", StringComparison.OrdinalIgnoreCase))
                {
                    mtllibLine = line;
                    break;
                }
            }

            if (mtllibLine == null)
            {
                Console.Error.WriteLine("OBJ file does not contain an mtllib line.");
                return;
            }

            // Extract referenced MTL file name
            string referencedMtl = mtllibLine.Substring("mtllib ".Length).Trim();

            // Build full path to the referenced MTL file (relative to OBJ location)
            string referencedMtlPath = Path.Combine(Path.GetDirectoryName(objPath) ?? string.Empty, referencedMtl);

            if (File.Exists(referencedMtlPath))
            {
                Console.WriteLine($"Verification succeeded: OBJ references existing MTL file '{referencedMtl}'.");
            }
            else
            {
                Console.Error.WriteLine($"Verification failed: Referenced MTL file not found: {referencedMtlPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}