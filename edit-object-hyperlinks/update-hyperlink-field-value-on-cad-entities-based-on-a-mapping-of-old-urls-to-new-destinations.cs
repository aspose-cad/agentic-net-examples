using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Mapping of old URLs to new destinations
            var urlMap = new Dictionary<string, string>
            {
                { "http://oldsite.com/page1", "https://newsite.com/newpage1" },
                { "http://oldsite.com/page2", "https://newsite.com/newpage2" }
            };

            // Load the CAD drawing
            using (Image image = Image.Load(inputPath))
            {
                var cadImage = (CadImage)image;

                // Update hyperlinks in top‑level entities
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    UpdateEntityHyperlink(entity, urlMap);
                }

                // Update hyperlinks inside block definitions
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        UpdateEntityHyperlink(entity, urlMap);
                    }
                }

                // Save the modified drawing
                cadImage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    // Checks an entity for a Hyperlink property and replaces its value if it matches the map
    static void UpdateEntityHyperlink(CadEntityBase entity, Dictionary<string, string> urlMap)
    {
        PropertyInfo hyperlinkProp = entity.GetType().GetProperty("Hyperlink", BindingFlags.Public | BindingFlags.Instance);
        if (hyperlinkProp != null && hyperlinkProp.PropertyType == typeof(string))
        {
            string currentUrl = hyperlinkProp.GetValue(entity) as string;
            if (!string.IsNullOrEmpty(currentUrl) && urlMap.TryGetValue(currentUrl, out string newUrl))
            {
                hyperlinkProp.SetValue(entity, newUrl);
            }
        }
    }
}