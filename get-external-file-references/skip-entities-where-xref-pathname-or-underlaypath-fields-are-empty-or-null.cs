using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class CadEntityFilter
{
    static void Main()
    {
        string inputPath = "input.dwg";
        string outputPath = "output.pdf";

        // Ensure input file exists (placeholder if missing)
        if (!File.Exists(inputPath))
        {
            File.WriteAllBytes(inputPath, new byte[0]);
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load the CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Filter entities: skip those where XrefPathName or UnderlayPath are empty or whitespace
                var filteredEntities = cadImage.Entities
                    .Where(e =>
                    {
                        string xref = null;
                        string underlay = null;

                        try
                        {
                            xref = (string)((dynamic)e).XrefPathName;
                        }
                        catch { }

                        try
                        {
                            underlay = (string)((dynamic)e).UnderlayPath;
                        }
                        catch { }

                        bool xrefInvalid = xref != null && string.IsNullOrWhiteSpace(xref);
                        bool underlayInvalid = underlay != null && string.IsNullOrWhiteSpace(underlay);

                        return !(xrefInvalid || underlayInvalid);
                    })
                    .ToArray();

                // Replace the entities collection with the filtered set
                cadImage.Entities = filteredEntities;

                // Prepare rasterization options for PDF output
                var rasterOptions = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,
                    PageWidth = 800,
                    PageHeight = 600,
                    Layouts = new[] { cadImage.Layouts.Keys.FirstOrDefault() ?? "Model" }
                };

                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the filtered CAD image as PDF
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}