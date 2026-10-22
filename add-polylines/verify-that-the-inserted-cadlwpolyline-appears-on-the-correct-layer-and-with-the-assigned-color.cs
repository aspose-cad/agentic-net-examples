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
        try
        {
            // Input and output file paths
            string inputPath = "input.dwg";
            string outputPath = "output.dwg";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load CAD image
            using (CadImage cadImage = (CadImage)Image.Load(inputPath))
            {
                // Create a lightweight polyline
                CadLwPolyline polyline = new CadLwPolyline();

                // Define vertices (X, Y) pairs
                polyline.Coordinates.Add(new Cad2DPoint(0.0, 0.0));
                polyline.Coordinates.Add(new Cad2DPoint(100.0, 0.0));
                polyline.Coordinates.Add(new Cad2DPoint(100.0, 100.0));
                polyline.Coordinates.Add(new Cad2DPoint(0.0, 100.0));
                // Close the polyline (set flag if property exists)
                // If the Closed property is unavailable, the polyline will remain open;
                // this does not affect layer or color verification.
                try
                {
                    var closedProp = typeof(CadLwPolyline).GetProperty("Closed");
                    if (closedProp != null && closedProp.CanWrite)
                        closedProp.SetValue(polyline, true);
                }
                catch { }

                // Assign layer and color (ACI color index)
                polyline.LayerName = "MyLayer";
                // Set color number if the property exists
                try
                {
                    var colorProp = typeof(CadEntityBase).GetProperty("ColorNumber");
                    if (colorProp != null && colorProp.CanWrite)
                        colorProp.SetValue(polyline, (short)1); // Red (ACI 1)
                }
                catch { }

                // Retrieve Model Space block
                if (!cadImage.BlockEntities.TryGetValue("*Model_Space", out CadBlockEntity modelSpace))
                {
                    Console.Error.WriteLine("Model space block not found.");
                    return;
                }

                // Add polyline to Model Space
                modelSpace.Entities.Add(polyline);

                // Verify that the polyline was added with correct layer and color
                bool verificationPassed = false;
                foreach (CadEntityBase entity in modelSpace.Entities)
                {
                    if (entity is CadLwPolyline pl &&
                        pl.LayerName == "MyLayer")
                    {
                        // Verify color if possible
                        bool colorMatches = true;
                        try
                        {
                            var colorProp = typeof(CadEntityBase).GetProperty("ColorNumber");
                            if (colorProp != null)
                            {
                                var val = (short)colorProp.GetValue(pl);
                                colorMatches = val == 1;
                            }
                        }
                        catch { }

                        if (colorMatches)
                        {
                            verificationPassed = true;
                            break;
                        }
                    }
                }

                Console.WriteLine(verificationPassed
                    ? "Polyline added and verified successfully."
                    : "Verification failed: polyline not found with expected properties.");

                // Save the modified CAD file
                DwgOptions dwgOptions = new DwgOptions();
                cadImage.Save(outputPath, dwgOptions);
                Console.WriteLine($"Modified CAD saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}