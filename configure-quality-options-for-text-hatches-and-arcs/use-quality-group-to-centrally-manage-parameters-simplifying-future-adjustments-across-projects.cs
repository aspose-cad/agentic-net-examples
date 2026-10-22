using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadQualityDemo
{
    // Centralised quality parameters for text, hatches and arcs.
    struct QualitySettings
    {
        public int TextQuality;   // 0‑100, higher = better quality
        public int HatchQuality;  // 0‑100, higher = better quality
        public int ArcQuality;    // 0‑100, higher = better quality

        public QualitySettings(int text, int hatch, int arc)
        {
            TextQuality = text;
            HatchQuality = hatch;
            ArcQuality = arc;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file – adjust as needed.
                string inputPath = Path.Combine(Environment.CurrentDirectory, "Resources", "sample.dwg");
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input CAD file not found: {inputPath}");
                    return;
                }

                // Prepare output directory.
                string outputDir = Path.Combine(Environment.CurrentDirectory, "Output");
                Directory.CreateDirectory(outputDir);
                string outputPath = Path.Combine(outputDir, "sample_quality.pdf");

                // Define desired quality levels.
                QualitySettings quality = new QualitySettings(text: 90, hatch: 80, arc: 95);

                // Load the CAD image.
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD‑specific features.
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    // Configure rasterization options – these control how vector entities are rendered.
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        // Set a reasonable page size (in pixels) for the rasterized output.
                        PageWidth = 2000,
                        PageHeight = 2000,
                        // Preserve the original drawing layout.
                        Layouts = new[] { "Model" },

                        // Map quality settings to available rasterization parameters.
                        // Here we use LineScale as an example; higher values can improve visual fidelity.
                        LineScale = quality.ArcQuality / 100f,

                        // Optional: choose drawing mode.
                        DrawType = CadDrawTypeMode.UseObjectColor
                    };

                    // Create PDF options and attach the rasterization settings.
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the result.
                    cadImage.Save(outputPath, pdfOptions);
                    Console.WriteLine($"CAD file saved with quality settings to: {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}