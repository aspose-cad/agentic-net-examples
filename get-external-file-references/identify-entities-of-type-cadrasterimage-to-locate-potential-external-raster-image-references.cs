using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadRasterExample
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Paths to input CAD file, raster image to insert, and output PDF
                    string cadInputPath = "input.dwg";
                    string rasterImagePath = "newImage.png";
                    string pdfOutputPath = "output.pdf";

            
                    string outputDir = Path.GetDirectoryName(pdfOutputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }
        // Load the CAD drawing
                    using (Image cadImageBase = Image.Load(cadInputPath))
                    {
                        CadImage cadImage = (CadImage)cadImageBase;

                        // List existing raster image definitions
                        foreach (CadBaseObject obj in cadImage.Objects)
                        {
                            if (obj is CadRasterImageDef rasterDef)
                            {
                                Console.WriteLine($"Raster Def Handle: {rasterDef.ObjectHandle}, File: {rasterDef.FileName}, SizeU: {rasterDef.ImageSizeU}, SizeV: {rasterDef.ImageSizeV}");
                            }
                        }

                        // Load the external raster image to obtain its dimensions
                        using (Image rasterImg = Image.Load(rasterImagePath))
                        {
                            double width = rasterImg.Width;
                            double height = rasterImg.Height;

                            // Create a new raster image definition
                            CadRasterImageDef newDef = new CadRasterImageDef();
                            newDef.FileName = Path.GetFileName(rasterImagePath);
                            newDef.ImageSizeU = width;
                            newDef.ImageSizeV = height;
                            // Generate a simple unique object handle
                            newDef.ObjectHandle = "0x" + Guid.NewGuid().ToString("N").Substring(0, 8);

                            // Append the new definition to the Objects collection
                            cadImage.Objects = cadImage.Objects.Concat(new CadBaseObject[] { newDef }).ToArray();

                            // Define insertion point and orientation vectors
                            Cad3DPoint insertionPoint = new Cad3DPoint(0, 0, 0);
                            Cad3DPoint uVector = new Cad3DPoint(width, 0, 0);
                            Cad3DPoint vVector = new Cad3DPoint(0, height, 0);

                            // Create the raster image entity referencing the definition
                            CadRasterImage rasterEntity = new CadRasterImage(newDef, insertionPoint, uVector, vVector);
                            rasterEntity.ImageDefReference = newDef.ObjectHandle;

                            // Append the new entity to the Entities collection
                            cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { rasterEntity }).ToArray();
                        }

                        // Save the modified drawing as PDF
                        PdfOptions pdfOptions = new PdfOptions();
                        using (FileStream outStream = new FileStream(pdfOutputPath, FileMode.Create))
                        {
                            cadImage.Save(outStream, pdfOptions);
                        }
                    }

                    Console.WriteLine("CAD processing completed. PDF saved to: " + pdfOutputPath);
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
    }
}