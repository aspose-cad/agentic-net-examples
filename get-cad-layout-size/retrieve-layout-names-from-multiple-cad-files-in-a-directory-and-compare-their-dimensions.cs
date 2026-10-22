using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadLayoutComparer
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Input directory containing CAD files
                string inputDirectory = @"C:\CADFiles";

                if (!Directory.Exists(inputDirectory))
                {
                    Console.Error.WriteLine($"Directory does not exist: {inputDirectory}");
                    return;
                }

                // Supported CAD extensions
                string[] extensions = new[] { ".dwg", ".dxf", ".dgn", ".dwf", ".ifc", ".stl", ".obj", ".cf2", ".iges", ".step", ".plt" };
                List<string> cadFiles = new List<string>();

                foreach (string ext in extensions)
                {
                    cadFiles.AddRange(Directory.GetFiles(inputDirectory, "*" + ext, SearchOption.TopDirectoryOnly));
                }

                if (cadFiles.Count == 0)
                {
                    Console.Error.WriteLine("No CAD files found in the specified directory.");
                    return;
                }

                // Dictionary to hold layout dimensions per file
                // Key: file path, Value: dictionary of layout name -> (width, height)
                var allLayouts = new Dictionary<string, Dictionary<string, (float Width, float Height)>>();

                foreach (string filePath in cadFiles)
                {
                    using (CadImage cadImage = (CadImage)Image.Load(filePath))
                    {
                        var layoutsInfo = new Dictionary<string, (float Width, float Height)>();

                        // Iterate through each layout in the CAD file
                        foreach (CadLayout layout in cadImage.Layouts.Values)
                        {
                            string layoutName = layout.LayoutName ?? "Unnamed";

                            // Calculate dimensions using extents
                            double widthD = layout.MaxExtents.X - layout.MinExtents.X;
                            double heightD = layout.MaxExtents.Y - layout.MinExtents.Y;

                            float width = (float)widthD;
                            float height = (float)heightD;

                            layoutsInfo[layoutName] = (width, height);
                            Console.WriteLine($"File: {Path.GetFileName(filePath)} | Layout: {layoutName} | Width: {width} | Height: {height}");
                        }

                        allLayouts[filePath] = layoutsInfo;
                    }
                }

                // Compare dimensions across files
                Console.WriteLine("\nComparing layout dimensions across files:");

                // Gather all unique layout names
                var uniqueLayoutNames = new HashSet<string>();
                foreach (var fileLayouts in allLayouts.Values)
                {
                    foreach (string layoutName in fileLayouts.Keys)
                    {
                        uniqueLayoutNames.Add(layoutName);
                    }
                }

                // For each layout name, list dimensions per file
                foreach (string layoutName in uniqueLayoutNames)
                {
                    Console.WriteLine($"\nLayout: {layoutName}");
                    foreach (var fileEntry in allLayouts)
                    {
                        string fileName = Path.GetFileName(fileEntry.Key);
                        if (fileEntry.Value.TryGetValue(layoutName, out var dims))
                        {
                            Console.WriteLine($"  {fileName}: Width={dims.Width}, Height={dims.Height}");
                        }
                        else
                        {
                            Console.WriteLine($"  {fileName}: Layout not present");
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
}