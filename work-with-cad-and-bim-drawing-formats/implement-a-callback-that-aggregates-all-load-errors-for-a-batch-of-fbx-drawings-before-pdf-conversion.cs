using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class BatchFbxToPdfConverter
{
    static void Main()
    {
        // Folder containing FBX drawings.
        string inputFolder = @"C:\CadDrawings\Fbx";
        // Output folder for generated PDFs.
        string outputFolder = @"C:\CadDrawings\Pdf";

        // Validate input folder.
        if (!Directory.Exists(inputFolder))
        {
            Console.WriteLine($"Input folder does not exist: {inputFolder}");
            return;
        }

        // Ensure output directory exists.
        Directory.CreateDirectory(outputFolder);

        // Collect all FBX files in the input folder.
        string[] drawingPaths = Directory.GetFiles(inputFolder, "*.fbx", SearchOption.TopDirectoryOnly);

        // List to hold aggregated error messages.
        List<string> aggregatedErrors = new List<string>();

        // Process each drawing.
        foreach (string drawingPath in drawingPaths)
        {
            // Guard against missing files.
            if (!File.Exists(drawingPath))
            {
                aggregatedErrors.Add($"{Path.GetFileName(drawingPath)}: File not found.");
                continue;
            }

            // Prepare load options: ignore errors so the image loads even if some entities are problematic.
            var loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            // Load the drawing.
            Image cadImage = null;
            try
            {
                cadImage = Image.Load(drawingPath, loadOptions);
            }
            catch (Exception ex)
            {
                // Capture the exception as an error.
                aggregatedErrors.Add($"{Path.GetFileName(drawingPath)}: Exception during load – {ex.Message}");
                continue; // Skip conversion for this file.
            }

            // After loading, collect any non‑fatal errors reported by the SDK.
            foreach (var err in loadOptions.Errors)
            {
                // RenderResult provides RenderCode (Message may not be available in this SDK version).
                aggregatedErrors.Add($"{Path.GetFileName(drawingPath)}: Load error – {err.RenderCode}");
            }

            // Prepare rasterization options for PDF conversion.
            var rasterOptions = new CadRasterizationOptions
            {
                PageWidth = 800,
                PageHeight = 600,
                UnitType = UnitType.Centimenter,
                LineScale = 0.5f,
                AutomaticLayoutsScaling = true
            };

            // Prepare PDF export options and attach rasterization settings.
            var pdfOptions = new PdfOptions
            {
                VectorRasterizationOptions = rasterOptions
            };

            // Determine output PDF file name.
            string pdfFileName = Path.GetFileNameWithoutExtension(drawingPath) + ".pdf";
            string pdfPath = Path.Combine(outputFolder, pdfFileName);

            // Save the drawing as PDF.
            try
            {
                cadImage.Save(pdfPath, pdfOptions);
            }
            catch (Exception ex)
            {
                // Capture any errors that occur during the conversion.
                aggregatedErrors.Add($"{Path.GetFileName(drawingPath)}: Exception during PDF conversion – {ex.Message}");
            }
            finally
            {
                // Release resources.
                cadImage?.Dispose();
            }
        }

        // Output aggregated errors (could be logged, displayed, or written to a file).
        if (aggregatedErrors.Count > 0)
        {
            Console.WriteLine("Aggregated load/conversion errors:");
            foreach (var errMsg in aggregatedErrors)
            {
                Console.WriteLine(errMsg);
            }
        }
        else
        {
            Console.WriteLine("All drawings processed successfully without errors.");
        }
    }
}