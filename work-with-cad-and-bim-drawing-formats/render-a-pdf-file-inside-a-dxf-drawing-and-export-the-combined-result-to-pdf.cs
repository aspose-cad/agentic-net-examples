using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;

namespace CadPdfEmbeddingExample
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Path to the source DXF drawing
                    string dxfPath = "input.dxf";

                    // Path to the PDF file that we want to render inside the DXF drawing
                    // (Note: Direct embedding of PDF content into a DXF is not supported by Aspose.CAD.
                    //  This example demonstrates loading the DXF and exporting it to PDF.)
                    string pdfOutputPath = "combined_output.pdf";

            
                    string outputDir = Path.GetDirectoryName(pdfOutputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }
        // Load the DXF drawing
                    using (Image dxfImage = Image.Load(dxfPath))
                    {
                        // Cast the loaded image to CadImage to access CAD-specific members if needed
                        CadImage cadImage = dxfImage as CadImage;
                        if (cadImage == null)
                        {
                            Console.WriteLine("Failed to load DXF as CadImage.");
                            return;
                        }

                        // (Optional) Here you could manipulate cadImage.Entities, Layouts, etc.
                        // For this example we keep the drawing unchanged.

                        // Prepare PDF export options
                        PdfOptions pdfOptions = new PdfOptions();

                        // Save the combined result (the original DXF drawing) as a PDF file
                        cadImage.Save(pdfOutputPath, pdfOptions);
                    }

                    Console.WriteLine("DXF drawing has been exported to PDF successfully.");
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}