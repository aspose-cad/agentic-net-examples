using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dxf";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the DXF file
            using (Image cadImage = Image.Load(inputPath))
            {
                // Cast to CadImage to access layout collection
                CadImage cad = (CadImage)cadImage;

                // Attempt to select a specific layout (e.g., the first one)
                CadLayout specificLayout = cad.Layouts.Values.FirstOrDefault();

                // NOTE: Aspose.CAD PdfOptions does not provide a LayoutName property.
                // Therefore, exporting a single layout directly to PDF is not supported.
                // The entire drawing will be exported.

                PdfOptions pdfOptions = new PdfOptions();

                // Save the drawing (or its first layout) to PDF using default rasterization options
                cad.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"PDF saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}