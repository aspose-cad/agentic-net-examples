using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Input CAD file path (replace with actual file)
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Export with default observer point
                ObjOptions defaultOptions = new ObjOptions();
                string defaultOutput = "output_default.obj";

                string defaultDir = Path.GetDirectoryName(defaultOutput);
                if (!string.IsNullOrEmpty(defaultDir) && !Directory.Exists(defaultDir))
                {
                    Directory.CreateDirectory(defaultDir);
                }

                cadImage.Save(defaultOutput, defaultOptions);
                Console.WriteLine($"Default OBJ exported to: {defaultOutput}");

                // Export with a modified observer point
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    // Example observer point coordinates; adjust as required
                    ObserverPoint = new ObserverPoint(100, 100, 100)
                };

                ObjOptions modifiedOptions = new ObjOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                string modifiedOutput = "output_modified.obj";

                string modifiedDir = Path.GetDirectoryName(modifiedOutput);
                if (!string.IsNullOrEmpty(modifiedDir) && !Directory.Exists(modifiedDir))
                {
                    Directory.CreateDirectory(modifiedDir);
                }

                cadImage.Save(modifiedOutput, modifiedOptions);
                Console.WriteLine($"Modified OBJ exported to: {modifiedOutput}");
            }

            // Simple comparison of the two exported files
            string defaultFile = "output_default.obj";
            string modifiedFile = "output_modified.obj";

            if (File.Exists(defaultFile) && File.Exists(modifiedFile))
            {
                long defaultSize = new FileInfo(defaultFile).Length;
                long modifiedSize = new FileInfo(modifiedFile).Length;

                Console.WriteLine($"Default OBJ size: {defaultSize} bytes");
                Console.WriteLine($"Modified OBJ size: {modifiedSize} bytes");
                Console.WriteLine(defaultSize == modifiedSize
                    ? "Both OBJ files have the same size."
                    : "OBJ files differ in size.");
            }
            else
            {
                Console.Error.WriteLine("One or both exported OBJ files were not created.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}