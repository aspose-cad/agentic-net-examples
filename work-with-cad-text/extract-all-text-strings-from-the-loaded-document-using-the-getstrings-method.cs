using Aspose.CAD.FileFormats.Cad;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;

namespace AsposeCadExtractStrings
{
    class Program
    {
        static void Main()
        {
            try
            {
                string inputPath = "input.dwg";

                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file does not exist: {inputPath}");
                    return;
                }

                using (CadImage cadImage = (CadImage)Image.Load(inputPath))
                {
                    IList<string> strings = cadImage.GetStrings();

                    Console.WriteLine("Extracted text strings:");
                    foreach (string text in strings)
                    {
                        Console.WriteLine(text);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}