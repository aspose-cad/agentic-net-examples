using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main(string[] args)
    {
        // Path to the source CAD file
        string cadPath = "sample.dwg";

        if (!File.Exists(cadPath))
        {
            Console.WriteLine($"CAD file not found at path: {cadPath}");
            return;
        }

        try
        {
            using (Image cadImage = Image.Load(cadPath))
            {
                // ---------- Automatic sizing ----------
                // Save PNG with default options (automatic size based on drawing extents)
                PngOptions autoOptions = new PngOptions();
                string autoPngPath = "auto_sized.png";

                try
                {
                    cadImage.Save(autoPngPath, autoOptions);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to save automatic PNG: {ex.Message}");
                    return;
                }

                if (File.Exists(autoPngPath))
                {
                    try
                    {
                        using (Image autoPng = Image.Load(autoPngPath))
                        {
                            Console.WriteLine($"Automatic sizing - Width: {autoPng.Width}, Height: {autoPng.Height}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to load automatic PNG: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Automatic PNG file was not created.");
                }

                // ---------- Manual dimensions ----------
                // Save PNG with explicitly set width and height
                PngOptions manualOptions = new PngOptions
                {
                    VectorRasterizationOptions = new CadRasterizationOptions
                    {
                        PageWidth = 800,
                        PageHeight = 600
                    }
                };
                string manualPngPath = "manual_sized.png";

                try
                {
                    cadImage.Save(manualPngPath, manualOptions);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to save manual PNG: {ex.Message}");
                    return;
                }

                if (File.Exists(manualPngPath))
                {
                    try
                    {
                        using (Image manualPng = Image.Load(manualPngPath))
                        {
                            Console.WriteLine($"Manual sizing - Width: {manualPng.Width}, Height: {manualPng.Height}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to load manual PNG: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Manual PNG file was not created.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}
