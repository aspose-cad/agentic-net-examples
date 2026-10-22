using System;
using System.Collections.Generic;
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
                // Input and output CAD file paths
                string inputPath = "input.dwg";
        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        string outputPath = "output.dwg";

                // Mapping of old hyperlink URLs to new URLs
                Dictionary<string, string> hyperlinkMap = new Dictionary<string, string>()
                {
                    { "http://oldsite.com/page1", "https://newsite.com/page1" },
                    { "http://oldsite.com/page2", "https://newsite.com/page2" }
                };

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to work with CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Iterate over top‑level entities in the drawing
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // If the entity has a hyperlink and it exists in the mapping, update it
                        if (!string.IsNullOrEmpty(entity.Hyperlink) && hyperlinkMap.ContainsKey(entity.Hyperlink))
                        {
                            entity.Hyperlink = hyperlinkMap[entity.Hyperlink];
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