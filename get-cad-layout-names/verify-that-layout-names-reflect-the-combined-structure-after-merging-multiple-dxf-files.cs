using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace AsposeCadLayoutVerification
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder input DXF file paths
                string[] inputFiles = { "file1.dxf", "file2.dxf", "file3.dxf" };

                // Verify that all input files exist
                foreach (string path in inputFiles)
                {
                    if (!File.Exists(path))
                    {
                        Console.Error.WriteLine($"Input file not found: {path}");
                        return;
                    }
                }

                // Collect unique layout names from all DXF files
                var combinedLayouts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (string path in inputFiles)
                {
                    using (Image image = Image.Load(path))
                    {
                        // Cast to CadImage to access CAD-specific members
                        var cadImage = image as CadImage;
                        if (cadImage == null)
                        {
                            Console.Error.WriteLine($"Failed to cast loaded image to CadImage for file: {path}");
                            continue;
                        }

                        // Iterate through layouts in the current file
                        foreach (CadLayout layout in cadImage.Layouts.Values)
                        {
                            if (!string.IsNullOrEmpty(layout.LayoutName))
                            {
                                combinedLayouts.Add(layout.LayoutName);
                            }
                        }
                    }
                }

                // Output the combined layout names
                Console.WriteLine("Combined layout names after merging DXF files:");
                foreach (string layoutName in combinedLayouts)
                {
                    Console.WriteLine($"- {layoutName}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}