using System;
using System.Diagnostics;
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
            // Define input and output file paths
            string inputPath = "input.dwg";
            string outputWithoutSubstitution = "output_without_substitution.png";
            string outputWithSubstitution = "output_with_substitution.png";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file as a CadImage
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Prepare PNG export options
                PngOptions pngOptions = new PngOptions();

                // ------------------------------
                // Export without font substitution
                // ------------------------------
                // Ensure no default font is set
                cadImage.DefaultFont = null;

                Stopwatch stopwatchWithout = Stopwatch.StartNew();
                cadImage.Save(outputWithoutSubstitution, pngOptions);
                stopwatchWithout.Stop();

                // ------------------------------
                // Export with font substitution
                // ------------------------------
                // Set a default font (e.g., Arial) to enable substitution
                cadImage.DefaultFont = "Arial";

                Stopwatch stopwatchWith = Stopwatch.StartNew();
                cadImage.Save(outputWithSubstitution, pngOptions);
                stopwatchWith.Stop();

                // Output performance results
                Console.WriteLine($"Export without font substitution: {stopwatchWithout.ElapsedMilliseconds} ms");
                Console.WriteLine($"Export with font substitution: {stopwatchWith.ElapsedMilliseconds} ms");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}