using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Aspose.CAD;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder paths for the OBJ and its associated MTL file
            string objPath = "input.obj";
            string mtlPath = Path.ChangeExtension(objPath, ".mtl");

            // Verify that the OBJ file exists
            if (!File.Exists(objPath))
            {
                Console.Error.WriteLine($"OBJ file not found: {objPath}");
                return;
            }

            // Load the OBJ file using Aspose.CAD to ensure it is a valid CAD file
            using (Image cadImage = Image.Load(objPath))
            {
                // No further processing needed with the CAD image for this check
            }

            // Verify that the MTL file exists
            if (!File.Exists(mtlPath))
            {
                Console.Error.WriteLine($"MTL file not found: {mtlPath}");
                return;
            }

            // Extract material names referenced in the OBJ file (usemtl statements)
            HashSet<string> referencedMaterials = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string line in File.ReadLines(objPath))
            {
                string trimmed = line.Trim();
                if (trimmed.StartsWith("usemtl ", StringComparison.OrdinalIgnoreCase))
                {
                    string materialName = trimmed.Substring(7).Trim();
                    if (!string.IsNullOrEmpty(materialName))
                    {
                        referencedMaterials.Add(materialName);
                    }
                }
            }

            // Extract material definitions present in the MTL file (newmtl statements)
            HashSet<string> definedMaterials = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string line in File.ReadLines(mtlPath))
            {
                string trimmed = line.Trim();
                if (trimmed.StartsWith("newmtl ", StringComparison.OrdinalIgnoreCase))
                {
                    string materialName = trimmed.Substring(7).Trim();
                    if (!string.IsNullOrEmpty(materialName))
                    {
                        definedMaterials.Add(materialName);
                    }
                }
            }

            // Determine missing material definitions
            var missingMaterials = referencedMaterials.Except(definedMaterials).ToList();

            if (missingMaterials.Count == 0)
            {
                Console.WriteLine("All referenced materials are defined in the MTL file.");
            }
            else
            {
                Console.WriteLine("The following materials are referenced in the OBJ but missing in the MTL file:");
                foreach (string mat in missingMaterials)
                {
                    Console.WriteLine($"- {mat}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}