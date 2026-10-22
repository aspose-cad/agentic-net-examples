using System;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
                // Get input CAD file path and new hyperlink URL from the user
                Console.Write("Enter path to CAD file: ");
                string inputPath = Console.ReadLine();
        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        Console.Write("Enter new hyperlink URL: ");
                string newUrl = Console.ReadLine();

                // Load the CAD file
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("Failed to load CAD image.");
                        return;
                    }

                    // Iterate through top‑level entities and update any hyperlink entity
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Identify hyperlink entities by type name (Aspose.CAD may expose a CadHyperlink class)
                        if (entity.GetType().Name == "CadHyperlink")
                        {
                            // Use reflection to set the URL property if it exists
                            var urlProperty = entity.GetType().GetProperty("Url");
                            if (urlProperty != null && urlProperty.CanWrite)
                            {
                                urlProperty.SetValue(entity, newUrl);
                                Console.WriteLine("Hyperlink URL updated.");
                            }
                        }
                    }

                    // Save the modified CAD file with a new name
                    string outputPath = System.IO.Path.Combine(
                        System.IO.Path.GetDirectoryName(inputPath),
                        System.IO.Path.GetFileNameWithoutExtension(inputPath) + "_updated" + System.IO.Path.GetExtension(inputPath));

                    cadImage.Save(outputPath);
                    Console.WriteLine($"Updated CAD file saved to: {outputPath}");
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}