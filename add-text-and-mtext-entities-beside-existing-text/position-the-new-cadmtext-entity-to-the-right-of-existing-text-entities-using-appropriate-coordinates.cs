using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

namespace CadTextAddition
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source DWG/DXF file
                string sourcePath = "input.dwg";
                // Path for the output PDF file
                string outputPath = "output.pdf";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Load the CAD drawing
                using (Image image = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadImage = (CadImage)image;

                    // Determine the rightmost X coordinate among existing MText entities
                    double maxX = 0.0;
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (entity is CadMText mtext && mtext.InsertionPoint != null)
                        {
                            if (mtext.InsertionPoint.X > maxX)
                                maxX = mtext.InsertionPoint.X;
                        }
                    }

                    // Define a small offset to place the new text to the right of existing ones
                    double offset = 10.0;

                    // Create a new CadMText entity
                    CadMText newMText = new CadMText
                    {
                        Text = "Added MText",
                        InsertionPoint = new Cad3DPoint(maxX + offset, 0.0, 0.0),
                        InitialTextHeight = 2.5
                    };

                    // Add the new MText entity to the Model Space block
                    if (cadImage.BlockEntities.TryGetValue("*Model_Space", out CadBlockEntity modelSpace))
                    {
                        modelSpace.AddEntity(newMText);
                    }
                    else
                    {
                        Console.Error.WriteLine("Model space block not found.");
                        return;
                    }

                    // Prepare PDF save options
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = new CadRasterizationOptions
                        {
                            PageWidth = 1600,
                            PageHeight = 1200
                            // DrawType defaults to Vector; no explicit enum needed
                        }
                    };

                    // Save the modified drawing as PDF
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine("CAD drawing processed and saved to PDF.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}