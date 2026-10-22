using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CombineDxfToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input DXF files to combine
            string[] inputFiles = { "drawing1.dxf", "drawing2.dxf", "drawing3.dxf" };
            // Output PDF file
            string outputPdf = "combined.pdf";

            // Verify that at least one input file exists
            var existingFiles = new List<string>();
            foreach (var file in inputFiles)
            {
                if (File.Exists(file))
                {
                    existingFiles.Add(file);
                }
                else
                {
                    Console.WriteLine($"Warning: Input file \"{file}\" not found and will be skipped.");
                }
            }

            if (existingFiles.Count == 0)
            {
                Console.WriteLine("No valid input files were found. Exiting.");
                return;
            }

            // Load each drawing and store the CadImage objects
            var images = new List<CadImage>();
            foreach (var file in existingFiles)
            {
                try
                {
                    var img = (CadImage)Image.Load(file);
                    images.Add(img);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading \"{file}\": {ex.Message}");
                }
            }

            if (images.Count == 0)
            {
                Console.WriteLine("No drawings could be loaded. Exiting.");
                return;
            }

            // Prepare PDF export options with unified page size (A4 landscape) and margins
            var pdfOptions = new PdfOptions();
            var rasterOptions = new CadRasterizationOptions
            {
                // A4 landscape dimensions in points (1 point = 1/72 inch)
                PageWidth = 842f,   // 11.69 inches
                PageHeight = 595f,  // 8.27 inches
                // Optional: set margins (in points)
                // Here we simply keep the default margins; adjust as needed
            };
            pdfOptions.VectorRasterizationOptions = rasterOptions;

            // Create a temporary folder to store individual PDF pages
            string tempFolder = Path.Combine(Path.GetTempPath(), "CadPdfPages_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempFolder);
            var pageFiles = new List<string>();

            // Export each drawing to a separate PDF page
            for (int i = 0; i < images.Count; i++)
            {
                string pagePath = Path.Combine(tempFolder, $"page_{i + 1}.pdf");
                images[i].Save(pagePath, pdfOptions);
                pageFiles.Add(pagePath);
                images[i].Dispose();
            }

            // Combine the individual PDF pages into a single PDF file
            // Simple concatenation of PDF byte streams (works for many PDF generators)
            using (var outputStream = new FileStream(outputPdf, FileMode.Create, FileAccess.Write))
            {
                foreach (var pageFile in pageFiles)
                {
                    byte[] pageBytes = File.ReadAllBytes(pageFile);
                    outputStream.Write(pageBytes, 0, pageBytes.Length);
                }
            }

            // Clean up temporary files
            try
            {
                Directory.Delete(tempFolder, true);
            }
            catch
            {
                // Ignore cleanup errors
            }

            Console.WriteLine($"Combined PDF saved to \"{outputPdf}\".");
        }
    }
}
