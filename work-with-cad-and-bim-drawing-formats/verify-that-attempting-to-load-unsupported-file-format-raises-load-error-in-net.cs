using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

namespace CadLoadDiagnostics
{
    class Program
    {
        static void Main(string[] args)
        {
            string unsupportedFilePath = "sample.unsupported";

            // Ensure placeholder file exists
            if (!File.Exists(unsupportedFilePath))
            {
                string dir = Path.GetDirectoryName(unsupportedFilePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                // Create an empty placeholder file
                File.WriteAllBytes(unsupportedFilePath, new byte[0]);
            }

            var loadOptions = new LoadOptions();
            loadOptions.IgnoreErrors = false; // Do not ignore errors so that they are reported

            try
            {
                using (Image cadImage = Image.Load(unsupportedFilePath, loadOptions))
                {
                    // If loading succeeds, this is unexpected for an unsupported format
                    Console.WriteLine("Image loaded unexpectedly.");
                }
            }
            catch (Exception ex)
            {
                // Expected path: an exception is thrown for unsupported formats
                Console.WriteLine($"Exception caught: {ex.Message}");

                // Inspect the collected load errors
                foreach (RenderResult error in loadOptions.Errors)
                {
                    Console.WriteLine($"Load error: {error.Message}, Code: {error.RenderCode}");
                }
            }
        }
    }
}