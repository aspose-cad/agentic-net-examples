using System;
using System.IO;
using System.Collections;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source CAD file
            string inputPath = "input.dwg";

            // Verify that the source file exists
            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Input file '{inputPath}' not found.");
                return;
            }

            // Create an output directory for the report
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
            Directory.CreateDirectory(outputDir);
            string reportPath = Path.Combine(outputDir, "ExternalReferencesReport.txt");

            // Load the CAD file
            using (Image image = Image.Load(inputPath))
            {
                CadImage cadImage = image as CadImage;
                if (cadImage == null)
                {
                    Console.WriteLine("Loaded file is not a CAD image.");
                    return;
                }

                // Attempt to retrieve external references via reflection (property may not exist in current API)
                PropertyInfo externalRefsProp = cadImage.GetType().GetProperty("ExternalReferences");
                IEnumerable externalRefs = externalRefsProp?.GetValue(cadImage) as IEnumerable;

                using (StreamWriter writer = new StreamWriter(reportPath))
                {
                    writer.WriteLine($"External file references report for '{inputPath}':");
                    writer.WriteLine();

                    if (externalRefs != null)
                    {
                        foreach (object externalReference in externalRefs)
                        {
                            // Try to get the FilePath property via reflection
                            PropertyInfo filePathProp = externalReference.GetType().GetProperty("FilePath");
                            string referencePath = filePathProp?.GetValue(externalReference) as string ?? "Unknown";

                            bool exists = !string.IsNullOrEmpty(referencePath) && File.Exists(referencePath);
                            string status = exists ? "Resolved" : "Missing";

                            writer.WriteLine($"Reference: {referencePath}");
                            writer.WriteLine($"Status: {status}");
                            writer.WriteLine();
                        }
                    }
                    else
                    {
                        writer.WriteLine("External reference enumeration is not supported by the current Aspose.CAD API.");
                    }
                }
            }

            Console.WriteLine($"Report generated at: {reportPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}