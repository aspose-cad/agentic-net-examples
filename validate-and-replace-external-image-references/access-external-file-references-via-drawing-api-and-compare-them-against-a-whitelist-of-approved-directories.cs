using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

namespace CADReferenceChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the CAD file to be examined
                string inputFilePath = "input.dwg";

                // Verify that the input file exists
                if (!File.Exists(inputFilePath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputFilePath}");
                    return;
                }

                // Whitelist of directories that are allowed for external references
                List<string> approvedDirectories = new List<string>
                {
                    @"C:\ApprovedCADRefs",
                    @"D:\Shared\CADRefs"
                };

                // Load the CAD file
                using (Image loadedImage = Image.Load(inputFilePath))
                {
                    CadImage cadImage = loadedImage as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("The loaded file is not a CAD image.");
                        return;
                    }

                    bool anyReference = false;

                    // Iterate through block entities to find external references (XREFs)
                    foreach (DictionaryEntry entry in cadImage.BlockEntities)
                    {
                        dynamic block = entry.Value;
                        bool isExternal = false;

                        // Attempt to determine if this block is an external reference
                        try
                        {
                            isExternal = (bool)block.IsExternalReference;
                        }
                        catch
                        {
                            // Property not available; skip this block
                            continue;
                        }

                        if (!isExternal)
                            continue;

                        anyReference = true;

                        string refPath = string.Empty;
                        try
                        {
                            refPath = (string)block.Path;
                        }
                        catch
                        {
                            // Fallback to block name if path is not exposed
                            try { refPath = (string)block.Name; } catch { refPath = "UnknownPath"; }
                        }

                        string refDirectory = Path.GetDirectoryName(refPath) ?? string.Empty;
                        bool isApproved = false;

                        foreach (string approvedDir in approvedDirectories)
                        {
                            if (string.Equals(
                                    refDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar),
                                    approvedDir.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar),
                                    StringComparison.OrdinalIgnoreCase))
                            {
                                isApproved = true;
                                break;
                            }
                        }

                        if (isApproved)
                        {
                            Console.WriteLine($"Approved reference: {refPath}");
                        }
                        else
                        {
                            Console.WriteLine($"Unapproved reference detected: {refPath}");
                        }
                    }

                    if (!anyReference)
                    {
                        Console.WriteLine("No external references found in the drawing.");
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