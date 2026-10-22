using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
                // Path to the source CAD file
                string inputPath = "input.dwg";

        
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        // Path for the output file (preserving original format)
                string outputPath = "output.dwg";

                // Load the CAD drawing
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Example modification (optional): change background color
                    // cadImage.BackgroundColor = System.Drawing.Color.LightGray;

                    // Determine appropriate save options based on the output extension
                    string extension = Path.GetExtension(outputPath).ToLowerInvariant();

                    ImageOptionsBase saveOptions = null;
                    switch (extension)
                    {
                        case ".pdf":
                            saveOptions = new PdfOptions();
                            break;
                        case ".svg":
                            saveOptions = new SvgOptions();
                            break;
                        case ".png":
                            saveOptions = new PngOptions();
                            break;
                        case ".jpg":
                        case ".jpeg":
                            saveOptions = new JpegOptions();
                            break;
                        case ".bmp":
                            saveOptions = new BmpOptions();
                            break;
                        case ".tif":
                        case ".tiff":
                            saveOptions = new TiffOptions();
                            break;
                        case ".dxf":
                            saveOptions = new DxfOptions();
                            break;
                        // Add other format-specific options if needed
                        default:
                            // For formats without specific options, keep saveOptions as null
                            break;
                    }

                    // Save the modified drawing, preserving the original file format
                    if (saveOptions != null)
                        cadImage.Save(outputPath, saveOptions);
                    else
                        cadImage.Save(outputPath);
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}