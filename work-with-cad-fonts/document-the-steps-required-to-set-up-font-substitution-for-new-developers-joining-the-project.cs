using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class FontSubstitutionSetup
{
    static void Main()
    {
        try
        {
            // 1. Define source CAD file and output destinations
            string sourcePath = "sample.dwg";
            string pdfOutputPath = "output/sample.pdf";
            string dxfOutputPath = "output/sample.dxf";

            // 2. Ensure output directories exist
            string pdfDir = Path.GetDirectoryName(pdfOutputPath);
            if (!string.IsNullOrEmpty(pdfDir) && !Directory.Exists(pdfDir))
                Directory.CreateDirectory(pdfDir);

            string dxfDir = Path.GetDirectoryName(dxfOutputPath);
            if (!string.IsNullOrEmpty(dxfDir) && !Directory.Exists(dxfDir))
                Directory.CreateDirectory(dxfDir);

            // 3. Verify that the source file exists
            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // 4. Load the CAD file
            using (var cadImage = (CadImage)Image.Load(sourcePath))
            {
                // 5. Set the default font for missing font references
                cadImage.DefaultFont = "Arial";

                // 6. (Optional) Define explicit font substitutions
                // cadImage.FontSubstitutes["MissingFontName"] = "SubstituteFontName";

                // 7. Prepare rasterization options for PDF conversion
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1100
                };

                // 8. Create PDF options and save
                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };
                cadImage.Save(pdfOutputPath, pdfOptions);

                // 9. Prepare DXF options with explicit binary format
                var dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Binary
                };
                cadImage.Save(dxfOutputPath, dxfOptions);
            }

            Console.WriteLine("Font substitution setup completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}