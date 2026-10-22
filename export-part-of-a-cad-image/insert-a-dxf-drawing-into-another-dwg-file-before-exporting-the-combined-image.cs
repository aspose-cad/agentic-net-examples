using Aspose.CAD.FileFormats.Cad.CadObjects;
using System.Linq;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
                // Paths to the source DXF and the target DWG files
                string dxfPath = "input.dxf";
                string dwgPath = "base.dwg";

                // Verify that source files exist
                if (!File.Exists(dxfPath) || !File.Exists(dwgPath))
                {
                    Console.WriteLine("Required CAD files not found. Ensure 'input.dxf' and 'base.dwg' exist.");
                    return;
                }

                // Load the DXF drawing and the DWG drawing
                using (CadImage dxfImage = (CadImage)Image.Load(dxfPath))
                using (CadImage dwgImage = (CadImage)Image.Load(dwgPath))
                {
                    // Insert each entity from the DXF into the DWG
                    foreach (CadEntityBase entity in dxfImage.Entities)
                    {
                        dwgImage.Entities = dwgImage.Entities.Concat(new CadEntityBase[] { entity }).ToArray();
                    }

                    // Prepare output directory
                    string outputDir = "Output";
                    Directory.CreateDirectory(outputDir);

                    // Export the combined drawing to PDF
                    string outputPath = Path.Combine(outputDir, "combined.pdf");
                    PdfOptions pdfOptions = new PdfOptions();
                    dwgImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine("Combined drawing saved successfully.");
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}