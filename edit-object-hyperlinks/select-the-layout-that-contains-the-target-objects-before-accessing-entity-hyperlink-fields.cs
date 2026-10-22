using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string sourcePath = "sample.dwg";
            string targetLayout = "Layout1";

            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            using (Image image = Image.Load(sourcePath))
            {
                CadImage cadImage = (CadImage)image;

                // Verify the requested layout exists
                if (!cadImage.Layouts.ContainsKey(targetLayout))
                {
                    Console.Error.WriteLine($"Layout \"{targetLayout}\" not found in the drawing.");
                    return;
                }

                // Restrict rasterization to the target layout (if rendering is needed later)
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    Layouts = new[] { targetLayout }
                };

                // Process model space entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    string hyperlink = GetEntityHyperlink(entity);
                    if (!string.IsNullOrEmpty(hyperlink))
                    {
                        Console.WriteLine($"Entity Type: {entity.GetType().Name}, Hyperlink: {hyperlink}");
                    }
                }

                // Process block entities
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        string hyperlink = GetEntityHyperlink(entity);
                        if (!string.IsNullOrEmpty(hyperlink))
                        {
                            Console.WriteLine($"Block: {block.Name}, Entity Type: {entity.GetType().Name}, Hyperlink: {hyperlink}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    static string GetEntityHyperlink(CadEntityBase entity)
    {
        PropertyInfo prop = entity.GetType().GetProperty("Hyperlink", BindingFlags.Public | BindingFlags.Instance);
        if (prop != null)
        {
            object value = prop.GetValue(entity);
            return value as string;
        }
        return null;
    }
}