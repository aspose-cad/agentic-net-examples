using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadToPdfExport
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Folder containing the source AutoCAD drawings
                    string inputFolder = @"C:\CadFiles\Input";
                    // Folder where PDF files will be saved
                    string outputFolder = @"C:\CadFiles\Output";

                    // Ensure output directory exists
                    if (!Directory.Exists(outputFolder))
                    {
                        Directory.CreateDirectory(outputFolder);
                    }

                    // Get all DWG files in the input folder (add other extensions if needed)
                    string[] cadFiles = Directory.GetFiles(inputFolder, "*.dwg", SearchOption.TopDirectoryOnly);

                    foreach (string cadFilePath in cadFiles)
                    {
                        // Load the CAD drawing using Aspose.CAD
                        using (Image cadImage = Image.Load(cadFilePath))
                        {
                            // Prepare PDF export options
                            PdfOptions pdfOptions = new PdfOptions();

                            // Determine output PDF file path
                            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(cadFilePath);
                            string pdfFilePath = Path.Combine(outputFolder, fileNameWithoutExt + ".pdf");

                            // Save the drawing as PDF
                            cadImage.Save(pdfFilePath, pdfOptions);
                        }
                    }

                    Console.WriteLine("Export completed.");
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}