using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputDirectory = "output";
        const string outputFileName = "output.dxf";
        const int maxRetries = 1;

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        Directory.CreateDirectory(outputDirectory);
        string outputPath = Path.Combine(outputDirectory, outputFileName);

        try
        {
            using (Image cadImage = Image.Load(inputPath))
            {
                DxfOptions dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };

                int attempt = 0;
                bool saved = false;

                while (attempt <= maxRetries && !saved)
                {
                    try
                    {
                        cadImage.Save(outputPath, dxfOptions);
                        saved = true;
                        Console.WriteLine($"DXF file saved to: {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Export error on attempt {attempt + 1}: {ex.Message}");
                        if (attempt < maxRetries)
                        {
                            Console.WriteLine("Retrying export...");
                        }
                        else
                        {
                            Console.Error.WriteLine("Export failed after retries.");
                            throw;
                        }
                    }

                    attempt++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}