using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadFontChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the CAD file to be examined
                string cadFilePath = "input.dwg";

                // Verify the input file exists
                if (!File.Exists(cadFilePath))
                {
                    Console.WriteLine($"Input file not found: {cadFilePath}");
                    return;
                }

                // Load the CAD drawing
                using (Image image = Image.Load(cadFilePath))
                {
                    // Cast the loaded image to CadImage to access CAD-specific members
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("The loaded file is not a supported CAD format.");
                        return;
                    }

                    // Collect fonts used in the CAD drawing
                    HashSet<string> usedFontNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    // Include the default font defined for the drawing, if any
                    if (!string.IsNullOrEmpty(cadImage.DefaultFont))
                    {
                        usedFontNames.Add(cadImage.DefaultFont);
                    }

                    // NOTE: The Aspose.CAD API does not expose font information for CadText,
                    // CadMText, or attribute entities. Therefore, only the drawing's default
                    // font can be detected via the available API.

                    // Output the results
                    Console.WriteLine("Fonts detected in the CAD drawing (limited to DefaultFont):");
                    if (usedFontNames.Count > 0)
                    {
                        foreach (string fontName in usedFontNames)
                        {
                            Console.WriteLine(" - " + fontName);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No font information available.");
                    }

                    // Determine missing fonts
                    // NOTE: Retrieving the list of fonts installed on the system typically
                    // requires System.Drawing or platform‑specific APIs, which are not
                    // permitted in this console‑only sample. Consequently, missing‑font
                    // detection cannot be performed here.
                    Console.WriteLine();
                    Console.WriteLine("Missing font detection is not supported in this sample due to the lack of a permitted system‑font enumeration API.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}