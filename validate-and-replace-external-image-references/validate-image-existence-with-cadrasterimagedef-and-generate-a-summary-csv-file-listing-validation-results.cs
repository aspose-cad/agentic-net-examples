using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder input CAD file path
            string inputPath = "input.dwg";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input CAD file not found: {inputPath}");
                return;
            }

            // Prepare output directory and CSV file path
            string outputDirectory = "output";
            Directory.CreateDirectory(outputDirectory);
            string csvPath = Path.Combine(outputDirectory, "RasterImageValidation.csv");

            // Load CAD image
            using (Image cadFile = Image.Load(inputPath))
            {
                CadImage cadImage = (CadImage)cadFile;

                // Collect validation results
                List<string> csvLines = new List<string>();
                csvLines.Add("ObjectHandle,FileName,Exists");

                foreach (CadBaseObject cadObject in cadImage.Objects)
                {
                    if (cadObject is CadRasterImageDef rasterDef)
                    {
                        string fileName = rasterDef.FileName ?? string.Empty;
                        // Resolve relative paths based on CAD file location
                        string resolvedPath = Path.IsPathRooted(fileName)
                            ? fileName
                            : Path.Combine(Path.GetDirectoryName(inputPath) ?? string.Empty, fileName);
                        bool exists = File.Exists(resolvedPath);
                        string line = $"{rasterDef.ObjectHandle},{fileName},{exists}";
                        csvLines.Add(line);
                    }
                }

                // Write CSV summary
                File.WriteAllLines(csvPath, csvLines);
                Console.WriteLine($"Validation summary written to: {csvPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}