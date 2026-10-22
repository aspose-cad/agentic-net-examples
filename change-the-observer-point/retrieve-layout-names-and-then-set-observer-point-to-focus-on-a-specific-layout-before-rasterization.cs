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
                string inputPath = "input.dwg";

                // Verify that the input file exists
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file '{inputPath}' does not exist.");
                    return;
                }

                // Load the CAD image
                using (Image cadImage = Image.Load(inputPath))
                {
                    CadImage cad = (CadImage)cadImage;

                    // Retrieve layout names
                    string[] layoutNames = new string[cad.Layouts.Count];
                    int index = 0;
                    foreach (CadLayout layout in cad.Layouts.Values)
                    {
                        layoutNames[index++] = layout.LayoutName;
                    }

                    // Select a layout (use the first one if available)
                    string selectedLayout = layoutNames.Length > 0 ? layoutNames[0] : null;

                    // Configure rasterization options
                    CadRasterizationOptions rasterOptions = new CadRasterizationOptions();
                    if (selectedLayout != null)
                    {
                        rasterOptions.Layouts = new string[] { selectedLayout };
                    }
                    rasterOptions.PageWidth = 1600;
                    rasterOptions.PageHeight = 1200;
                    rasterOptions.DrawType = CadDrawTypeMode.UseObjectColor;

                    // Configure PNG output options
                    PngOptions pngOptions = new PngOptions();
                    pngOptions.VectorRasterizationOptions = rasterOptions;

                    string outputPath = "output.png";

                    // Save the rasterized image
                    cadImage.Save(outputPath, pngOptions);
                    Console.WriteLine($"Rasterized image saved to '{outputPath}'.");
                }
    
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}
}