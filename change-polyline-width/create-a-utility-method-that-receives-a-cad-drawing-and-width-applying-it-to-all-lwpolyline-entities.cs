using Aspose.CAD.FileFormats.Cad.CadObjects.Polylines;
using System;
using System.Collections.Generic;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadTables;
using Aspose.CAD.ImageOptions;
using System.IO;

namespace ChangePolylineWidth
{
    class Program
    {
        // Desired line weight (in hundredths of millimeters, as used by Aspose.CAD)
        const int DesiredLineWeight = 25; // 0.25 mm

        static void Main(string[] args)
        {
        try
        {
                    // Input and output file paths – adjust as needed.
                    string inputPath = "input.dwg";
            
                if (!File.Exists(inputPath))
                {
                    Console.WriteLine($"Input file not found: {inputPath}");
                    return;
                }
        string outputPath = "output.dwg";

                    // Load the CAD drawing.
                    using (Image img = Image.Load(inputPath))
                    {
                        // Cast to CadImage to access the object model.
                        CadImage cadImage = img as CadImage;
                        if (cadImage == null)
                        {
                            Console.WriteLine("Failed to load CAD image.");
                            return;
                        }

                        // Process top‑level entities.
                        ProcessEntityCollection(cadImage.Entities, cadImage);

                        // Process entities inside every block definition.
                        foreach (CadBlockEntity block in cadImage.BlockEntities.ValuesTyped)
                        {
                            ProcessEntityCollection(block.Entities, cadImage);
                        }

                        // Save the modified drawing preserving the original format.
                        var saveOptions = new CadRasterizationOptions(); // Options placeholder; not used for DWG save.
                        cadImage.Save(outputPath);
                        Console.WriteLine($"Polyline widths updated and saved to '{outputPath}'.");
                    }
        
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            return;
        }
}

        /// <summary>
        /// Recursively walks through a collection of CAD entities,
        /// updates the line weight of any polyline (lightweight or 3‑D),
        /// and descends into inserted blocks.
        /// </summary>
        static void ProcessEntityCollection(IEnumerable<CadEntityBase> entities, CadImage cadImage)
        {
            foreach (CadEntityBase entity in entities)
            {
                // Lightweight polyline (2‑D)
                if (entity is CadLwPolyline lwPolyline)
                {
                    // Set the line weight.
                    lwPolyline.LineWeight = DesiredLineWeight;
                }
                // 3‑D polyline
                else if (entity is CadPolyline poly3D)
                {
                    poly3D.LineWeight = DesiredLineWeight;
                }
                // Inserted block – need to process its definition.
                else if (entity is CadInsertObject insert)
                {
                    // Retrieve the block definition by name.
                    // The block name is stored in the OriginalBlockName property.
                    string blockName = insert.OriginalBlockName;
                    if (cadImage.BlockEntities.ContainsKey(blockName))
                    {
                        CadBlockEntity blockDef = cadImage.BlockEntities[blockName];
                        ProcessEntityCollection(blockDef.Entities, cadImage);
                    }
                }
                // Other entity types are ignored for this operation.
            }
        }
    }
}