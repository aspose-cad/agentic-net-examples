using System;
using System.Collections;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main(string[] args)
    {
        // Path to the DWG/DXF file
        string inputPath = "sample.dwg";

        // Filtering criteria
        string targetLayerName = "MyLayer";
        double minimumLineLength = 10.0;

        if (!System.IO.File.Exists(inputPath))
        {
            Console.WriteLine("Input file not found: " + inputPath);
            return;
        }

        // Load the CAD file
        using (Image image = Image.Load(inputPath))
        {
            // Cast to CadImage to access CAD object model
            CadImage cadImage = (CadImage)image;

            // Iterate through block definitions
            foreach (DictionaryEntry entry in cadImage.BlockEntities)
            {
                CadBlockEntity block = (CadBlockEntity)entry.Value;

                // Iterate through entities inside the block
                foreach (CadEntityBase entity in block.Entities)
                {
                    // Process only line entities
                    if (entity is CadLine line)
                    {
                        double dx = line.SecondPoint.X - line.FirstPoint.X;
                        double dy = line.SecondPoint.Y - line.FirstPoint.Y;
                        double dz = line.SecondPoint.Z - line.FirstPoint.Z;
                        double length = Math.Sqrt(dx * dx + dy * dy + dz * dz);

                        // Apply custom filtering criteria
                        if (line.LayerName == targetLayerName && length >= minimumLineLength)
                        {
                            Console.WriteLine($"Found line on layer '{line.LayerName}' with length {length}");
                        }
                    }
                }
            }
        }
    }
}