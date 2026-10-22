using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadObjects.Vertices;
using Aspose.CAD.WatermarkGuard;

class Program
{
    static void Main()
    {
        string inputPath = "input.dxf";
        string outputPath = "output_watermarked.dxf";
        string watermarkText = "Confidential";

        try
        {
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load CAD drawing
            using (Image cadImage = Image.Load(inputPath))
            {
                CadImage cad = cadImage as CadImage;
                if (cad == null)
                {
                    Console.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Vertex extraction (example for polylines and 3D polylines)
                foreach (CadEntityBase entity in cad.Entities)
                {
                    if (entity is CadLwPolyline lwPolyline)
                    {
                        foreach (Cad2DPoint pt in lwPolyline.Coordinates)
                        {
                            // Placeholder for processing 2D point
                            // Console.WriteLine($"2D Point: ({pt.X}, {pt.Y})");
                        }
                    }
                    else if (entity is CadPolyline polyline3D)
                    {
                        foreach (CadVertexBase vertex in polyline3D.ChildObjects.OfType<CadVertexBase>())
                        {
                            Cad3DPoint pt3 = vertex.LocationPoint;
                            // Placeholder for processing 3D point
                            // Console.WriteLine($"3D Point: ({pt3.X}, {pt3.Y}, {pt3.Z})");
                        }
                    }
                }

                // Apply blind watermark using the image's WatermarkGuardService
                cad.WatermarkGuardService.TextToEmbed = watermarkText;
                WatermarkingStatus embedStatus = cad.WatermarkGuardService.EmbeddingStatus();
                if (embedStatus != WatermarkingStatus.SUCCESS)
                {
                    Console.WriteLine($"Watermark embedding failed: {embedStatus}");
                    return;
                }

                // Save watermarked CAD drawing
                DxfOptions saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };
                cadImage.Save(outputPath, saveOptions);
            }

            // Reload saved file and validate watermark
            using (Image savedImage = Image.Load(outputPath))
            {
                CadImage savedCad = savedImage as CadImage;
                if (savedCad == null)
                {
                    Console.WriteLine("Failed to cast saved image to CadImage.");
                    return;
                }

                savedCad.WatermarkGuardService.TextToValidate = watermarkText;
                WatermarkingStatus validationStatus = savedCad.WatermarkGuardService.ValidationStatus();
                if (validationStatus == WatermarkingStatus.SUCCESS)
                {
                    Console.WriteLine("Watermark validation succeeded.");
                }
                else
                {
                    Console.WriteLine($"Watermark validation failed: {validationStatus}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}