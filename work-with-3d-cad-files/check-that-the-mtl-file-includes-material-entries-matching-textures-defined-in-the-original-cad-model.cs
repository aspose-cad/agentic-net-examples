using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // Paths to the OBJ model and its associated MTL file
        string objFilePath = "model.obj";
        string mtlFilePath = "model.mtl";

        // Verify that the files exist before proceeding
        if (!File.Exists(objFilePath))
        {
            Console.Error.WriteLine($"OBJ file not found: {objFilePath}");
            return;
        }

        if (!File.Exists(mtlFilePath))
        {
            Console.Error.WriteLine($"MTL file not found: {mtlFilePath}");
            return;
        }

        try
        {
            // Collect material names referenced in the OBJ file (via usemtl)
            var objMaterialNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var line in File.ReadLines(objFilePath))
            {
                var trimmed = line.Trim();
                if (trimmed.StartsWith("usemtl ", StringComparison.OrdinalIgnoreCase))
                {
                    var materialName = trimmed.Substring(7).Trim();
                    if (!string.IsNullOrEmpty(materialName))
                    {
                        objMaterialNames.Add(materialName);
                    }
                }
            }

            // Parse the .mtl file to collect defined material entries
            var mtlMaterialNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var line in File.ReadLines(mtlFilePath))
            {
                var trimmed = line.Trim();
                if (trimmed.StartsWith("newmtl ", StringComparison.OrdinalIgnoreCase))
                {
                    var materialName = trimmed.Substring(7).Trim();
                    if (!string.IsNullOrEmpty(materialName))
                    {
                        mtlMaterialNames.Add(materialName);
                    }
                }
            }

            // Report OBJ materials missing in the .mtl file
            foreach (var objMat in objMaterialNames)
            {
                if (!mtlMaterialNames.Contains(objMat))
                {
                    Console.WriteLine($"Material '{objMat}' referenced in OBJ is missing in the .mtl file.");
                }
            }

            // Report .mtl materials not used in the OBJ file
            foreach (var mtlMat in mtlMaterialNames)
            {
                if (!objMaterialNames.Contains(mtlMat))
                {
                    Console.WriteLine($"Material '{mtlMat}' present in the .mtl file is not used in the OBJ model.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Processing failed: {ex.Message}");
        }
    }
}