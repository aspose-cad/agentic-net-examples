using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace AddPolylineExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFilePath = "sample.dwg"; // TODO: replace with actual path
            string outputFolderPath = "Output";
            string outputPngPath = Path.Combine(outputFolderPath, "sample_with_polyline.png");

            if (string.IsNullOrWhiteSpace(inputFilePath) || !File.Exists(inputFilePath))
            {
                Console.Error.WriteLine("Error: Input CAD file not found at path: " + inputFilePath);
                return;
            }

            if (!string.IsNullOrWhiteSpace(outputFolderPath) && !Directory.Exists(outputFolderPath))
            {
                Directory.CreateDirectory(outputFolderPath);
            }

            try
            {
                using (Image loadedImage = Image.Load(inputFilePath))
                {
                    CadImage cadImage = loadedImage as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Error: Loaded file is not a recognized CAD format.");
                        return;
                    }

                    // Create a lightweight polyline (CadLwPolyline)
                    CadLwPolyline newPolyline = new CadLwPolyline
                    {
                        LayerName = "0",
                        Flag = CadLwPolylineFlag.Closed,
                        ColorId = -1 // ByLayer
                    };

                    newPolyline.Coordinates.Add(new Cad2DPoint(10.0, 10.0));
                    newPolyline.Coordinates.Add(new Cad2DPoint(200.0, 10.0));
                    newPolyline.Coordinates.Add(new Cad2DPoint(200.0, 150.0));
                    newPolyline.Coordinates.Add(new Cad2DPoint(10.0, 150.0));

                    // Insert the polyline into the drawing
                    cadImage.Entities = cadImage.Entities.Concat(new CadEntityBase[] { newPolyline }).ToArray();

                    // Verify addition
                    int polylineCount = cadImage.Entities.Count(e => e is CadLwPolyline);
                    Console.WriteLine("Verification: Number of lightweight polylines in drawing = " + polylineCount);
                    if (polylineCount == 0)
                    {
                        Console.Error.WriteLine("Error: Polyline was not added to the drawing.");
                    }

                    // Get the Model layout (first layout is usually Model)
                    var modelLayout = cadImage.Layouts.Values.FirstOrDefault();
                    if (modelLayout == null)
                    {
                        Console.Error.WriteLine("Error: No layout found in the CAD file.");
                        return;
                    }

                    // Export to PNG for visual verification
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions
                    {
                        PageWidth = 800,
                        PageHeight = 600,
                        Layouts = new[] { modelLayout.LayoutName },
                        DrawType = CadDrawTypeMode.UseObjectColor
                    };

                    PngOptions pngOptions = new PngOptions
                    {
                        VectorRasterizationOptions = rasterOptions
                    };

                    cadImage.Save(outputPngPath, pngOptions);
                    Console.WriteLine("Export completed. PNG saved to: " + outputPngPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("An exception occurred: " + ex.Message);
            }
        }
    }
}
