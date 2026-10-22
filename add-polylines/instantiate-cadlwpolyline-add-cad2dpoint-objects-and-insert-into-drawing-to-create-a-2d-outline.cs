using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "input.dwg";
            string outputPath = "output.pdf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image loadedImage = Image.Load(inputPath))
            {
                // Cast to CadImage to access CAD-specific members
                CadImage cadImage = (CadImage)loadedImage;

                // Create a lightweight polyline (2D outline)
                var polyline = new CadLwPolyline();

                // Set the polyline to be closed (using reflection for compatibility)
                PropertyInfo isClosedProp = typeof(CadLwPolyline).GetProperty("IsClosed");
                if (isClosedProp != null && isClosedProp.CanWrite)
                {
                    isClosedProp.SetValue(polyline, true);
                }

                // Add vertices to the polyline (using reflection for compatibility)
                PropertyInfo verticesProp = typeof(CadLwPolyline).GetProperty("Vertices");
                if (verticesProp != null)
                {
                    var vertices = verticesProp.GetValue(polyline) as IList<Cad2DPoint>;
                    if (vertices != null)
                    {
                        vertices.Add(new Cad2DPoint(0, 0));
                        vertices.Add(new Cad2DPoint(200, 0));
                        vertices.Add(new Cad2DPoint(200, 150));
                        vertices.Add(new Cad2DPoint(0, 150));
                    }
                }

                // Insert the polyline into the Model Space block if possible
                if (cadImage.BlockEntities != null && cadImage.BlockEntities.ContainsKey("*Model_Space"))
                {
                    var modelSpace = cadImage.BlockEntities["*Model_Space"];
                    if (modelSpace.Entities is IList<CadEntityBase> entityList)
                    {
                        entityList.Add(polyline);
                    }
                }

                // Save the modified drawing as PDF
                var pdfOptions = new PdfOptions();
                cadImage.Save(outputPath, pdfOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
