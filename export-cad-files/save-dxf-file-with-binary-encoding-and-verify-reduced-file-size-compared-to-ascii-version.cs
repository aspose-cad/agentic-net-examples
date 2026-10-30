using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadConsts;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputFilePath = args.Length > 0 ? args[0] : "input.dxf";

            if (!File.Exists(inputFilePath))
            {
                Console.WriteLine($"Input file not found: {inputFilePath}");
                return;
            }

            string outputDirectory = "output";
            Directory.CreateDirectory(outputDirectory);

            string asciiOutputPath = Path.Combine(outputDirectory, "output_ascii.dxf");
            string binaryOutputPath = Path.Combine(outputDirectory, "output_binary.dxf");

            using (Image cadImage = Image.Load(inputFilePath))
            {
                var asciiOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Ascii
                };
                cadImage.Save(asciiOutputPath, asciiOptions);
                Console.WriteLine($"ASCII DXF saved to: {asciiOutputPath}");

                var binaryOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Binary
                };
                cadImage.Save(binaryOutputPath, binaryOptions);
                Console.WriteLine($"Binary DXF saved to: {binaryOutputPath}");
            }

            if (File.Exists(asciiOutputPath) && File.Exists(binaryOutputPath))
            {
                long asciiSize = new FileInfo(asciiOutputPath).Length;
                long binarySize = new FileInfo(binaryOutputPath).Length;

                Console.WriteLine($"ASCII DXF size: {asciiSize} bytes");
                Console.WriteLine($"Binary DXF size: {binarySize} bytes");

                if (binarySize < asciiSize)
                {
                    Console.WriteLine("Binary DXF is smaller than ASCII DXF.");
                }
                else
                {
                    Console.WriteLine("Binary DXF is not smaller than ASCII DXF.");
                }
            }
            else
            {
                Console.WriteLine("One or both DXF files were not created.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}