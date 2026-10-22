using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace CompositeCadApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourcePath = "source.dxf";
            string basePath = "base.dxf";
            string outputDir = "output";

            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            if (!File.Exists(basePath))
            {
                Console.WriteLine($"Base file not found: {basePath}");
                return;
            }

            Directory.CreateDirectory(outputDir);

            try
            {
                using (CadImage sourceImage = (CadImage)Image.Load(sourcePath))
                using (CadImage baseImage = (CadImage)Image.Load(basePath))
                {
                    // Create a new block entity to hold the source drawing's entities
                    var insertedBlock = new CadBlockEntity();
                    insertedBlock.Name = "InsertedBlock";

                    // Add each entity from the source drawing into the block
                    foreach (var entity in sourceImage.Entities)
                    {
                        insertedBlock.AddEntity(entity);
                    }

                    // Add the block to the base drawing's block dictionary
                    baseImage.BlockEntities.Add(insertedBlock.Name, insertedBlock);

                    // Save the composite drawing as DXF, preserving editable objects
                    var dxfOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert,
                        Version = DxfOutputVersion.R12,
                        DxfFileFormat = CadFileFormat.Binary
                    };
                    string dxfOutputPath = Path.Combine(outputDir, "composite.dxf");
                    baseImage.Save(dxfOutputPath, dxfOptions);
                    Console.WriteLine($"Composite DXF saved to {dxfOutputPath}");

                    // Render the composite drawing to PDF for a visual composite view
                    var rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 800,
                        PageHeight = 600,
                        UnitType = UnitType.Point,
                        LineScale = 1.0f,
                        ObserverPoint = new ObserverPoint(30f, 30f, 0f)
                    };
                    var pdfOptions = new PdfOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };
                    string pdfOutputPath = Path.Combine(outputDir, "composite.pdf");
                    baseImage.Save(pdfOutputPath, pdfOptions);
                    Console.WriteLine($"Composite PDF saved to {pdfOutputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing CAD files: {ex.Message}");
            }
        }
    }
}