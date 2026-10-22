using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadExternalReferenceReplacement
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Paths (replace placeholders as needed)
                string sourceCadPath = @"C:\CAD\source.dwg";
                string updatedReferencePath = @"C:\CAD\updated_reference.dwg";
                string outputPdfPath = @"C:\CAD\output.pdf";

                // Validate input files
                if (!File.Exists(sourceCadPath))
                {
                    Console.WriteLine($"Source CAD file not found: {sourceCadPath}");
                    return;
                }

                if (!File.Exists(updatedReferencePath))
                {
                    Console.WriteLine($"Updated reference CAD file not found: {updatedReferencePath}");
                    return;
                }

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPdfPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the source CAD drawing
                using (Image sourceImage = Image.Load(sourceCadPath))
                {
                    CadImage cadImage = sourceImage as CadImage;
                    if (cadImage == null)
                    {
                        Console.WriteLine("Failed to load source CAD image as CadImage.");
                        return;
                    }

                    // Load the updated external reference drawing
                    using (Image updatedRefImage = Image.Load(updatedReferencePath))
                    {
                        CadImage updatedRefCad = updatedRefImage as CadImage;
                        if (updatedRefCad == null)
                        {
                            Console.WriteLine("Failed to load updated reference as CadImage.");
                            return;
                        }

                        // Iterate over all top‑level entities in the source drawing
                        foreach (CadEntityBase entity in cadImage.Entities)
                        {
                            if (entity is CadInsertObject insertObj)
                            {
                                string originalBlockName = insertObj.OriginalBlockName;
                                if (!string.IsNullOrEmpty(originalBlockName) &&
                                    originalBlockName.EndsWith(".dwg", StringComparison.OrdinalIgnoreCase))
                                {
                                    // Remove existing block definition if present
                                    if (cadImage.BlockEntities.ContainsKey(originalBlockName))
                                    {
                                        cadImage.BlockEntities.Remove(originalBlockName);
                                    }

                                    // Try to find a matching block in the updated reference
                                    if (updatedRefCad.BlockEntities.TryGetValue(originalBlockName, out CadBlockEntity updatedBlock))
                                    {
                                        // Directly assign the matching block
                                        cadImage.BlockEntities[originalBlockName] = updatedBlock;
                                    }
                                    else if (updatedRefCad.BlockEntities.Count > 0)
                                    {
                                        // Fallback: use the first block from the updated reference
                                        foreach (KeyValuePair<string, CadBlockEntity> kvp in updatedRefCad.BlockEntities)
                                        {
                                            cadImage.BlockEntities[originalBlockName] = kvp.Value;
                                            break;
                                        }
                                    }

                                    // Update the insert object's name to reference the new block
                                    insertObj.Name = originalBlockName;
                                }
                            }
                        }
                    }

                    // Prepare PDF export options
                    PdfOptions pdfOptions = new PdfOptions();

                    // Export the modified CAD drawing to PDF
                    cadImage.Save(outputPdfPath, pdfOptions);
                }

                Console.WriteLine("PDF exported successfully to: " + outputPdfPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}