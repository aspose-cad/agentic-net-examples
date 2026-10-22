using Aspose.CAD.FileFormats.Cad.CadConsts;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.ImageOptions;

namespace AsposeCadEntityListExample
{
    class Program
    {
        static void Main(string[] args)
        {
            const string inputPath = "sample.dwg";
            const string outputPath = "output.pdf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the existing DWG/DXF file
                using (Image image = Image.Load(inputPath))
                {
                    // Cast to CadImage to work with CAD entities
                    CadImage cadImage = (CadImage)image;

                    // Prepare a custom list of CAD entities
                    List<CadEntityBase> customEntities = new List<CadEntityBase>();

                    // Create a line entity
                    CadLine line = new CadLine(
                        new Cad3DPoint(0, 0, 0),
                        new Cad3DPoint(100, 100, 0));
                    customEntities.Add(line);

                    // Create a lightweight polyline entity
                    CadLwPolyline polyline = new CadLwPolyline
                    {
                        Coordinates = new List<Cad2DPoint>
                        {
                            new Cad2DPoint(150, 150),
                            new Cad2DPoint(200, 150),
                            new Cad2DPoint(200, 200),
                            new Cad2DPoint(150, 200)
                        },
                        Flag = CadLwPolylineFlag.Closed
                    };
                    customEntities.Add(polyline);

                    // Assign the custom entity list to the CAD image
                    cadImage.Entities = customEntities;

                    // Save the modified drawing as PDF
                    PdfOptions pdfOptions = new PdfOptions();
                    cadImage.Save(outputPath, pdfOptions);
                }

                Console.WriteLine($"Successfully saved PDF to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}