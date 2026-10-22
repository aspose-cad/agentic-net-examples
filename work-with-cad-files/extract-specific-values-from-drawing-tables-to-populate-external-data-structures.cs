using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadTableExtractionExample
{
    public class TableRowData
    {
        public string ColumnA { get; set; }
        public string ColumnB { get; set; }
        public string ColumnC { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Failed to cast loaded image to CadImage.");
                        return;
                    }

                    List<TableRowData> extractedRows = new List<TableRowData>();

                    foreach (var entity in cadImage.Entities)
                    {
                        // Aspose.CAD does not provide a dedicated table entity type.
                        // Attempt to identify a table by its runtime type name (if any).
                        string typeName = entity.GetType().Name;
                        if (typeName.IndexOf("Table", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            Console.Error.WriteLine("Table extraction is not supported by Aspose.CAD SDK.");
                            // No further processing possible.
                        }
                    }

                    if (extractedRows.Count == 0)
                    {
                        Console.WriteLine("No table data extracted.");
                    }
                    else
                    {
                        Console.WriteLine("Extracted Table Data:");
                        foreach (var row in extractedRows)
                        {
                            Console.WriteLine($"{row.ColumnA}\t{row.ColumnB}\t{row.ColumnC}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}