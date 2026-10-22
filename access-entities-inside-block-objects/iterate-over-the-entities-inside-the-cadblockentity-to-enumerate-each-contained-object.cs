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
            try
            {
                // Path to the DWG/DXF file
                string cadFilePath = "sample.dwg";

                // Verify that the file exists before attempting to load it
                if (!File.Exists(cadFilePath))
                {
                    Console.Error.WriteLine($"File not found: {cadFilePath}");
                    return;
                }

                // Load the CAD file
                using (Image image = Image.Load(cadFilePath))
                {
                    // Cast to CadImage to access CAD object model members
                    if (image is CadImage cadImage)
                    {
                        // Iterate over block definitions in the drawing
                        foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                        {
                            // Output basic block information
                            Console.WriteLine($"Block Base Point: X={block.BasePoint.X}, Y={block.BasePoint.Y}, Z={block.BasePoint.Z}");

                            // Iterate over entities contained in the block
                            foreach (CadEntityBase entity in block.Entities)
                            {
                                // Output the type of each entity
                                Console.WriteLine($"  Entity Type: {entity.GetType().Name}");
                            }
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine("Loaded image is not a CAD image.");
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
}