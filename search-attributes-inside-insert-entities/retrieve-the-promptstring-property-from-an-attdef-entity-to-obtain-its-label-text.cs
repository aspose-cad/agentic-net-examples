using System;
using System.IO;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadAttributeInspection
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the CAD file (DWG/DXF)
                string cadFilePath = "input.dwg";

                // Verify the input file exists
                if (!File.Exists(cadFilePath))
                {
                    Console.Error.WriteLine($"Input file not found: {cadFilePath}");
                    return;
                }

                // Load the CAD file using Aspose.CAD
                using (Image image = Image.Load(cadFilePath))
                {
                    // Cast the loaded image to CadImage to access CAD object model
                    CadImage cadImage = (CadImage)image;

                    // Iterate through top‑level drawing entities
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        // Identify ATTDEF entities by their type name
                        if (entity.TypeName != null && entity.TypeName.ToString().Equals("ATTDEF", StringComparison.OrdinalIgnoreCase))
                        {
                            // Attempt to read the PromptString property via reflection
                            PropertyInfo promptProp = entity.GetType().GetProperty("PromptString", BindingFlags.Public | BindingFlags.Instance);
                            if (promptProp != null)
                            {
                                object value = promptProp.GetValue(entity);
                                string promptText = value as string ?? string.Empty;
                                Console.WriteLine($"ATTDEF PromptString: {promptText}");
                            }
                            else
                            {
                                Console.WriteLine("ATTDEF PromptString property is not available in the current Aspose.CAD SDK version.");
                            }
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
}