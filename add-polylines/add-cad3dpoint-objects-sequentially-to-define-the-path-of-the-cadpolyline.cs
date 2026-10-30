using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.ImageOptions;

namespace CadPolylineExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input CAD file (DWG, DXF, etc.)
            string inputPath = "input.dwg";


        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }
// Output CAD file
            string outputPath = Path.Combine("output", "output.dwg");

            // Ensure the output directory exists
            try
            {
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating output directory: {ex.Message}");
                return;
            }

            // Load the CAD image
            CadImage cadImage;
            try
            {
                var image = Image.Load(inputPath);
                cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Failed to load CAD image as CadImage.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading CAD file: {ex.Message}");
                return;
            }

            using (cadImage)
            {
                // Define the polyline points sequentially
                var points = new[]
                {
                    new Cad3DPoint(0.0, 0.0, 0.0),
                    new Cad3DPoint(100.0, 0.0, 0.0),
                    new Cad3DPoint(100.0, 100.0, 0.0),
                    new Cad3DPoint(0.0, 100.0, 0.0),
                    new Cad3DPoint(0.0, 0.0, 0.0) // Closing the polyline
                };

                // Create a new 3D polyline
                var polyline = new CadPolyline(points);

                // Add the polyline to the model space block
                // The native model space block key is "*MODEL_SPACE"
                if (cadImage.BlockEntities.TryGetValue("*MODEL_SPACE", out var modelSpace))
                {
                    modelSpace.AddEntity(polyline);
                }
                else
                {
                    Console.WriteLine("Model space block not found. Cannot add polyline.");
                    return;
                }

                // Save the modified CAD image
                var saveOptions = new DwgOptions();
                try
                {
                    cadImage.Save(outputPath, saveOptions);
                    Console.WriteLine($"File saved to: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving file: {ex.Message}");
                }
            }
        }
    }
}