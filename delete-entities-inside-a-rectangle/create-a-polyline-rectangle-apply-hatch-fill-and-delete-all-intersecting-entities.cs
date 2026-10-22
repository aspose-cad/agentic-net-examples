using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main(string[] args)
    {
        // Define input and output file paths
        string inputPath = "input.dwg";
        string outputPath = "output.pdf";

        // Validate input file existence
        if (string.IsNullOrEmpty(inputPath) || !File.Exists(inputPath))
        {
            Console.Error.WriteLine("Error: Input CAD file not found at path: " + inputPath);
            return;
        }

        // Ensure output directory exists
        string outputDirectory = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
        {
            try
            {
                Directory.CreateDirectory(outputDirectory);
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine("Error: Unable to create output directory. " + dirEx.Message);
                return;
            }
        }

        try
        {
            // Load the CAD file
            using (Image image = Image.Load(inputPath))
            {
                // Cast to CadImage for CAD‑specific operations
                CadImage cadImage = (CadImage)image;

                // Example: iterate over top‑level entities (optional demonstration)
                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    // Simple type check example (no modifications performed)
                    if (entity is CadLine line)
                    {
                        // Access line points if needed
                        Cad3DPoint start = line.FirstPoint;
                        Cad3DPoint end = line.SecondPoint;
                        // Placeholder for processing logic
                    }
                }

                // Save the CAD image to PDF format
                PdfOptions pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
            }

            Console.WriteLine("Conversion completed successfully. Output saved to: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error during processing: " + ex.Message);
        }
    }
}