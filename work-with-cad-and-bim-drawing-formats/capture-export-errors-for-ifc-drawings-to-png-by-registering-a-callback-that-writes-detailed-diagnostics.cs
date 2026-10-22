using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace AsposeCadDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFilePath = "input.dwg";
            string outputDirectory = "output";

            if (!File.Exists(inputFilePath))
            {
                Console.Error.WriteLine($"Error: The specified CAD file \"{inputFilePath}\" does not exist.");
                return;
            }

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            var loadOptions = new LoadOptions();
            loadOptions.IgnoreErrors = true;

            try
            {
                using (Image cadImage = Image.Load(inputFilePath, loadOptions))
                {
                    if (loadOptions.Errors != null && loadOptions.Errors.Count > 0)
                    {
                        Console.Error.WriteLine("Load completed with the following warnings/errors:");
                        foreach (var error in loadOptions.Errors)
                        {
                            Console.Error.WriteLine($"{error.RenderCode}: {error.Message}");
                        }
                    }

                    var cad = cadImage as CadImage;
                    if (cad == null)
                    {
                        Console.Error.WriteLine("Error: Loaded image could not be cast to CadImage.");
                        return;
                    }

                    foreach (var layoutEntry in cad.Layouts)
                    {
                        var layout = layoutEntry.Value;
                        string layoutName = layoutEntry.Key;

                        var rasterOptions = new CadRasterizationOptions
                        {
                            PageWidth = 1600f,
                            PageHeight = 1200f,
                            Layouts = new[] { layoutName }
                        };

                        rasterOptions.RenderResult = result =>
                        {
                            foreach (var failure in result.Failures)
                            {
                                Console.Error.WriteLine($"{failure.RenderCode}: {failure.Message}");
                            }
                        };

                        var pngOptions = new PngOptions
                        {
                            VectorRasterizationOptions = rasterOptions
                        };

                        string pngFileName = $"{Path.GetFileNameWithoutExtension(inputFilePath)}_{layoutName}.png";
                        string pngFilePath = Path.Combine(outputDirectory, pngFileName);

                        try
                        {
                            cadImage.Save(pngFilePath, pngOptions);
                            Console.WriteLine($"Successfully saved PNG for layout \"{layoutName}\" to \"{pngFilePath}\".");
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Error saving PNG for layout \"{layoutName}\": {saveEx.Message}");
                        }
                    }

                    var dxfOptions = new DxfOptions
                    {
                        DxfFileFormat = CadFileFormat.Binary,
                        OutputMode = CadOutputMode.Convert
                    };

                    string dxfFileName = $"{Path.GetFileNameWithoutExtension(inputFilePath)}_converted.dxf";
                    string dxfFilePath = Path.Combine(outputDirectory, dxfFileName);

                    try
                    {
                        cadImage.Save(dxfFilePath, dxfOptions);
                        Console.WriteLine($"Successfully saved DXF to \"{dxfFilePath}\".");
                    }
                    catch (Exception dxfSaveEx)
                    {
                        Console.Error.WriteLine($"Error saving DXF: {dxfSaveEx.Message}");
                    }
                }
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load CAD file \"{inputFilePath}\": {loadEx.Message}");
            }

            Console.WriteLine("Processing completed.");
        }
    }
}