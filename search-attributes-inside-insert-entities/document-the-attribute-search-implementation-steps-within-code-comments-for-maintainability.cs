using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

public class CadAttributeSearcher
{
    public static void Main(string[] args)
    {
        // ------------------------------------------------------------
        // Validate command‑line arguments
        // ------------------------------------------------------------
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: CadAttributeSearcher <input-cad-file> <output-directory>");
            return;
        }

        string inputFile = args[0];
        string outputDir = args[1];

        // ------------------------------------------------------------
        // Verify input file existence
        // ------------------------------------------------------------
        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"Input file not found: {inputFile}");
            return;
        }

        // ------------------------------------------------------------
        // Ensure the output directory exists
        // ------------------------------------------------------------
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // ------------------------------------------------------------
            // Load the CAD document
            // ------------------------------------------------------------
            using (CadImage cadImage = (CadImage)Image.Load(inputFile))
            {
                // ------------------------------------------------------------
                // ATTRIBUTE SEARCH IMPLEMENTATION STEPS
                // ------------------------------------------------------------
                // 1. Iterate over all block definitions stored in cadImage.BlockEntities.
                // 2. For each block, enumerate its child entities via CadBlockEntity.Entities.
                // 3. Identify attribute‑like entities. In Aspose.CAD the most common
                //    representation is CadText (or CadMText) whose text follows a
                //    convention (e.g., starts with “%”). Adjust the detection logic
                //    according to the specific CAD format you work with.
                // 4. When a matching entity is found, capture relevant details such as
                //    block name, attribute tag/value, and insertion point.
                // 5. Store the collected information for reporting or further processing.
                // ------------------------------------------------------------

                List<string> foundAttributes = new List<string>();

                // Iterate over block definitions
                foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                {
                    // Optional: obtain the block's name (if needed for reporting)
                    string blockName = block.Name ?? "UnnamedBlock";

                    // Enumerate entities inside the block
                    foreach (CadEntityBase entity in block.Entities)
                    {
                        // Detect CadText entities that may represent attributes
                        if (entity is CadText cadText)
                        {
                            // Simple heuristic: attribute text starts with "%"
                            if (!string.IsNullOrEmpty(cadText.DefaultValue) && cadText.DefaultValue.StartsWith("%"))
                            {
                                string info = $"{blockName}: {cadText.DefaultValue}";
                                foundAttributes.Add(info);
                            }
                        }
                        // Detect CadMText entities (multi‑line text) using the same heuristic
                        else if (entity is CadMText cadMText)
                        {
                            if (!string.IsNullOrEmpty(cadMText.Text) && cadMText.Text.StartsWith("%"))
                            {
                                string info = $"{blockName}: {cadMText.Text}";
                                foundAttributes.Add(info);
                            }
                        }
                    }
                }

                // ------------------------------------------------------------
                // Write the attribute report to a text file
                // ------------------------------------------------------------
                string reportPath = Path.Combine(outputDir, "AttributeReport.txt");
                File.WriteAllLines(reportPath, foundAttributes);
                Console.WriteLine($"Attribute report written to: {reportPath}");

                // ------------------------------------------------------------
                // OPTIONAL: Save the CAD document as PDF (preserving geometry)
                // ------------------------------------------------------------
                var rasterOptions = new CadRasterizationOptions
                {
                    // Correct enum spelling for centimeters
                    UnitType = UnitType.Centimenter,
                    LineScale = 1.0f
                };

                var pdfOptions = new PdfOptions
                {
                    VectorRasterizationOptions = rasterOptions
                };

                string pdfOutput = Path.Combine(outputDir,
                    Path.GetFileNameWithoutExtension(inputFile) + ".pdf");
                cadImage.Save(pdfOutput, pdfOptions);
                Console.WriteLine($"CAD file saved as PDF to: {pdfOutput}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing CAD file: {ex.Message}");
        }
    }
}