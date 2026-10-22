using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "layout.png";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine("Input file not found.");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)image;

                // Get the first layout in the drawing
                CadLayout layout = cadImage.Layouts.Values.First();

                // Retrieve the block that defines the layout's contents
                string blockHandle = layout.BlockTableRecordHandle;
                if (!cadImage.BlockEntities.ContainsKey(blockHandle))
                {
                    Console.WriteLine("Block for layout not found.");
                    return;
                }

                CadBlockEntity block = cadImage.BlockEntities[blockHandle] as CadBlockEntity;
                if (block == null)
                {
                    Console.WriteLine("Block entity is not a CadBlockEntity.");
                    return;
                }

                // Example: remove the first line entity found in the block
                CadEntityBase entityToRemove = block.Entities.OfType<CadLine>().FirstOrDefault();
                if (entityToRemove != null)
                {
                    // Create a new collection without the entity to remove
                    CadEntityBase[] newEntities = block.Entities.Where(e => e != entityToRemove).ToArray();

                    // Replace the block's entity collection
                    block.Entities = newEntities;

                    Console.WriteLine("Removed one line entity from the layout block.");
                }
                else
                {
                    Console.WriteLine("No line entity found to remove.");
                }

                // Export the layout to a PNG file
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layouts = new[] { layout.LayoutName },
                    PageWidth = 1600,
                    PageHeight = 1200,
                    DrawType = CadDrawTypeMode.UseObjectColor
                };

                PngOptions pngOptions = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                cadImage.Save(outputPath, pngOptions);
                Console.WriteLine($"Layout exported to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}