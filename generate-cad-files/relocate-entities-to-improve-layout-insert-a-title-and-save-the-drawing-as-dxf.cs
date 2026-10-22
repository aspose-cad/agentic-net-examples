using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                var cadImage = (CadImage)image;

                // Translation offset
                double offsetX = 100.0;
                double offsetY = 50.0;

                // Relocate entities
                foreach (var entity in cadImage.Entities)
                {
                    switch (entity)
                    {
                        case CadLine line:
                            line.FirstPoint = new Cad3DPoint(line.FirstPoint.X + offsetX, line.FirstPoint.Y + offsetY, line.FirstPoint.Z);
                            line.SecondPoint = new Cad3DPoint(line.SecondPoint.X + offsetX, line.SecondPoint.Y + offsetY, line.SecondPoint.Z);
                            break;

                        case CadLwPolyline polyline:
                            for (int i = 0; i < polyline.Coordinates.Count; i++)
                            {
                                var pt = polyline.Coordinates[i];
                                polyline.Coordinates[i] = new Cad2DPoint(pt.X + offsetX, pt.Y + offsetY);
                            }
                            break;

                        case CadInsertObject insert:
                            var pos = insert.InsertionPoint;
                            insert.InsertionPoint = new Cad3DPoint(pos.X + offsetX, pos.Y + offsetY, pos.Z);
                            break;

                        // Add handling for other entity types as needed
                    }
                }

                // Insert title text
                var title = new CadMText
                {
                    Text = "Project Title",
                    InsertionPoint = new Cad3DPoint(0.0, 0.0, 0.0),
                    InitialTextHeight = 10.0
                };

                // Append the new entity
                cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { title }).ToArray();

                // Save as DXF
                var dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };
                cadImage.Save(outputPath, dxfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}