using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Input and output paths
        string inputPath = "input.dwg";
        string outputDir = "output";
        string outputPath = Path.Combine(outputDir, "output.pdf");

        // Verify input file exists
        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        // Ensure output directory exists
        Directory.CreateDirectory(outputDir);

        try
        {
            // Load CAD image
            using (Image cadImage = Image.Load(inputPath))
            {
                // Retrieve hyperlink from CAD (placeholder implementation)
                string hyperlink = GetHyperlinkFromCad(cadImage);

                // Validate hyperlink format before saving
                if (!IsValidUrl(hyperlink))
                {
                    Console.WriteLine("Invalid hyperlink format. Save operation aborted.");
                    return;
                }

                // Configure rasterization options (UnitType omitted due to API limitation)
                var rasterizationOptions = new CadRasterizationOptions
                {
                    // Additional options can be set here if needed
                };

                // Set up PDF export options
                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterizationOptions
                };

                // Save the CAD file as PDF
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"File successfully saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during processing: {ex.Message}");
        }
    }

    // Placeholder method to extract a hyperlink from the CAD image.
    // In a real scenario, this would inspect CAD entities or metadata.
    static string GetHyperlinkFromCad(Image cadImage)
    {
        // Example static hyperlink; replace with actual extraction logic.
        return "https://example.com";
    }

    // Validates that a URL is absolute and uses HTTP or HTTPS scheme.
    static bool IsValidUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) &&
               (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}