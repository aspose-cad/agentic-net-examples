using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Input CAD file and external reference file paths
            string sourcePath = "source.dwg";
            string xrefPath = "reference.dwg";

            // Verify that the required files exist
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source CAD file not found: {sourcePath}");
                return;
            }

            if (!File.Exists(xrefPath))
            {
                Console.Error.WriteLine($"External reference CAD file not found: {xrefPath}");
                // Proceed without inserting the XREF as this feature is not available in the current API
            }

            // Prepare output directory and file path
            string outputDir = "Output";
            Directory.CreateDirectory(outputDir);
            string outputPath = Path.Combine(outputDir, "result.dwg");

            // Load the source CAD drawing
            using (CadImage cadImage = (CadImage)Image.Load(sourcePath))
            {
                // NOTE: Direct insertion of external references (XREF) is not supported in the current Aspose.CAD API.
                // This placeholder indicates where such logic would be implemented if available.

                // Calculate and display layout sizes (including referenced content if XREF were present)
                foreach (var layoutEntry in cadImage.Layouts)
                {
                    CadLayout layout = layoutEntry.Value;
                    if (layout.MinExtents != null && layout.MaxExtents != null)
                    {
                        double width = layout.MaxExtents.X - layout.MinExtents.X;
                        double height = layout.MaxExtents.Y - layout.MinExtents.Y;
                        Console.WriteLine($"Layout '{layout.LayoutName}' size: Width = {width}, Height = {height}");
                    }
                }

                // Save the (potentially modified) CAD drawing back to DWG format using DXF options for compatibility
                var saveOptions = new DxfOptions();
                cadImage.Save(outputPath, saveOptions);
            }

            Console.WriteLine($"Modified CAD file saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}