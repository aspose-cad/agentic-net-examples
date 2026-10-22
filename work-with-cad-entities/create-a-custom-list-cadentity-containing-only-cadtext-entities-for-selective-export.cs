using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadConsts;
using Aspose.CAD.ImageOptions;

namespace CadTextExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source CAD file
                string sourcePath = "source.dxf";

                // Verify source file exists
                if (!File.Exists(sourcePath))
                {
                    Console.Error.WriteLine($"Source file not found: {sourcePath}");
                    return;
                }

                // Load the source CAD image
                CadImage sourceImage = (CadImage)Image.Load(sourcePath);

                // Prepare a list to hold only CadText entities
                List<CadEntityBase> textEntities = new List<CadEntityBase>();

                // Iterate through all layouts in the source image
                foreach (CadLayout layout in sourceImage.Layouts.Values)
                {
                    // Iterate through entities in the current layout
                    foreach (CadEntityBase entity in layout.Entities)
                    {
                        // Check if the entity is a CadText object
                        if (entity.TypeName == CadEntityTypeName.TEXT)
                        {
                            textEntities.Add(entity);
                        }
                    }
                }

                // Create a new CAD image that will contain only the collected text entities
                CadImage textOnlyImage = new CadImage();

                // Replace the Entities collection with the filtered text entities
                textOnlyImage.Entities = textEntities.ToArray();

                // Define save options to preserve editable objects and set file format
                DxfOptions saveOptions = new DxfOptions
                {
                    OutputMode = CadOutputMode.Convert,
                    DxfFileFormat = CadFileFormat.Ascii
                };

                // Path for the output CAD file containing only text entities
                string outputPath = "texts_only.dxf";

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the new CAD image
                textOnlyImage.Save(outputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
                return;
            }
        }
    }
}