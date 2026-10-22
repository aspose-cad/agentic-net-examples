using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadStepExport
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dwg";
            string outputPath = "output.stp";

            try
            {
                ExportToStep(inputPath, outputPath);
                Console.WriteLine("Export completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        static void ExportToStep(string inputPath, string outputPath)
        {
            if (!File.Exists(inputPath))
                throw new FileNotFoundException("Input file not found.", inputPath);

            string outDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outDir) && !Directory.Exists(outDir))
                Directory.CreateDirectory(outDir);

            using (Image cadImage = Image.Load(inputPath))
            {
                // STEP export is not supported by Aspose.CAD.
                // The SDK provides DxfOptions for DXF export, but no StepOptions or equivalent.
                throw new NotSupportedException("STEP (STP) export is not supported by Aspose.CAD library.");
            }
        }
    }
}