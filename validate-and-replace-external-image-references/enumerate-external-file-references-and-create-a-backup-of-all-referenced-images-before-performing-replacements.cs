using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            string inputCadPath = "input.dwg";
            string outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "output");
            string backupDirectory = Path.Combine(outputDirectory, "backup");

            if (!File.Exists(inputCadPath))
            {
                Console.Error.WriteLine($"Input CAD file not found: {inputCadPath}");
                return;
            }

            Directory.CreateDirectory(outputDirectory);
            Directory.CreateDirectory(backupDirectory);

            using (Image image = Image.Load(inputCadPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                    return;
                }

                // Process top‑level entities
                ProcessEntities(cadImage.Entities, inputCadPath, backupDirectory);

                // Process block entities
                foreach (DictionaryEntry entry in cadImage.BlockEntities)
                {
                    var block = entry.Value as CadBlockEntity;
                    if (block != null)
                    {
                        ProcessEntities(block.Entities, inputCadPath, backupDirectory);
                    }
                }

                // Save the (potentially) modified CAD file
                string outputCadPath = Path.Combine(outputDirectory, "output.dwg");
                var dxfOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert
                };
                cadImage.Save(outputCadPath, dxfOptions);
                Console.WriteLine($"Saved modified CAD file to: {outputCadPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void ProcessEntities(IEnumerable<CadEntityBase> entities, string cadFilePath, string backupDir)
    {
        foreach (var entity in entities)
        {
            Type entityType = entity.GetType();

            // Heuristic: raster image entities usually contain "Image" in their type name
            if (entityType.Name.Contains("Image"))
            {
                // Try to obtain a property that holds the external file name/path
                PropertyInfo prop = entityType.GetProperty("FileName") ??
                                    entityType.GetProperty("ImagePath") ??
                                    entityType.GetProperty("Path");

                if (prop != null)
                {
                    string referencedPath = prop.GetValue(entity) as string;
                    if (!string.IsNullOrEmpty(referencedPath))
                    {
                        // Resolve relative paths based on the CAD file location
                        if (!Path.IsPathRooted(referencedPath))
                        {
                            string cadFolder = Path.GetDirectoryName(Path.GetFullPath(cadFilePath));
                            referencedPath = Path.Combine(cadFolder, referencedPath);
                        }

                        if (File.Exists(referencedPath))
                        {
                            string backupFile = Path.Combine(backupDir, Path.GetFileName(referencedPath));
                            if (!File.Exists(backupFile))
                            {
                                File.Copy(referencedPath, backupFile);
                                Console.WriteLine($"Backed up: {referencedPath} -> {backupFile}");
                            }
                            else
                            {
                                Console.WriteLine($"Backup already exists for: {referencedPath}");
                            }

                            // Placeholder for replacement logic
                            // string newPath = "newImage.png";
                            // prop.SetValue(entity, newPath);
                        }
                        else
                        {
                            Console.Error.WriteLine($"Referenced image not found: {referencedPath}");
                        }
                    }
                }
            }
        }
    }
}