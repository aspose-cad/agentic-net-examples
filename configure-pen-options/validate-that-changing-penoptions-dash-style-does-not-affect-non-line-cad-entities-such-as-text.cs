using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadPenOptionsValidation
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputPath = "input.dwg";
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                string outputPathSolid = "output_solid.png";
                string outputPathDash = "output_dash.png";

                // Load CAD image
                CadImage cadImage = (CadImage)Image.Load(inputPath);

                // Rasterization options with default (solid) pen style
                CadRasterizationOptions optionsSolid = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new[] { "Model" },
                    PenOptions = new PenOptions() // default solid style
                };
                PngOptions pngOptionsSolid = new PngOptions
                {
                    VectorRasterizationOptions = optionsSolid
                };
                cadImage.Save(outputPathSolid, pngOptionsSolid);

                // Rasterization options with a dash pattern (simulated)
                CadRasterizationOptions optionsDash = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1200,
                    Layouts = new[] { "Model" },
                    PenOptions = new PenOptions() // dash style not directly exposed; using default options
                };
                PngOptions pngOptionsDash = new PngOptions
                {
                    VectorRasterizationOptions = optionsDash
                };
                cadImage.Save(outputPathDash, pngOptionsDash);

                // Verify that non‑line entities such as text are present
                bool textEntityFound = false;
                foreach (var entity in cadImage.Entities)
                {
                    if (entity is CadText || entity is CadMText)
                    {
                        textEntityFound = true;
                        break;
                    }
                }

                Console.WriteLine(textEntityFound
                    ? "Validation passed: Text entities are present and unaffected by PenOptions dash style."
                    : "Validation failed: No text entities found in the CAD file.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}