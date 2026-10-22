using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad.CadConsts;

namespace CadMTextModifier
{
    class Program
    {
        static void Main(string[] args)
        {
            const string inputPath = "input.dxf";
            const string outputPath = "output.dxf";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                // Load the CAD file
                Image cadImage = Image.Load(inputPath);
                CadImage cad = (CadImage)cadImage;

                // Prepare a list to hold all modified entities
                List<CadEntityBase> modifiedEntities = new List<CadEntityBase>();

                // Iterate through each layout in the CAD drawing
                foreach (var layoutPair in cad.Layouts)
                {
                    CadLayout layout = layoutPair.Value;

                    // Process each entity in the current layout
                    foreach (CadEntityBase entity in layout.Entities)
                    {
                        // Check if the entity is an MText object
                        if (entity.TypeName == CadEntityTypeName.MTEXT)
                        {
                            // Cast to CadMText to access its properties
                            CadMText originalMText = (CadMText)entity;

                            // Apply custom logic – for example, convert the text to uppercase
                            string newText = originalMText.Text != null ? originalMText.Text.ToUpper() : string.Empty;

                            // Create a new CadMText with modified content while preserving other properties
                            CadMText modifiedMText = new CadMText()
                            {
                                Text = newText,
                                InsertionPoint = originalMText.InsertionPoint,
                                InitialTextHeight = originalMText.InitialTextHeight,
                                RotationAngleRad = originalMText.RotationAngleRad,
                                ReferenceRectangleWidth = originalMText.ReferenceRectangleWidth,
                                SoftOwner = originalMText.SoftOwner,
                                LayerName = originalMText.LayerName,
                                ColorId = originalMText.ColorId,
                                ColorValue = originalMText.ColorValue
                            };

                            // Add the modified MText to the collection
                            modifiedEntities.Add(modifiedMText);
                        }
                        else
                        {
                            // Non‑MText entities are added unchanged
                            modifiedEntities.Add(entity);
                        }
                    }
                }

                // Replace the image's entity collection with the modified list
                cad.Entities = modifiedEntities.ToArray();

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the modified drawing preserving editable objects
                DxfOptions saveOptions = new DxfOptions()
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Ascii
                };
                cad.Save(outputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}