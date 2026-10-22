using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Input CAD file path
            string sourcePath = "input.dwg";

            // Directory where external references will be copied
            string targetDirectory = "ExternalFiles";

            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            // Load the CAD file
            using (Image image = Image.Load(sourcePath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("The loaded file is not a CAD image.");
                    return;
                }

                // Find insert objects that likely reference external files (XREFs)
                var externalBlocks = cadImage.Entities
                                            .OfType<CadInsertObject>()
                                            .Where(br => !string.IsNullOrEmpty(br.Name) &&
                                                         (br.Name.EndsWith(".dwg", StringComparison.OrdinalIgnoreCase) ||
                                                          br.Name.EndsWith(".dxf", StringComparison.OrdinalIgnoreCase)));

                foreach (CadInsertObject blockRef in externalBlocks)
                {
                    // Assume the Name property holds the external reference path (may be relative)
                    string externalPath = blockRef.Name;

                    if (string.IsNullOrEmpty(externalPath))
                        continue;

                    // Resolve relative paths against the source CAD file directory
                    if (!Path.IsPathRooted(externalPath))
                    {
                        string sourceDir = Path.GetDirectoryName(sourcePath);
                        externalPath = Path.Combine(sourceDir ?? string.Empty, externalPath);
                    }

                    if (!File.Exists(externalPath))
                    {
                        Console.Error.WriteLine($"External reference not found: {externalPath}");
                        continue;
                    }

                    string fileName = Path.GetFileName(externalPath);
                    string destinationPath = Path.Combine(targetDirectory, fileName);

                    try
                    {
                        File.Copy(externalPath, destinationPath, overwrite: true);
                        Console.WriteLine($"Copied: {externalPath} -> {destinationPath}");
                    }
                    catch (Exception copyEx)
                    {
                        Console.Error.WriteLine($"Failed to copy '{externalPath}': {copyEx.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}