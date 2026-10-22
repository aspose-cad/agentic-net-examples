using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

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
                if (image is not CadImage cadImage)
                {
                    Console.Error.WriteLine("Loaded file is not a CAD image.");
                    return;
                }

                bool anyExternal = false;
                bool anyUnresolved = false;

                // Iterate through block entities which may represent external references
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped) {
                    Type blockType = block.GetType();

                    // Look for a boolean property that indicates an external reference
                    PropertyInfo isExternalProp = blockType.GetProperty("IsExternalReference", BindingFlags.Public | BindingFlags.Instance);
                    if (isExternalProp != null && isExternalProp.PropertyType == typeof(bool) && (bool)isExternalProp.GetValue(block))
                    {
                        anyExternal = true;

                        // Try to obtain the referenced file path
                        string filePath = "(unknown)";
                        PropertyInfo pathProp = blockType.GetProperty("FilePath", BindingFlags.Public | BindingFlags.Instance);
                        if (pathProp != null && pathProp.PropertyType == typeof(string))
                        {
                            filePath = (string)pathProp.GetValue(block);
                        }

                        // Check if the reference is resolved
                        bool isResolved = true;
                        PropertyInfo resolvedProp = blockType.GetProperty("IsResolved", BindingFlags.Public | BindingFlags.Instance);
                        if (resolvedProp != null && resolvedProp.PropertyType == typeof(bool))
                        {
                            isResolved = (bool)resolvedProp.GetValue(block);
                        }

                        if (!isResolved)
                        {
                            anyUnresolved = true;
                            Console.WriteLine($"Unresolved external reference: {filePath}");
                        }
                        else
                        {
                            Console.WriteLine($"Resolved external reference: {filePath}");
                        }
                    }
                }

                if (!anyExternal)
                {
                    Console.WriteLine("No external references found in the drawing.");
                }
                else if (!anyUnresolved)
                {
                    Console.WriteLine("All external references are resolved.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}