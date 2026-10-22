using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadImageReferenceChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            // Placeholder input CAD file path
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file does not exist: {inputPath}");
                return;
            }

            try
            {
                using (Image cadImage = Image.Load(inputPath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cad = cadImage as CadImage;
                    if (cad == null)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    string cadDirectory = Path.GetDirectoryName(inputPath) ?? string.Empty;
                    bool missingFound = false;

                    // Iterate over all CAD objects to find raster image definitions
                    foreach (CadBaseObject cadObject in cad.Objects)
                    {
                        if (cadObject is CadRasterImageDef rasterDef)
                        {
                            string referencedFile = rasterDef.FileName;
                            if (string.IsNullOrEmpty(referencedFile))
                            {
                                Console.WriteLine($"Raster image definition with handle {rasterDef.ObjectHandle} has no file name.");
                                continue;
                            }

                            // Resolve relative paths against the CAD file directory
                            string resolvedPath = Path.IsPathRooted(referencedFile)
                                ? referencedFile
                                : Path.Combine(cadDirectory, referencedFile);

                            if (!File.Exists(resolvedPath))
                            {
                                Console.WriteLine($"Missing external image reference: {referencedFile} (Resolved: {resolvedPath}, ObjectHandle: {rasterDef.ObjectHandle})");
                                missingFound = true;
                            }
                        }
                    }

                    if (!missingFound)
                    {
                        Console.WriteLine("No missing external image references were detected.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}