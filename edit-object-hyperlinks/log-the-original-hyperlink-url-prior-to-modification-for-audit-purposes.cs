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
                // Path to the source CAD file
                string inputPath = "input.dwg";
        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Path for the modified CAD file
                string outputPath = "output.dwg";

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Iterate over top‑level drawing entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Identify MText entities that may contain hyperlink URLs
                        if (entity is CadMText mtext)
                        {
                            string originalText = mtext.Text;
                            // Simple check for a URL pattern
                            if (!string.IsNullOrEmpty(originalText) &&
                                originalText.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                            {
                                // Log the original hyperlink URL for audit
                                Console.WriteLine($"Original hyperlink URL: {originalText}");

                                // Example modification: replace the domain part of the URL
                                string modifiedText = originalText.Replace("oldsite.com", "newsite.com");
                                mtext.Text = modifiedText;
                            }
                        }
                    }

                    // Save the modified drawing
                    cadImage.Save(outputPath);
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}