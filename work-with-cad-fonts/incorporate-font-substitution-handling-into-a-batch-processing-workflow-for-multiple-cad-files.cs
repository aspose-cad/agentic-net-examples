using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD;
using System;
using System.IO;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

namespace CadBatchProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input and output directories
            string inputFolder = @"C:\CAD\Input";
            string outputFolder = @"C:\CAD\Output";

            // Ensure the output directory exists
            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Retrieve CAD files from the input folder
            string[] cadFiles;
            try
            {
                cadFiles = Directory.GetFiles(inputFolder, "*.dwg");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to enumerate CAD files: {ex.Message}");
                return;
            }

            foreach (string cadFilePath in cadFiles)
            {
                try
                {
                    // Load the CAD file
                    CadImage cadImage = (CadImage)Aspose.CAD.Image.Load(cadFilePath);

                    // Set font substitution (e.g., use Arial when the original font is missing)
                    cadImage.DefaultFont = "Arial";

                    // Prepare PDF conversion options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Determine PDF output path
                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(cadFilePath);
                    string pdfOutputPath = Path.Combine(outputFolder, fileNameWithoutExt + ".pdf");

                    // Save as PDF
                    cadImage.Save(pdfOutputPath, pdfOptions);

                    // Prepare DXF conversion options preserving editable objects
                    DxfOptions dxfOptions = new DxfOptions();
                    dxfOptions.OutputMode = CadOutputMode.Convert;
                    dxfOptions.Version = DxfOutputVersion.R12;
                    dxfOptions.DxfFileFormat = CadFileFormat.Ascii;

                    // Determine DXF output path
                    string dxfOutputPath = Path.Combine(outputFolder, fileNameWithoutExt + ".dxf");

                    // Save as DXF
                    cadImage.Save(dxfOutputPath, dxfOptions);

                    // Release resources
                    cadImage.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing file '{cadFilePath}': {ex.Message}");
                    // Continue with the next file
                }
            }

            Console.WriteLine("Batch processing completed.");
        }
    }
}