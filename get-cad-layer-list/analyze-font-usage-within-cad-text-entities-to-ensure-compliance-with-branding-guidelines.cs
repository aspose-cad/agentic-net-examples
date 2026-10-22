using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.Collections;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        try
        {
                // Path to the CAD file
                string inputPath = "sample.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"File not found: {inputPath}");
                    return;
                }

                // Load the CAD image
                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    // Output the default font defined for the drawing (if any)
                    Console.WriteLine($"Default font (if defined): {cadImage.DefaultFont}");

                    // Inspect text entities in the main drawing space
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (entity is CadText textEntity)
                        {
                            string fontInfo = string.IsNullOrEmpty(cadImage.DefaultFont) ? "Unknown" : cadImage.DefaultFont;
                            Console.WriteLine($"Text entity Font: {fontInfo}");
                        }
                        else if (entity is CadMText mTextEntity)
                        {
                            string fontInfo = string.IsNullOrEmpty(cadImage.DefaultFont) ? "Unknown" : cadImage.DefaultFont;
                            Console.WriteLine($"MText entity Font: {fontInfo}");
                        }
                    }

                    // Inspect text entities inside block definitions
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        foreach (CadEntityBase blockEntity in block.Entities)
                        {
                            if (blockEntity is CadText blockText)
                            {
                                string fontInfo = string.IsNullOrEmpty(cadImage.DefaultFont) ? "Unknown" : cadImage.DefaultFont;
                                Console.WriteLine($"Block '{block.Name}' Text entity Font: {fontInfo}");
                            }
                            else if (blockEntity is CadMText blockMText)
                            {
                                string fontInfo = string.IsNullOrEmpty(cadImage.DefaultFont) ? "Unknown" : cadImage.DefaultFont;
                                Console.WriteLine($"Block '{block.Name}' MText entity Font: {fontInfo}");
                            }
                        }
                    }
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}