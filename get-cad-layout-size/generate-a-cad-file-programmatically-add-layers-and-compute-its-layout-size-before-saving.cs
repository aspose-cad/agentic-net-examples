using System;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadGenerationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new empty CAD image (DWG format)
            using (var cadImage = new CadImage())
            {
                // Add a new layer to the drawing
                var newLayer = new CadLayerTable
                {
                    Name = "MyLayer",
                    ColorId = 7,      // Default color (white)
                    Flags = 0,
                    PlotFlag = true
                };
                cadImage.Layers.Add(newLayer);

                // Add a line entity to the new layer
                var line = new CadLine
                {
                    FirstPoint = new Cad3DPoint(0, 0, 0),
                    SecondPoint = new Cad3DPoint(100, 100, 0),
                    LayerName = "MyLayer"
                };
                cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { line }).ToArray();

                // Compute layout size (using the first layout, typically model space)
                var layout = cadImage.Layouts?.Values?.FirstOrDefault();
                if (layout != null)
                {
                    var min = layout.MinExtents;
                    var max = layout.MaxExtents;
                    double width = max.X - min.X;
                    double height = max.Y - min.Y;
                    Console.WriteLine($"Layout Size: Width = {width}, Height = {height}");
                }
                else
                {
                    Console.WriteLine("No layouts found in the CAD image.");
                }

                // Save the CAD image to a DWG file with error handling
                try
                {
                    cadImage.Save("GeneratedDrawing.dwg");
                    Console.WriteLine("CAD file saved successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving CAD file: {ex.Message}");
                }
            }
        }
    }
}