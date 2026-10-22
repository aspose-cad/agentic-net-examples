using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Input CAD file (OBJ format expected to reference an .mtl file)
            string cadFilePath = "model.obj";

            if (!File.Exists(cadFilePath))
            {
                Console.Error.WriteLine($"CAD file not found: {cadFilePath}");
                return;
            }

            // Load CAD image
            using (Image loadedImage = Image.Load(cadFilePath))
            {
                // Cast to CadImage to access CAD-specific data
                Image cadImage = loadedImage ;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Loaded file is not a supported CAD image.");
                    return;
                }

                // Locate the associated .mtl file (same name, .mtl extension)
                string mtlFilePath = Path.ChangeExtension(cadFilePath, ".mtl");
                if (!File.Exists(mtlFilePath))
                {
                    Console.Error.WriteLine($".mtl file not found: {mtlFilePath}");
                    return;
                }

                // Parse .mtl file to obtain diffuse color (Kd) for each material
                Dictionary<string, (float R, float G, float B)> mtlDiffuseColors = ParseMtlFile(mtlFilePath);

                // NOTE: Aspose.CAD's current API does not expose material definitions (e.g., diffuse colors) from CadImage.
                // Therefore, a direct comparison between CAD material colors and .mtl definitions cannot be performed.
                // The following placeholder informs the user of this limitation.

                if (mtlDiffuseColors.Count == 0)
                {
                    Console.WriteLine("No diffuse color definitions found in the .mtl file.");
                }
                else
                {
                    Console.WriteLine("Parsed diffuse colors from .mtl file, but CAD material color data is unavailable via the current Aspose.CAD API.");
                    foreach (var kvp in mtlDiffuseColors)
                    {
                        Console.WriteLine($"Material \"{kvp.Key}\": Kd = ({kvp.Value.R:F3}, {kvp.Value.G:F3}, {kvp.Value.B:F3})");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Parses an .mtl file and returns a dictionary of material name to its diffuse color (Kd) in 0‑1 range.
    private static Dictionary<string, (float R, float G, float B)> ParseMtlFile(string mtlPath)
    {
        var result = new Dictionary<string, (float R, float G, float B)>(StringComparer.OrdinalIgnoreCase);
        string[] lines = File.ReadAllLines(mtlPath);
        string currentMaterial = null;

        foreach (string rawLine in lines)
        {
            string line = rawLine.Trim();

            // Skip empty lines and comments
            if (line.Length == 0 || line.StartsWith("#"))
                continue;

            if (line.StartsWith("newmtl ", StringComparison.OrdinalIgnoreCase))
            {
                currentMaterial = line.Substring(7).Trim();
                continue;
            }

            if (line.StartsWith("Kd ", StringComparison.OrdinalIgnoreCase) && currentMaterial != null)
            {
                string[] parts = line.Substring(3).Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 3 &&
                    float.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float r) &&
                    float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float g) &&
                    float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out float b))
                {
                    result[currentMaterial] = (r, g, b);
                }
            }
        }

        return result;
    }
}