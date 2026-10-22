using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

namespace CadLayoutExport
{
    class Program
    {
        static void Main()
        {
            // Paths to the drawings
            string mainDrawingPath = "main.dwg";
            string blockDrawingPath = "block.dwg";

            // Verify input files exist
            if (!File.Exists(mainDrawingPath) || !File.Exists(blockDrawingPath))
            {
                Console.WriteLine("One or both input files were not found.");
                return;
            }

            try
            {
                // Load the main drawing
                using (Image mainImage = Image.Load(mainDrawingPath))
                {
                    CadImage mainCadImage = (CadImage)mainImage;

                    // Load the external block drawing
                    using (Image blockImage = Image.Load(blockDrawingPath))
                    {
                        CadImage blockCadImage = (CadImage)blockImage;

                        // Append all entities from the block drawing to the main drawing
                        mainCadImage.Entities = mainCadImage.Entities
                            .Concat(blockCadImage.Entities)
                            .ToArray();
                    }

                    // Export the list of layout names after inserting the block
                    foreach (var layoutName in mainCadImage.Layouts.Keys)
                    {
                        Console.WriteLine(layoutName);
                    }

                    // Optional: save the modified drawing (preserving objects)
                    // string outputPath = "modified.dxf";
                    // Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                    // DxfOptions saveOptions = new DxfOptions
                    // {
                    //     OutputMode = CadOutputMode.Convert,
                    //     DxfFileFormat = CadConsts.CadFileFormat.Binary
                    // };
                    // mainCadImage.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}