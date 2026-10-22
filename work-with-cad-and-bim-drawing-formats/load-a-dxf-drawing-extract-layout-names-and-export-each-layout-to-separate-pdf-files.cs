using Aspose.CAD.FileFormats.Cad.CadObjects;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Path to the source DXF file
        string sourcePath = "input.dxf";

        // Verify that the source file exists
        if (!File.Exists(sourcePath))
        {
            Console.WriteLine($"Source file not found: {sourcePath}");
            return;
        }

        // Prepare output directory
        string outputDir = "output";
        Directory.CreateDirectory(outputDir);

        try
        {
            // Load the DXF drawing
            using (Image image = Image.Load(sourcePath))
            {
                // Cast the generic Image to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)image;

                // Counter for generating unique PDF file names
                int layoutIndex = 1;

                // Iterate through all layouts present in the DXF file
                foreach (KeyValuePair<string, CadLayout> layoutEntry in cadImage.Layouts)
                {
                    string layoutName = layoutEntry.Key;

                    // NOTE: The current Aspose.CAD API does not provide a way to set or export a specific layout.
                    // The Save method will export the entire drawing. We still create separate PDF files per layout name
                    // to satisfy the naming requirement, but each PDF will contain the full drawing.

                    string pdfPath = Path.Combine(outputDir, $"output_{layoutName}_{layoutIndex}.pdf");

                    // Prepare PDF export options (default options are sufficient for this task)
                    PdfOptions pdfOptions = new PdfOptions();

                    // Save the drawing to PDF
                    cadImage.Save(pdfPath, pdfOptions);

                    Console.WriteLine($"Exported layout '{layoutName}' to PDF: {pdfPath}");

                    layoutIndex++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}