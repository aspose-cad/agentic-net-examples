using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadArcTextExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourcePath = "input.dwg";
            string outputPath = "output.dxf";

            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            try
            {
                using (Image image = Image.Load(sourcePath))
                {
                    CadImage cadImage = (CadImage)image;

                    // Create ARC entity
                    CadArc arc = new CadArc
                    {
                        CenterPoint = new Cad3DPoint(0.0, 0.0, 0.0),
                        Radius = 50.0,
                        StartAngle = 0.0,
                        EndAngle = 180.0,
                        LayerName = "0"
                    };

                    // Determine SoftOwner for text (use first layout's block record handle if available)
                    string softOwner = cadImage.Layouts.Values.FirstOrDefault()?.BlockTableRecordHandle ?? string.Empty;

                    // Create TEXT entity
                    CadText text = new CadText
                    {
                        FirstAlignment = new Cad3DPoint(0.0, 0.0, 0.0),
                        TextHeight = 12.0,
                        DefaultValue = "Sample Text",
                        TextRotation = 0.0,
                        SoftOwner = softOwner,
                        LayerName = "0"
                    };

                    // Append new entities to the drawing
                    var updatedEntities = cadImage.Entities
                        .Concat(new CadEntityBase[] { arc, text })
                        .ToArray();
                    cadImage.Entities = updatedEntities;

                    // Save the modified drawing as DXF
                    var dxfOptions = new DxfOptions();
                    cadImage.Save(outputPath, dxfOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}