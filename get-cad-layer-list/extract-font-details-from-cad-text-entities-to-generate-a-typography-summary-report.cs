using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadTypographyReport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the CAD drawing
                string inputPath = "input.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Prepare a list to hold report lines
                    List<string> reportLines = new List<string>();

                    // Add default font information
                    string defaultFont = cadImage.DefaultFont;
                    reportLines.Add($"Default Font: {defaultFont}");
                    reportLines.Add(string.Empty);

                    // Iterate over top‑level entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Process single line text entities
                        if (entity is CadText cadText)
                        {
                            string textContent = cadText.DefaultValue;
                            double fontSize = cadText.TextHeight;
                            reportLines.Add($"Text Entity - Content: \"{textContent}\", Font Size: {fontSize}");
                        }
                        // Process multi‑line text entities
                        else if (entity is CadMText cadMText)
                        {
                            string textContent = cadMText.Text;
                            double fontSize = cadMText.InitialTextHeight;
                            reportLines.Add($"MText Entity - Content: \"{textContent}\", Font Size: {fontSize}");
                        }
                    }

                    // Output the typography summary report
                    Console.WriteLine("=== Typography Summary Report ===");
                    foreach (string line in reportLines)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}