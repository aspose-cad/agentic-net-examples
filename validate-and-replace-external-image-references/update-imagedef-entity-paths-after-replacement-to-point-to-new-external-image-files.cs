using System;
using System.IO;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.dwg";
            string outputPath = "output.dwg";
            string newImagePath = "newImage.png";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    Type entityType = entity.GetType();
                    if (entityType.Name.Equals("CadImageDef", StringComparison.OrdinalIgnoreCase))
                    {
                        PropertyInfo pathProp = entityType.GetProperty("ImageDefPath", BindingFlags.Public | BindingFlags.Instance);
                        if (pathProp != null && pathProp.CanWrite)
                        {
                            pathProp.SetValue(entity, newImagePath);
                        }
                    }
                }

                var saveOptions = new DwgOptions();
                cadImage.Save(outputPath);
            }

            Console.WriteLine("Image definition paths updated and file saved successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}