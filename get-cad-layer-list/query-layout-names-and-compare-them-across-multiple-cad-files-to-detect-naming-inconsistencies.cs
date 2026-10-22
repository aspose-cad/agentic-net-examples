using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

namespace LayoutComparisonSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input CAD files (replace with actual paths)
                string[] inputFiles = new string[]
                {
                    "Input/file1.dwg",
                    "Input/file2.dwg",
                    "Input/file3.dwg"
                };

                // Verify that all input files exist
                List<string> missingFiles = inputFiles.Where(path => !File.Exists(path)).ToList();
                if (missingFiles.Count > 0)
                {
                    Console.Error.WriteLine("The following input files were not found:");
                    foreach (string missing in missingFiles)
                    {
                        Console.Error.WriteLine(missing);
                    }
                    return;
                }

                // Dictionary to hold layout names for each file
                Dictionary<string, List<string>> layoutsPerFile = new Dictionary<string, List<string>>();

                // Load each CAD file and collect its layout names
                foreach (string filePath in inputFiles)
                {
                    using (Image image = Image.Load(filePath))
                    {
                        CadImage cadImage = (CadImage)image;
                        // CadImage.Layouts is a dictionary; layout names are the keys
                        List<string> layoutNames = cadImage.Layouts.Keys.ToList();
                        layoutsPerFile.Add(Path.GetFileName(filePath), layoutNames);
                    }
                }

                // Gather all unique layout names across the files
                HashSet<string> allLayoutNames = new HashSet<string>(layoutsPerFile.SelectMany(kv => kv.Value));

                // Prepare report lines
                List<string> reportLines = new List<string>
                {
                    "Layout Naming Consistency Report",
                    $"Generated on {DateTime.Now}",
                    string.Empty
                };

                // Compare layout presence across files
                foreach (string layoutName in allLayoutNames)
                {
                    List<string> presentIn = layoutsPerFile
                        .Where(kv => kv.Value.Contains(layoutName))
                        .Select(kv => kv.Key)
                        .ToList();

                    List<string> missingIn = layoutsPerFile
                        .Where(kv => !kv.Value.Contains(layoutName))
                        .Select(kv => kv.Key)
                        .ToList();

                    if (missingIn.Count == 0)
                    {
                        reportLines.Add($"Layout \"{layoutName}\" is present in all files.");
                    }
                    else
                    {
                        reportLines.Add($"Layout \"{layoutName}\" is missing in: {string.Join(", ", missingIn)}");
                    }
                }

                // Output report to console
                foreach (string line in reportLines)
                {
                    Console.WriteLine(line);
                }

                // Write report to a file in the Output directory
                string outputDir = "Output";
                Directory.CreateDirectory(outputDir);
                string reportPath = Path.Combine(outputDir, "layout_report.txt");
                File.WriteAllLines(reportPath, reportLines);
                Console.WriteLine($"Report written to {reportPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}