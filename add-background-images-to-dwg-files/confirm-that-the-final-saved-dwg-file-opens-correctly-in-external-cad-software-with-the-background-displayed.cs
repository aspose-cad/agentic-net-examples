using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

namespace AsposeCadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the source DWG file
            string sourcePath = "input.dwg";
            // Path for the saved DWG file
            string outputPath = "output.dwg";

            // Verify source file exists
            if (!File.Exists(sourcePath))
            {
                Console.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            try
            {
                // Load the DWG file using Aspose.CAD.Image.Load
                using (Image loadedImage = Image.Load(sourcePath))
                {
                    // Cast the loaded image to CadImage to access CAD-specific members if needed
                    CadImage cadImage = (CadImage)loadedImage;

                    // (Optional) Perform any modifications to the CAD drawing here.
                    // For this example we simply re-save the file to confirm it opens correctly.

                    try
                    {
                        // Save the CAD image back to DWG format.
                        cadImage.Save(outputPath);
                        Console.WriteLine("DWG file has been saved to: " + outputPath);
                    }
                    catch (Exception saveEx)
                    {
                        Console.WriteLine("Error saving DWG file: " + saveEx.Message);
                    }
                }
            }
            catch (Exception loadEx)
            {
                Console.WriteLine("Error loading DWG file: " + loadEx.Message);
            }
        }
    }
}
