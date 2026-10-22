using System;
using System.IO;
using System.Diagnostics;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class CadRenderPerformance
{
    static void Main()
    {
        // Input CAD file path
        string inputPath = "input.dwg";

        // Output directory
        string outputDir = "output";

        // Validate input file
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Load CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Common rasterization settings
                CadRasterizationOptions baseRasterOptions = new CadRasterizationOptions();
                baseRasterOptions.UnitType = UnitType.Centimenter; // Correct enum spelling
                baseRasterOptions.LineScale = 0.5f;
                baseRasterOptions.PageWidth = 800;
                baseRasterOptions.PageHeight = 600;
                baseRasterOptions.Layouts = new[] { "Model" };

                // ---------- Render before origin adjustment ----------
                // Set margins (origin at left/top)
                Margins marginsBefore = new Margins
                {
                    Left = 100,
                    Right = 100,
                    Top = 0,
                    Bottom = 0
                };
                baseRasterOptions.Margins = marginsBefore;

                // PNG options for the first render
                PngOptions pngOptionsBefore = new PngOptions
                {
                    VectorRasterizationOptions = baseRasterOptions
                };

                string beforePath = Path.Combine(outputDir, "render_before.png");

                Stopwatch sw = new Stopwatch();
                sw.Start();
                cadImage.Save(beforePath, pngOptionsBefore);
                sw.Stop();

                Console.WriteLine($"Render before origin adjustment: {sw.ElapsedMilliseconds} ms");

                // ---------- Render after origin adjustment ----------
                // Clone base options and modify margins to shift origin
                CadRasterizationOptions rasterOptionsAfter = new CadRasterizationOptions
                {
                    UnitType = baseRasterOptions.UnitType,
                    LineScale = baseRasterOptions.LineScale,
                    PageWidth = baseRasterOptions.PageWidth,
                    PageHeight = baseRasterOptions.PageHeight,
                    Layouts = baseRasterOptions.Layouts
                };

                // New margins (origin shifted down/right)
                Margins marginsAfter = new Margins
                {
                    Left = 0,
                    Right = 0,
                    Top = 100,
                    Bottom = 100
                };
                rasterOptionsAfter.Margins = marginsAfter;

                // PNG options for the second render
                PngOptions pngOptionsAfter = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptionsAfter
                };

                string afterPath = Path.Combine(outputDir, "render_after.png");

                sw.Restart();
                cadImage.Save(afterPath, pngOptionsAfter);
                sw.Stop();

                Console.WriteLine($"Render after origin adjustment: {sw.ElapsedMilliseconds} ms");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}