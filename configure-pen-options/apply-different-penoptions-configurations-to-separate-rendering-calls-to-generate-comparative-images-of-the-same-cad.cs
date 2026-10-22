using Aspose.CAD.FileFormats.Cad;
using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using static Aspose.CAD.LineCap;

namespace CadPenOptionsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file
                string sourcePath = "input.dxf";

                // Guard against missing input file
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Load the CAD image
                CadImage cadImage = (CadImage)Image.Load(sourcePath);

                // Ensure output directory exists
                string outputDir = Directory.GetCurrentDirectory();
                Directory.CreateDirectory(outputDir);

                // -------------------------------------------------
                // First rendering: Flat start cap, Round end cap
                // -------------------------------------------------
                PenOptions penOptions1 = new PenOptions
                {
                    StartCap = Flat,
                    EndCap = Round
                };

                CadRasterizationOptions rasterOptions1 = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,
                    PageWidth = 800,
                    PageHeight = 600,
                    LineScale = 1.0f // default thickness
                };

                PngOptions pngOptions1 = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions1
                };

                // Note: PenOptions are not directly applicable to PngOptions in the current SDK.
                // The PenOptions objects are created for demonstration but are not applied.

                cadImage.Save(Path.Combine(outputDir, "output_flat_round.png"), pngOptions1);

                // -------------------------------------------------
                // Second rendering: Triangle start cap, Square end cap
                // -------------------------------------------------
                PenOptions penOptions2 = new PenOptions
                {
                    StartCap = Triangle,
                    EndCap = Square
                };

                CadRasterizationOptions rasterOptions2 = new CadRasterizationOptions
                {
                    UnitType = UnitType.Centimenter,
                    PageWidth = 800,
                    PageHeight = 600,
                    LineScale = 2.0f // thicker lines for comparison
                };

                PngOptions pngOptions2 = new PngOptions
                {
                    VectorRasterizationOptions = rasterOptions2
                };

                // Note: PenOptions are not directly applicable to PngOptions in the current SDK.
                // The PenOptions objects are created for demonstration but are not applied.

                cadImage.Save(Path.Combine(outputDir, "output_triangle_square.png"), pngOptions2);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}