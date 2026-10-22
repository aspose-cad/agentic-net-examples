using System;
using System.IO;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        const string inputPath = "input.dwg";
        const string outputPath = "output.dxf";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                var cadImage = (CadImage)image;

                foreach (CadEntityBase entity in cadImage.Entities)
                {
                    if (entity is CadLine line)
                    {
                        // Set line weight (in hundredths of a millimeter)
                        line.LineWeight = 25;

                        // Attempt to set ConstantWidth via reflection (property may not exist in this SDK version)
                        PropertyInfo constWidthProp = line.GetType().GetProperty("ConstantWidth", BindingFlags.Public | BindingFlags.Instance);
                        if (constWidthProp != null && constWidthProp.CanWrite)
                        {
                            constWidthProp.SetValue(line, 0.5);
                            Console.WriteLine("ConstantWidth set via reflection.");
                        }
                        else
                        {
                            Console.WriteLine("ConstantWidth property not available in this SDK version.");
                        }

                        break;
                    }
                }

                var saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };

                cadImage.Save(outputPath, saveOptions);
                Console.WriteLine($"Modified drawing saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}