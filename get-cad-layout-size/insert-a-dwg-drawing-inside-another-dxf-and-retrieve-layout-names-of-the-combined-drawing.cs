using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

namespace CadLayoutSizeExample
{
    class Program
    {
        private static string FormatSize(double width, double height)
        {
            return $"Width = {width:F2}, Height = {height:F2}";
        }

        static void Main(string[] args)
        {
            // Paths to the source DWG and target DXF files.
            string sourcePath = "source.dwg";   // DWG to be inserted
            string targetPath = "target.dxf";   // DXF that will receive the DWG

            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Error: Source file not found at '{sourcePath}'.");
                return;
            }

            if (!File.Exists(targetPath))
            {
                Console.Error.WriteLine($"Error: Target file not found at '{targetPath}'.");
                return;
            }

            try
            {
                // Load source and target CAD files.
                using (Image srcImg = Image.Load(sourcePath))
                using (Image tgtImg = Image.Load(targetPath))
                {
                    CadImage srcCad = srcImg as CadImage;
                    CadImage tgtCad = tgtImg as CadImage;

                    if (srcCad == null || tgtCad == null)
                    {
                        Console.Error.WriteLine("Error: One of the files could not be loaded as a CAD image.");
                        return;
                    }

                    // Merge block entities from source into target.
                    foreach (DictionaryEntry entry in srcCad.BlockEntities)
                    {
                        string blockName = entry.Key as string;
                        CadBlockEntity srcBlock = entry.Value as CadBlockEntity;

                        if (string.IsNullOrEmpty(blockName) || srcBlock == null)
                            continue;

                        // If the target already contains a block with the same name, rename the source block.
                        string newBlockName = blockName;
                        int suffix = 1;
                        while (tgtCad.BlockEntities.Contains(newBlockName))
                        {
                            newBlockName = $"{blockName}_{suffix}";
                            suffix++;
                        }

                        // Add the block to the target CAD image.
                        tgtCad.BlockEntities.Add(newBlockName, srcBlock);
                    }

                    // Save the combined drawing to a temporary file.
                    string combinedPath = Path.Combine(
                        Path.GetDirectoryName(targetPath) ?? string.Empty,
                        "combined.dxf");

                    var dxfOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    tgtCad.Save(combinedPath, dxfOptions);

                    // Load the combined drawing to retrieve layout information.
                    using (Image combinedImg = Image.Load(combinedPath))
                    {
                        CadImage combinedCad = combinedImg as CadImage;
                        if (combinedCad == null)
                        {
                            Console.Error.WriteLine("Error: Combined file could not be loaded as a CAD image.");
                            return;
                        }

                        IEnumerable<CadLayout> layouts = combinedCad.Layouts.Values;
                        if (layouts == null)
                        {
                            Console.WriteLine("No layouts were found in the combined CAD file.");
                            return;
                        }

                        string outputDirectory = Path.GetDirectoryName(combinedPath) ?? string.Empty;
                        string outputPath = Path.Combine(outputDirectory, "LayoutSizes.txt");

                        using (StreamWriter writer = new StreamWriter(outputPath, false))
                        {
                            int index = 1;
                            foreach (CadLayout layout in layouts)
                            {
                                if (layout == null)
                                    continue;

                                string layoutName = layout.LayoutName ?? $"Layout_{index}";
                                double width = 0;
                                double height = 0;

                                if (layout.MinExtents != null && layout.MaxExtents != null)
                                {
                                    width = layout.MaxExtents.X - layout.MinExtents.X;
                                    height = layout.MaxExtents.Y - layout.MinExtents.Y;
                                }

                                string description = $"Layout #{index}: Name = '{layoutName}', {FormatSize(width, height)}";
                                Console.WriteLine(description);
                                writer.WriteLine(description);
                                index++;
                            }
                        }

                        Console.WriteLine($"Layout size information saved to: {outputPath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An exception occurred: {ex.Message}");
            }
        }
    }
}