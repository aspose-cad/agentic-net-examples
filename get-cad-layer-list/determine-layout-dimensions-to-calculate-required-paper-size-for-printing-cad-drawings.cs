using Aspose.CAD;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Validate arguments
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: Program <input-cad-file> <output-directory>");
            return;
        }

        string inputPath = args[0];
        string outputDir = args[1];

        if (!File.Exists(inputPath))
        {
            Console.WriteLine("Input file does not exist: " + inputPath);
            return;
        }

        if (!Directory.Exists(outputDir))
        {
            Console.WriteLine("Output directory does not exist: " + outputDir);
            return;
        }

        try
        {
            // Load CAD image
            CadImage cadImage = (CadImage)Aspose.CAD.Image.Load(inputPath);

            // Iterate through all layouts and calculate dimensions
            foreach (CadLayout layout in cadImage.Layouts.Values)
            {
                double width = layout.MaxExtents.X - layout.MinExtents.X;
                double height = layout.MaxExtents.Y - layout.MinExtents.Y;

                Console.WriteLine("Layout \"" + layout.LayoutName + "\" dimensions:");
                Console.WriteLine("  Width : " + width.ToString("F2") + " units");
                Console.WriteLine("  Height: " + height.ToString("F2") + " units");
            }

            // Export the first layout to PDF using its calculated size (if any layout exists)
            if (cadImage.Layouts.Count > 0)
            {
                IEnumerator<CadLayout> enumerator = cadImage.Layouts.Values.GetEnumerator();
                enumerator.MoveNext();
                CadLayout firstLayout = enumerator.Current;

                double pageWidth = firstLayout.MaxExtents.X - firstLayout.MinExtents.X;
                double pageHeight = firstLayout.MaxExtents.Y - firstLayout.MinExtents.Y;

                // Configure rasterization options
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                rasterOptions.UnitType = UnitType.Centimenter;
                rasterOptions.PageWidth = (float)pageWidth;
                rasterOptions.PageHeight = (float)pageHeight;
                rasterOptions.Layouts = new string[] { firstLayout.LayoutName };
                rasterOptions.Margins = new Margins() { Left = 10, Right = 10, Top = 10, Bottom = 10 };

                // Configure PDF export options
                PdfOptions pdfOptions = new PdfOptions();
                pdfOptions.VectorRasterizationOptions = rasterOptions;

                string outputPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(inputPath) + ".pdf");
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine("PDF saved to: " + outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
}