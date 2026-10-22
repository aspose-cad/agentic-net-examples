using System;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadExportExample
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Path to the source CAD file (DWG/DXF)
                    string sourcePath = "input.dwg";

                    // Path for the exported PDF file
                    string pdfOutputPath = "output.pdf";

            
                    string outputDir = Path.GetDirectoryName(pdfOutputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }
        // Load the CAD file using Aspose.CAD
                    using (Image cadImageBase = Image.Load(sourcePath))
                    {
                        // Cast the loaded image to CadImage to access CAD-specific members
                        CadImage cadImage = (CadImage)cadImageBase;

                        // -------------------------------------------------
                        // Example: Inspect block definitions in the drawing
                        // -------------------------------------------------
                        foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                        {
                            Console.WriteLine($"Block Name: {block.Name}");
                            Console.WriteLine($"Number of Entities in Block: {block.Entities.Count}");

                            // List the types of entities contained in the block
                            foreach (CadEntityBase entity in block.Entities)
                            {
                                Console.WriteLine($"  Entity Type: {entity.GetType().Name}");
                            }

                            Console.WriteLine();
                        }

                        // -------------------------------------------------
                        // Export the CAD drawing to PDF
                        // -------------------------------------------------
                        PdfOptions pdfOptions = new PdfOptions();

                        // Save the CAD image as PDF using the matching options type
                        cadImage.Save(pdfOutputPath, pdfOptions);
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