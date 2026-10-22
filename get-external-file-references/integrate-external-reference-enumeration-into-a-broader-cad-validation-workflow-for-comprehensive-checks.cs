using System;
using System.IO;
using System.Collections;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the CAD file to validate
            string cadFilePath = "sample.dwg";

            // Load the CAD file and cast to CadImage
            using (Image image = Image.Load(cadFilePath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Unable to load CAD image.");
                    return;
                }

                // Perform layer validation
                ValidateLayers(cadImage);

                // Perform external reference (Xref) validation
                ValidateExternalReferences(cadImage);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }

    static void ValidateLayers(CadImage cadImage)
    {
        // Iterate through all layers (untyped enumeration)
        foreach (CadLayerTable layerObj in cadImage.Layers)
        {
            CadLayerTable layer = layerObj as CadLayerTable;
            if (layer == null) continue;

            // Example check: layer name should not be empty
            if (string.IsNullOrWhiteSpace(layer.Name))
            {
                Console.WriteLine("Found a layer with an empty name.");
            }
        }
    }

    static void ValidateExternalReferences(CadImage cadImage)
    {
        // External references are typically stored as block entities whose names start with "*XREF"
        foreach (DictionaryEntry entry in cadImage.BlockEntities)
        {
            CadBlockEntity block = entry.Value as CadBlockEntity;
            if (block == null) continue;

            string blockName = block.Name;
            if (blockName != null && blockName.StartsWith("*XREF", StringComparison.OrdinalIgnoreCase))
            {
                // Attempt to retrieve the external reference path (may be unavailable in the API)
                string path = GetXrefPath(block);
                Console.WriteLine($"External reference: Name = {blockName}, Path = {path}");

                // Verify that the referenced file exists
                if (!string.IsNullOrEmpty(path) && !File.Exists(path))
                {
                    Console.WriteLine($"Missing external reference file: {path}");
                }

                // Example additional check: limit nesting depth
                int nestingDepth = GetReferenceNestingDepth(cadImage, blockName, 0);
                if (nestingDepth > 3)
                {
                    Console.WriteLine($"Reference '{blockName}' exceeds allowed nesting depth ({nestingDepth}).");
                }
            }
        }
    }

    // Helper to obtain Xref path via reflection (placeholder for actual property)
    static string GetXrefPath(CadBlockEntity block)
    {
        var prop = block.GetType().GetProperty("XrefPath");
        if (prop != null)
        {
            var value = prop.GetValue(block) as string;
            return value ?? string.Empty;
        }
        return string.Empty;
    }

    static int GetReferenceNestingDepth(CadImage currentImage, string targetBlockName, int currentDepth)
    {
        foreach (DictionaryEntry entry in currentImage.BlockEntities)
        {
            CadBlockEntity block = entry.Value as CadBlockEntity;
            if (block == null) continue;

            string name = block.Name;
            if (name != null && name.Equals(targetBlockName, StringComparison.OrdinalIgnoreCase))
            {
                return currentDepth + 1;
            }

            // If this block is itself an external reference, recurse into it
            if (name != null && name.StartsWith("*XREF", StringComparison.OrdinalIgnoreCase))
            {
                string path = GetXrefPath(block);
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    using (Image nestedImg = Image.Load(path))
                    {
                        CadImage nestedCad = nestedImg as CadImage;
                        if (nestedCad != null)
                        {
                            int depth = GetReferenceNestingDepth(nestedCad, targetBlockName, currentDepth + 1);
                            if (depth > 0) return depth;
                        }
                    }
                }
            }
        }
        return 0;
    }
}