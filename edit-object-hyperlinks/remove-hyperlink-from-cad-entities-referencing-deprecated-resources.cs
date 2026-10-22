using System;
using System.IO;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Paths to the source and destination CAD files
            string sourcePath = "input.dwg";
            string destinationPath = "output.dwg";

            // Verify source file exists
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // Ensure destination directory exists
            string destDir = Path.GetDirectoryName(destinationPath);
            if (!string.IsNullOrEmpty(destDir) && !Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            // Load the CAD drawing
            using (Image loadedImage = Image.Load(sourcePath))
            {
                // Cast to CadImage to access CAD object model
                CadImage cadImage = (CadImage)loadedImage;

                // Iterate over top‑level drawing entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // Use reflection to check for a Hyperlink property (if present)
                    PropertyInfo hyperlinkProperty = entity.GetType().GetProperty("Hyperlink", BindingFlags.Public | BindingFlags.Instance);
                    if (hyperlinkProperty != null && hyperlinkProperty.CanRead && hyperlinkProperty.CanWrite)
                    {
                        string hyperlinkValue = hyperlinkProperty.GetValue(entity) as string;
                        // Identify hyperlinks that reference deprecated resources
                        if (!string.IsNullOrEmpty(hyperlinkValue) && hyperlinkValue.Contains("deprecated.example.com"))
                        {
                            // Remove the hyperlink by setting it to null
                            hyperlinkProperty.SetValue(entity, null);
                        }
                    }
                }

                // Save the modified drawing back to DWG format
                cadImage.Save(destinationPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}