using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            using (Image loadedImage = Image.Load(inputPath))
            {
                if (loadedImage is CadImage cadImage)
                {
                    var layoutDictionary = cadImage.Layouts;

                    // Attempt to enumerate layout identifiers
                    if (layoutDictionary is System.Collections.IDictionary dict)
                    {
                        foreach (var key in dict.Keys)
                        {
                            Console.WriteLine($"Layout identifier: {key}");
                        }
                    }
                    else if (layoutDictionary is System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, object>> kvEnum)
                    {
                        foreach (var kv in kvEnum)
                        {
                            Console.WriteLine($"Layout identifier: {kv.Key}");
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine("Unable to enumerate layout identifiers.");
                    }
                }
                else
                {
                    Console.Error.WriteLine("The loaded file is not a CAD image.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}