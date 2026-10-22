using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadHyperlinkReport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the CAD file (DWG/DXF)
                string cadFilePath = "input.dwg";

                // Verify that the file exists before attempting to load it
                if (!File.Exists(cadFilePath))
                {
                    Console.Error.WriteLine($"File not found: {cadFilePath}");
                    return;
                }

                // Load the CAD file
                using (Image image = Image.Load(cadFilePath))
                {
                    // Cast to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    Console.WriteLine("=== Top‑Level Entities Hyperlink Report ===");
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        string hyperlink = GetHyperlink(entity);
                        Console.WriteLine($"Entity Type: {entity.GetType().Name}, Layer: {entity.LayerName}, Hyperlink: {(string.IsNullOrEmpty(hyperlink) ? "None" : hyperlink)}");
                    }

                    Console.WriteLine();
                    Console.WriteLine("=== Block Entities Hyperlink Report ===");
                    foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                    {
                        Console.WriteLine($"Block Name: {block.Name}");
                        foreach (CadEntityBase entity in block.Entities)
                        {
                            string hyperlink = GetHyperlink(entity);
                            Console.WriteLine($"  Entity Type: {entity.GetType().Name}, Layer: {entity.LayerName}, Hyperlink: {(string.IsNullOrEmpty(hyperlink) ? "None" : hyperlink)}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }

        private static string GetHyperlink(CadEntityBase entity)
        {
            var hyperlinkProperty = entity.GetType().GetProperty("Hyperlink");
            if (hyperlinkProperty != null)
            {
                object value = hyperlinkProperty.GetValue(entity);
                return value as string;
            }
            return null;
        }
    }
}