using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadFontExtractor
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the CAD drawing
            string inputPath = "input.dwg";

            try
            {
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // Collection to store unique font names
                    HashSet<string> fonts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    // Add the default font defined for the drawing
                    if (!string.IsNullOrEmpty(cadImage.DefaultFont))
                    {
                        fonts.Add(cadImage.DefaultFont);
                    }

                    // Inspect top‑level entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        string typeName = entity.TypeName.ToString();

                        // Process single line text entities
                        if (typeName == "CadText")
                        {
                            ExtractFontFromEntity(entity, fonts, cadImage.DefaultFont);
                        }
                        // Process multi‑line text entities
                        else if (typeName == "CadMText")
                        {
                            ExtractFontFromEntity(entity, fonts, cadImage.DefaultFont);
                        }
                    }

                    // Output the collected font names
                    Console.WriteLine("Fonts used in text entities:");
                    foreach (string fontName in fonts)
                    {
                        Console.WriteLine("- " + fontName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // Helper method to extract font information using reflection
        private static void ExtractFontFromEntity(CadEntityBase entity, HashSet<string> fonts, string defaultFont)
        {
            // Try to get a property named "Font"
            PropertyInfo fontProp = entity.GetType().GetProperty("Font", BindingFlags.Public | BindingFlags.Instance);
            if (fontProp != null)
            {
                object fontValue = fontProp.GetValue(entity);
                if (fontValue != null)
                {
                    string fontName = fontValue.ToString();
                    if (!string.IsNullOrEmpty(fontName))
                    {
                        fonts.Add(fontName);
                        return;
                    }
                }
            }

            // If "Font" not found, try "TextStyle" which may contain a FontFamilyName
            PropertyInfo styleProp = entity.GetType().GetProperty("TextStyle", BindingFlags.Public | BindingFlags.Instance);
            if (styleProp != null)
            {
                object styleObj = styleProp.GetValue(entity);
                if (styleObj != null)
                {
                    PropertyInfo familyProp = styleObj.GetType().GetProperty("FontFamilyName", BindingFlags.Public | BindingFlags.Instance);
                    if (familyProp != null)
                    {
                        object familyValue = familyProp.GetValue(styleObj);
                        if (familyValue != null)
                        {
                            string fontName = familyValue.ToString();
                            if (!string.IsNullOrEmpty(fontName))
                            {
                                fonts.Add(fontName);
                                return;
                            }
                        }
                    }
                }
            }

            // Fallback to the drawing's default font
            if (!string.IsNullOrEmpty(defaultFont))
            {
                fonts.Add(defaultFont);
            }
        }
    }
}