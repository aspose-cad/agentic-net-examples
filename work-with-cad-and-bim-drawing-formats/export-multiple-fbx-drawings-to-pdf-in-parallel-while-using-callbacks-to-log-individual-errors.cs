using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        // List of FBX files to export
        var inputFiles = new List<string>
        {
            "model1.fbx",
            "model2.fbx",
            // add additional file paths as needed
        };

        // Ensure the output directory exists
        string outputDir = "output";
        Directory.CreateDirectory(outputDir);

        // Process each file in parallel
        Parallel.ForEach(inputFiles, inputFile =>
        {
            if (!File.Exists(inputFile))
            {
                Console.Error.WriteLine($"File not found: {inputFile}");
                return;
            }

            string fileName = Path.GetFileNameWithoutExtension(inputFile);
            string outputPath = Path.Combine(outputDir, fileName + ".pdf");

            // Configure load options
            var loadOptions = new LoadOptions
            {
                IgnoreErrors = true
            };

            try
            {
                // Load the FBX drawing as a CadImage
                using (var cadImage = (CadImage)Image.Load(inputFile, loadOptions))
                {
                    // Log any errors that occurred during loading
                    foreach (var loadError in loadOptions.Errors)
                    {
                        Console.Error.WriteLine($"Load error in '{inputFile}': {loadError.RenderCode}: {loadError.Message}");
                    }

                    // Set up rasterization options with a render-result callback
                    var rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 1600,
                        PageHeight = 1200,
                        RenderResult = result =>
                        {
                            // Enumerate rendering failures (if any) and log them
                            foreach (var failure in result.Failures)
                            {
                                Console.Error.WriteLine($"Render failure for '{inputFile}': {failure.RenderCode}: {failure.Message}");
                            }
                        }
                    };

                    // Configure PDF export options
                    var pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    // Save the drawing as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }
            }
            catch (Exception ex)
            {
                // Capture any exception thrown during load or save
                Console.Error.WriteLine($"Exception processing '{inputFile}': {ex.Message}");
            }
        });
    }
}