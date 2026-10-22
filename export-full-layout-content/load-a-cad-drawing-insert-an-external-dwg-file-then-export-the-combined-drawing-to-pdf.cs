using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

namespace CadInsertAndExport
{
    class Program
    {
        static void Main(string[] args)
        {
            // Paths to the main CAD drawing, the external DWG to insert, and the output PDF.
            string mainDrawingPath = "main.dwg";
            string externalDrawingPath = "external.dwg";
            string outputPdfPath = "combined.pdf";

            // Verify that the input files exist.
            if (!File.Exists(mainDrawingPath))
            {
                Console.Error.WriteLine($"Main drawing file not found: {mainDrawingPath}");
                return;
            }

            if (!File.Exists(externalDrawingPath))
            {
                Console.Error.WriteLine($"External drawing file not found: {externalDrawingPath}");
                return;
            }

            try
            {
                // Load the main CAD drawing.
                using (CadImage mainImage = (CadImage)Image.Load(mainDrawingPath))
                {
                    // Load the external DWG file that will be inserted.
                    using (CadImage externalImage = (CadImage)Image.Load(externalDrawingPath))
                    {
                        // Create a new block entity to hold the external drawing's entities.
                        CadBlockEntity externalBlock = new CadBlockEntity
                        {
                            BasePoint = new Cad3DPoint(0.0, 0.0, 0.0)
                        };

                        // Add each entity from the external drawing into the new block.
                        foreach (CadEntityBase entity in externalImage.Entities)
                        {
                            externalBlock.AddEntity(entity);
                        }

                        // Insert the new block into the main drawing's block collection.
                        // The key "ExternalBlock" is the name of the block.
                        mainImage.BlockEntities.Add("ExternalBlock", externalBlock);
                    }

                    // Prepare PDF save options.
                    PdfOptions pdfOptions = new PdfOptions();

                    // Export the combined drawing to PDF.
                    mainImage.Save(outputPdfPath, pdfOptions);
                }

                Console.WriteLine($"Combined PDF saved to: {outputPdfPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}