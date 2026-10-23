using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Dwf;
using Aspose.CAD.FileFormats.Dwf.Whip.Objects.Drawable.Text;

namespace AsposeCadRemoveElementExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dwf";
            string outputPath = "output.pdf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            try
            {
                // Load the image. For DWF the returned type is DwfImage.
                using (Image image = Image.Load(inputPath))
                {
                    DwfImage dwfImage = image as DwfImage;
                    if (dwfImage == null)
                    {
                        Console.Error.WriteLine("The loaded file is not a DWF image; removal of DwfWhipText is not supported for this format.");
                        return;
                    }

                    bool removed = false;

                    // Iterate through pages to find a DwfWhipText entity.
                    for (int pageIndex = 0; pageIndex < dwfImage.Pages.Length && !removed; pageIndex++)
                    {
                        var page = dwfImage.Pages[pageIndex];
                        var entities = page.Entities; // DwfWhipDrawable[]

                        for (int elementIndex = 0; elementIndex < entities.Length; elementIndex++)
                        {
                            if (entities[elementIndex] is DwfWhipText)
                            {
                                // Remove the element using the public API.
                                dwfImage.RemoveElement(pageIndex, elementIndex);
                                Console.WriteLine($"DwfWhipText entity removed from page {pageIndex}, element {elementIndex}.");
                                removed = true;
                                break;
                            }
                        }
                    }

                    if (!removed)
                    {
                        Console.WriteLine("No DwfWhipText entity found in the drawing.");
                    }

                    // Prepare PDF export options
                    var pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = new CadRasterizationOptions
                        {
                            PageWidth = 800,
                            PageHeight = 600,
                            BackgroundColor = Aspose.CAD.Color.White
                        }
                    };

                    // Save the (potentially modified) drawing
                    dwfImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"File saved successfully to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}