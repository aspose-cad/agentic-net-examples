using System;
using System.IO;
using System.Threading;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    private static Timer _timer;
    private const string InputPath = "input.dwg";
    private const string OutputDirectory = "output";
    private static int _renderCount = 0;

    static void Main()
    {
        if (!File.Exists(InputPath))
        {
            Console.WriteLine($"Input file not found: {InputPath}");
            return;
        }

        if (!Directory.Exists(OutputDirectory))
        {
            Directory.CreateDirectory(OutputDirectory);
        }

        _timer = new Timer(RenderCad, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

        Console.WriteLine("Press Enter to stop periodic rendering...");
        Console.ReadLine();

        _timer.Dispose();
    }

    private static void RenderCad(object state)
    {
        try
        {
            using (Image cadImage = Image.Load(InputPath))
            {
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,
                    PageHeight = 600
                    // PenOptions can be configured here if needed; omitted due to API constraints
                };

                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                string outputPath = Path.Combine(
                    OutputDirectory,
                    $"render_{DateTime.Now:yyyyMMdd_HHmmss}_{_renderCount}.pdf");

                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine($"Render #{_renderCount} saved.");
            _renderCount++;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Rendering error: {ex.Message}");
        }
    }
}