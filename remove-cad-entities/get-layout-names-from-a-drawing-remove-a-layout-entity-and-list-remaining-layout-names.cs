using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        string inputPath = "input.dwg";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // List layout names before removal
                Console.WriteLine("Layouts before removal:");
                foreach (KeyValuePair<string, CadLayout> kvp in cadImage.Layouts)
                {
                    Console.WriteLine(kvp.Value.LayoutName);
                }

                // Determine a layout to remove (first one found)
                string layoutKeyToRemove = null;
                foreach (KeyValuePair<string, CadLayout> kvp in cadImage.Layouts)
                {
                    layoutKeyToRemove = kvp.Key;
                    break;
                }

                if (layoutKeyToRemove != null)
                {
                    // Cast to mutable dictionary to allow removal
                    IDictionary<string, CadLayout> mutableLayouts = (IDictionary<string, CadLayout>)cadImage.Layouts;
                    bool removed = mutableLayouts.Remove(layoutKeyToRemove);
                    Console.WriteLine($"Removed layout '{layoutKeyToRemove}': {removed}");
                }
                else
                {
                    Console.WriteLine("No layouts found to remove.");
                }

                // List layout names after removal
                Console.WriteLine("Layouts after removal:");
                foreach (KeyValuePair<string, CadLayout> kvp in cadImage.Layouts)
                {
                    Console.WriteLine(kvp.Value.LayoutName);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}