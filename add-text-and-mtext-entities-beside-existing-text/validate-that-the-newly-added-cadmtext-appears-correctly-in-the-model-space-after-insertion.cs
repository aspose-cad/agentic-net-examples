using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

namespace AddMTextExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input CAD file path
                string inputPath = "input.dwg";

                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Load the CAD file as a CadImage
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("Failed to load CAD image.");
                        return;
                    }

                    // Create a new multi‑line text (MText) entity using property initializers
                    CadMText mText = new CadMText
                    {
                        Text = "Added by Aspose.CAD",
                        InsertionPoint = new Cad3DPoint(10.0, 20.0, 0.0),
                        InitialTextHeight = 2.5,
                        RotationAngleRad = 0.0,
                        SoftOwner = "" // Placeholder; set appropriately if needed
                    };

                    // Locate the Model Space block ("*Model_Space")
                    if (cadImage.BlockEntities.TryGetValue("*Model_Space", out var modelSpaceObj) &&
                        modelSpaceObj is CadBlockEntity modelSpace)
                    {
                        // Add the MText entity to Model Space
                        modelSpace.AddEntity(mText);
                        Console.WriteLine("MText entity added to Model Space.");
                    }
                    else
                    {
                        Console.WriteLine("Model Space block not found.");
                        return;
                    }

                    // Optional: Save the modified drawing to PDF for visual verification
                    string outputPath = "output.pdf";
                    PdfOptions pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = new CadRasterizationOptions
                        {
                            PageWidth = 800,
                            PageHeight = 600,
                            DrawType = CadDrawTypeMode.UseObjectColor
                        }
                    };
                    cadImage.Save(outputPath, pdfOptions);
                    Console.WriteLine($"Modified drawing saved to {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}