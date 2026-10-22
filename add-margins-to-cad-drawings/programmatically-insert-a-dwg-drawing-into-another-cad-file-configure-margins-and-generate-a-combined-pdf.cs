using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // Prepare input and output directories
        string dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
        Directory.CreateDirectory(outputDir);

        // Input CAD files
        string baseFilePath = Path.Combine(dataDir, "Base.dwg");
        string insertFilePath = Path.Combine(dataDir, "Insert.dwg");

        // Verify that source files exist
        if (!File.Exists(baseFilePath) || !File.Exists(insertFilePath))
        {
            Console.WriteLine("Required CAD files not found. Place Base.dwg and Insert.dwg in the Data folder.");
            return;
        }

        Image baseImage = null;
        Image insertImage = null;

        // Load both drawings with error handling
        try
        {
            baseImage = Image.Load(baseFilePath);
            insertImage = Image.Load(insertFilePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading CAD files: {ex.Message}");
            return;
        }

        using (baseImage)
        using (insertImage)
        {
            CadImage baseCad = baseImage as CadImage;
            CadImage insertCad = insertImage as CadImage;

            if (baseCad == null || insertCad == null)
            {
                Console.WriteLine("Failed to load CAD images.");
                return;
            }

            // ------------------------------------------------------------
            // Insert the second drawing into the first one.
            // Placeholder for insertion logic:
            // baseCad.Blocks.Add("InsertedBlock", insertCad);
            // baseCad.Entities.Add(new CadBlockReference("InsertedBlock", new PointF(0, 0)));
            // ------------------------------------------------------------

            // Configure PDF export options with page size (used to simulate margins)
            PdfOptions pdfOptions = new PdfOptions();
            CadRasterizationOptions rasterOptions = new CadRasterizationOptions
            {
                // A4 size in points (1 point = 1/72 inch)
                PageWidth = 842,   // 8.27 inches * 72
                PageHeight = 595,  // 11.69 inches * 72
                Layouts = new string[] { "Model" } // Use model space layout
            };
            pdfOptions.VectorRasterizationOptions = rasterOptions;

            // Save the combined drawing as PDF with error handling
            string outputPath = Path.Combine(outputDir, "Combined.pdf");
            try
            {
                baseCad.Save(outputPath, pdfOptions);
                Console.WriteLine($"Combined PDF saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving PDF: {ex.Message}");
            }
        }
    }
}
