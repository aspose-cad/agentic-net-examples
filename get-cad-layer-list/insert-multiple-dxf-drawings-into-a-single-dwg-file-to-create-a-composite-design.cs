using System.Linq;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the composite DWG file (must exist as a base drawing)
            string targetPath = "Composite.dwg";

            // Paths to the DXF drawings to be inserted
            string[] sourcePaths = { "Part1.dxf", "Part2.dxf", "Part3.dxf" };

            // Verify that the target file exists
            if (!File.Exists(targetPath))
            {
                Console.Error.WriteLine($"Target file '{targetPath}' does not exist.");
                return;
            }

            // Load the target DWG drawing
            using (Image targetImage = Image.Load(targetPath))
            {
                CadImage targetCad = targetImage as CadImage;
                if (targetCad == null)
                {
                    Console.Error.WriteLine("Failed to cast the target image to CadImage.");
                    return;
                }

                // Process each source DXF file
                foreach (string srcPath in sourcePaths)
                {
                    if (!File.Exists(srcPath))
                    {
                        Console.Error.WriteLine($"Source file '{srcPath}' does not exist. Skipping.");
                        continue;
                    }

                    // Load the source DXF drawing
                    using (Image srcImage = Image.Load(srcPath))
                    {
                        CadImage srcCad = srcImage as CadImage;
                        if (srcCad == null)
                        {
                            Console.Error.WriteLine($"Failed to cast source '{srcPath}' to CadImage. Skipping.");
                            continue;
                        }

                        // Append each entity from the source DXF to the target DWG
                        foreach (CadEntityBase entity in srcCad.Entities)
                        {
                            // Directly add the entity to the target drawing
                            targetCad.Entities = targetCad.Entities.Concat(new CadEntityBase[] { entity }).ToArray();
                        }
                    }
                }

                // Save the composite DWG drawing (overwrites the original file)
                targetCad.Save(targetPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}