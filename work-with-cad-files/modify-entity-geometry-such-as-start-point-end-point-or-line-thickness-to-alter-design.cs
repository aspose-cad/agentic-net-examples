using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.ImageOptions;

namespace CadGeometryModification
{
    class Program
    {
        static void Main()
        {
            string inputPath = "input.dwg";
            string outputDxf = "output_modified.dxf";
            string outputPng = "output_modified.png";

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the CAD file
                CadImage cadImage = (CadImage)Image.Load(inputPath);

                // Modify the first line entity found
                foreach (var entity in cadImage.Entities)
                {
                    if (entity.TypeName == CadEntityTypeName.LINE)
                    {
                        var line = (CadLine)entity;

                        // Offset start point by (+10, +10)
                        var start = line.FirstPoint;
                        var newStart = new Cad3DPoint(start.X + 10.0, start.Y + 10.0, start.Z);
                        line.FirstPoint = newStart;

                        // Offset end point by (-5, -5)
                        var end = line.SecondPoint;
                        var newEnd = new Cad3DPoint(end.X - 5.0, end.Y - 5.0, end.Z);
                        line.SecondPoint = newEnd;

                        break; // modify only the first line
                    }
                }

                // Ensure output directories exist
                Directory.CreateDirectory(Path.GetDirectoryName(outputDxf) ?? ".");
                Directory.CreateDirectory(Path.GetDirectoryName(outputPng) ?? ".");

                // Save modified CAD as DXF (ASCII format)
                var dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Ascii
                };
                cadImage.Save(outputDxf, dxfOptions);

                // Render to PNG with increased line thickness
                var pngOptions = new PngOptions();
                var rasterOptions = new CadRasterizationOptions
                {
                    PageWidth = 800,
                    PageHeight = 600,
                    LineScale = 2.0f,
                    UnitType = UnitType.Centimenter
                };

                var penOptions = new PenOptions
                {
                    StartCap = LineCap.Flat,
                    EndCap = LineCap.Flat
                };
                rasterOptions.PenOptions = penOptions;

                pngOptions.VectorRasterizationOptions = rasterOptions;

                cadImage.Save(outputPng, pngOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing CAD file: {ex.Message}");
            }
        }
    }
}