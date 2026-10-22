using Aspose.CAD;
using System;
using System.IO;
using System.Reflection;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class UpdateImageDefPaths
{
    static void Main()
    {
        // Input and output paths
        string inputPath = @"C:\CAD\input.dwg";
        string outputPath = @"C:\CAD\output.dwg";
        string newImageFolder = @"C:\CAD\NewImages";

        // Validate input file
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
        Directory.CreateDirectory(newImageFolder);

        try
        {
            // Load CAD document
            CadImage cadImage = (CadImage)Aspose.CAD.Image.Load(inputPath);

            // Update IMAGEDEF entities in top‑level entities
            foreach (CadEntityBase entity in cadImage.Entities)
            {
                UpdateImageDefEntityPath(entity, newImageFolder);
            }

            // Update IMAGEDEF entities inside block definitions
            foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
            {
                foreach (CadEntityBase entity in block.Entities)
                {
                    UpdateImageDefEntityPath(entity, newImageFolder);
                }
            }

            // Save the modified CAD document preserving original format
            cadImage.Save(outputPath);
            Console.WriteLine("CAD file saved with updated IMAGEDEF paths.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }

    // Helper method to locate IMAGEDEF entities and modify their Path property
    private static void UpdateImageDefEntityPath(CadEntityBase entity, string newFolder)
    {
        // Identify IMAGEDEF entity by its concrete type name
        if (entity.GetType().Name.Equals("ImageDefEntity", StringComparison.OrdinalIgnoreCase))
        {
            // Use reflection to access the Path property
            PropertyInfo pathProp = entity.GetType().GetProperty("Path", BindingFlags.Public | BindingFlags.Instance);
            if (pathProp != null && pathProp.CanWrite)
            {
                string currentPath = pathProp.GetValue(entity) as string;
                if (!string.IsNullOrEmpty(currentPath))
                {
                    string fileName = Path.GetFileName(currentPath);
                    string updatedPath = Path.Combine(newFolder, fileName);
                    pathProp.SetValue(entity, updatedPath);
                }
            }
        }
    }
}