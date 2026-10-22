using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.pdf";

        if (!File.Exists(inputPath))
        {
            Console.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        Image image;
        try
        {
            image = Image.Load(inputPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load CAD drawing: {ex.Message}");
            return;
        }

        using (image)
        {
            var cadImage = (CadImage)image;

            // Define vertical offset (negative Y moves drawing down)
            double offsetY = -50.0;

            // Translate all top‑level entities
            foreach (CadEntityBase entity in cadImage.Entities)
            {
                switch (entity)
                {
                    case CadLine line:
                        line.FirstPoint = new Cad3DPoint(line.FirstPoint.X, line.FirstPoint.Y + offsetY, line.FirstPoint.Z);
                        line.SecondPoint = new Cad3DPoint(line.SecondPoint.X, line.SecondPoint.Y + offsetY, line.SecondPoint.Z);
                        break;

                    case CadLwPolyline polyline:
                        for (int i = 0; i < polyline.Coordinates.Count; i++)
                        {
                            Cad2DPoint pt = polyline.Coordinates[i];
                            polyline.Coordinates[i] = new Cad2DPoint(pt.X, pt.Y + offsetY);
                        }
                        break;

                    case Cad3DFace face:
                        face.FirstCorner = new Cad3DPoint(face.FirstCorner.X, face.FirstCorner.Y + offsetY, face.FirstCorner.Z);
                        face.SecondCorner = new Cad3DPoint(face.SecondCorner.X, face.SecondCorner.Y + offsetY, face.SecondCorner.Z);
                        face.ThirdCorner = new Cad3DPoint(face.ThirdCorner.X, face.ThirdCorner.Y + offsetY, face.ThirdCorner.Z);
                        if (face.FourthCorner != null)
                        {
                            face.FourthCorner = new Cad3DPoint(face.FourthCorner.X, face.FourthCorner.Y + offsetY, face.FourthCorner.Z);
                        }
                        break;

                    case CadInsertObject insert:
                        insert.InsertionPoint = new Cad3DPoint(insert.InsertionPoint.X, insert.InsertionPoint.Y + offsetY, insert.InsertionPoint.Z);
                        break;

                    // Additional entity types can be handled here as needed
                }
            }

            // Save the modified drawing as PDF
            var pdfOptions = new PdfOptions();
            try
            {
                cadImage.Save(outputPath, pdfOptions);
                Console.WriteLine($"Successfully saved modified drawing to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save PDF: {ex.Message}");
            }
        }
    }
}
