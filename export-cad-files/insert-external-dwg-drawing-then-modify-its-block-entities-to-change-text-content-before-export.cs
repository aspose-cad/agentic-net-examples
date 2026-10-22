using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadBlockTextModifier
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input DWG path – either first argument or default.
            string inputPath = args.Length > 0 ? args[0] : "input.dwg";

            // Verify source file existence.
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Source file not found: {inputPath}");
                return;
            }

            // Prepare output folder.
            string outputFolder = Path.Combine(Path.GetDirectoryName(inputPath) ?? "", "output");
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Define output file (PDF format).
            string outputPath = Path.Combine(outputFolder, "modified.pdf");

            try
            {
                // Load the DWG drawing.
                Image image = Image.Load(inputPath);
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Loaded file is not a CAD image.");
                    return;
                }

                // Iterate through all block definitions.
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    // Enumerate entities inside the block.
                    foreach (var entity in block.Entities)
                    {
                        // Modify MText entities.
                        if (entity is CadMText mtext)
                        {
                            mtext.Text = "Modified Text";
                        }
                        // Modify simple Text entities (if present).
                        else if (entity is CadText simpleText)
                        {
                            simpleText.DefaultValue = "Modified Text";
                        }
                    }
                }

                // Set rasterization options for PDF export.
                CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 1600,
                    PageHeight = 1100,
                    Layouts = new[] { "ModelSpace" } // Use layout name string(s)
                };

                // Configure PDF save options.
                PdfOptions pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                // Save the modified drawing.
                cadImage.Save(outputPath, pdfOptions);

                Console.WriteLine($"Modified drawing saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}