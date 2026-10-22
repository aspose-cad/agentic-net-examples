using System;
using System.IO;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;

namespace CadExternalReferenceLister
{
    class Program
    {
        static void Main(string[] args)
        {
        try
        {
                    // Path to the CAD file to be inspected
                    string cadFilePath = "sample.dwg";

                    // Verify that the file exists before attempting to load it
                    if (!File.Exists(cadFilePath))
                    {
                        Console.WriteLine($"File not found: {cadFilePath}");
                        return;
                    }

                    // Load the CAD file using Aspose.CAD
                    using (Image image = Image.Load(cadFilePath))
                    {
                        // Cast the loaded image to CadImage to access CAD-specific members
                        CadImage cadImage = (CadImage)image;

                        // List external references from top‑level entities
                        ListExternalReferencesFromEntities(cadImage.Entities);

                        // List external references from block definitions
                        foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                        {
                            ListExternalReferencesFromEntities(block.Entities);
                        }
                    }
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}

        // Inspects a collection of CAD entities and writes external file paths with their originating entity type
        private static void ListExternalReferencesFromEntities(IEnumerable<CadEntityBase> entities)
        {
            foreach (CadEntityBase entity in entities)
            {
                // Check for inserted block references which may point to external files (Xrefs)
                if (entity is CadInsertObject insertObject)
                {
                    // The Name property typically holds the reference name or external file path
                    string externalPath = insertObject.Name ?? "<Unnamed>";
                    string entityType = insertObject.GetType().Name;

                    Console.WriteLine($"Entity Type: {entityType}, External Path: {externalPath}");
                }
            }
        }
    }
}