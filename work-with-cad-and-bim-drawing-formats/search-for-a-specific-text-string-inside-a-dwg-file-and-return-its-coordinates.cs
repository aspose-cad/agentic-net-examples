using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main(string[] args)
    {
        string inputPath = "sample.dwg";
        string searchString = "TargetText";

        if (!File.Exists(inputPath))
        {
            Console.Error.WriteLine($"Input file not found: {inputPath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to load CAD image.");
                    return;
                }

                bool found = false;

                foreach (var entity in cadImage.Entities)
                {
                    if (entity is CadText textEntity)
                    {
                        if (!string.IsNullOrEmpty(textEntity.DefaultValue) &&
                            textEntity.DefaultValue.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                        {
                            var pos = textEntity.FirstAlignment;
                            Console.WriteLine($"Found \"{searchString}\" in Text entity at ({pos.X}, {pos.Y})");
                            found = true;
                        }
                    }
                    else if (entity is CadMText mTextEntity)
                    {
                        if (!string.IsNullOrEmpty(mTextEntity.Text) &&
                            mTextEntity.Text.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                        {
                            var pos = mTextEntity.InsertionPoint;
                            Console.WriteLine($"Found \"{searchString}\" in MText entity at ({pos.X}, {pos.Y})");
                            found = true;
                        }
                    }
                }

                if (!found)
                {
                    Console.WriteLine($"Text \"{searchString}\" not found in the drawing.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing file: {ex.Message}");
        }
    }
}