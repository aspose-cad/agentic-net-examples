using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadTextExport
{
    class Program
    {
        static void Main(string[] args)
        {
            const string sourcePath = "source.dwg";
            const string outputPath = "annotations.dxf";

            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            try
            {
                // Load the source CAD file
                using (Image sourceImage = Image.Load(sourcePath))
                {
                    // Cast to CadImage to access CAD-specific members
                    CadImage cadSource = (CadImage)sourceImage;

                    // Get the first layout to use its BlockTableRecordHandle for SoftOwner
                    CadLayout firstLayout = cadSource.Layouts.Values.First();

                    // Filter only text entities (CadText and CadMText) and set SoftOwner
                    var textEntities = cadSource.Entities
                        .Where(e => e is CadText || e is CadMText)
                        .Select(e =>
                        {
                            if (e is CadText txt)
                            {
                                txt.SoftOwner = firstLayout.BlockTableRecordHandle;
                                return (CadEntityBase)txt;
                            }
                            else if (e is CadMText mtxt)
                            {
                                mtxt.SoftOwner = firstLayout.BlockTableRecordHandle;
                                return (CadEntityBase)mtxt;
                            }
                            return null;
                        })
                        .Where(e => e != null)
                        .ToArray();

                    // Create a new CadImage to hold only the text entities
                    CadImage cadResult = new CadImage
                    {
                        Entities = textEntities
                    };

                    // Save the result as DXF preserving editable objects
                    DxfOptions dxfOptions = new DxfOptions
                    {
                        OutputMode = CadOutputMode.Convert
                    };
                    cadResult.Save(outputPath, dxfOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
            }
        }
    }
}