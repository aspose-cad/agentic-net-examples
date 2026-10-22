using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        // Path to the CAD file to be inspected
        string cadFilePath = "input.dwg";

        // Verify that the CAD file exists before attempting to load it
        if (!File.Exists(cadFilePath))
        {
            Console.Error.WriteLine($"CAD file not found: {cadFilePath}");
            return;
        }

        try
        {
            // Load the CAD file
            using (Image image = Image.Load(cadFilePath))
            {
                CadImage cadImage = (CadImage)image;

                // Create a diagnostic report file
                using (StreamWriter reportWriter = new StreamWriter("MissingExternalFilesReport.txt"))
                {
                    reportWriter.WriteLine("Diagnostic Report: Missing External Files");
                    reportWriter.WriteLine("Generated on: " + DateTime.Now);
                    reportWriter.WriteLine();

                    bool missingFound = false;

                    // Iterate over top‑level entities to locate insert objects (potential external references)
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        if (entity is CadInsertObject insertObject)
                        {
                            // Determine whether the referenced block definition exists in the drawing
                            bool blockExists = false;
                            foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                            {
                                if (block.Name == insertObject.Name)
                                {
                                    blockExists = true;
                                    break;
                                }
                            }

                            // If the block definition is absent, treat it as a missing external file
                            if (!blockExists)
                            {
                                missingFound = true;
                                reportWriter.WriteLine($"Missing external block reference: {insertObject.Name}");
                            }
                        }
                    }

                    if (!missingFound)
                    {
                        reportWriter.WriteLine("No missing external files were detected.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}