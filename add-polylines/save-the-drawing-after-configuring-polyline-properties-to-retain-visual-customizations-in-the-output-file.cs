using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadBlockInspection
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input CAD file path (replace with actual path)
            string inputPath = "input.dwg";
            // Output CAD file path (replace with desired path)
            string outputPath = "output.dwg";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD drawing with error handling
            CadImage cadImage;
            try
            {
                using (Image image = Image.Load(inputPath))
                {
                    cadImage = (CadImage)image;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load CAD file: {ex.Message}");
                return;
            }

            // Process block entities
            foreach (CadBlockEntity block in cadImage.BlockEntities.Values)
            {
                foreach (CadEntityBase entity in block.Entities)
                {
                    if (entity is CadLwPolyline polyline)
                    {
                        for (int i = 0; i < polyline.Coordinates.Count; i++)
                        {
                            Cad2DPoint original = polyline.Coordinates[i];
                            Cad2DPoint shifted = new Cad2DPoint(original.X + 10, original.Y + 10);
                            polyline.Coordinates[i] = shifted;
                        }
                    }
                }
            }

            // Save the modified drawing with error handling
            try
            {
                cadImage.Save(outputPath);
                Console.WriteLine($"Modified drawing saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save CAD file: {ex.Message}");
            }
        }
    }
}
