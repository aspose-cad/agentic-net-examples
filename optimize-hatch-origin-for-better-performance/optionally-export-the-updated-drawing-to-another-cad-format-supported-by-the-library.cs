using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string pdfPath = "output.pdf";
        const string dxfPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image cadImage = Image.Load(inputPath))
            {
                CadImage cad = cadImage as CadImage;
                // Optional: modify the CAD model here using cad.BlockEntities, cad.Entities, etc.

                // Export to PDF
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(pdfPath, pdfOptions);
                Console.WriteLine($"PDF saved to: {pdfPath}");

                // Optional: export to another CAD format (DXF)
                DxfOptions dxfOptions = new DxfOptions
                {
                    // Preserve editable objects (example setting)
                    OutputMode = CadOutputMode.Convert
                };
                cadImage.Save(dxfPath, dxfOptions);
                Console.WriteLine($"DXF saved to: {dxfPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}