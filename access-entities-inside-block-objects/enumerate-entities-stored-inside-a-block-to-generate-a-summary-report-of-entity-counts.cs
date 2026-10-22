using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main(string[] args)
    {
        try
        {
                // Path to the CAD file (DWG/DXF)
                string filePath = "sample.dwg";

                // Ensure the file exists
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"File not found: {filePath}");
                    return;
                }

                // Load the CAD image
                using (Image image = Image.Load(filePath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("The loaded file is not a CAD image.");
                        return;
                    }

                    // Iterate through each block entity in the drawing
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        // Count specific entity types within the block
                        int lineCount = block.Entities.OfType<CadLine>().Count();
                        int circleCount = block.Entities.OfType<CadCircle>().Count();
                        int arcCount = block.Entities.OfType<CadArc>().Count();

                        // Output a summary for the current block
                        Console.WriteLine($"Block at BasePoint ({block.BasePoint.X}, {block.BasePoint.Y}, {block.BasePoint.Z}):");
                        Console.WriteLine($"  Lines:   {lineCount}");
                        Console.WriteLine($"  Circles: {circleCount}");
                        Console.WriteLine($"  Arcs:    {arcCount}");
                        Console.WriteLine();
                    }
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}