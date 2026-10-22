using System;
using System.IO;
using System.Reflection;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static void Main()
    {
        try
        {
            // Input CAD file path (replace with actual path)
            string inputPath = "input.dwg";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the CAD file
            using (Image image = Image.Load(inputPath))
            {
                if (image is not CadImage cadImage)
                {
                    Console.Error.WriteLine("The loaded file is not a CAD image.");
                    return;
                }

                // Specify the layout name to inspect
                string layoutName = "Model"; // replace with the desired layout name

                CadLayout targetLayout = null;

                // Find the layout by its name
                foreach (CadLayout layout in cadImage.Layouts.Values)
                {
                    if (string.Equals(layout.LayoutName, layoutName, StringComparison.OrdinalIgnoreCase))
                    {
                        targetLayout = layout;
                        break;
                    }
                }

                if (targetLayout == null)
                {
                    Console.Error.WriteLine($"Layout '{layoutName}' not found in the CAD file.");
                    return;
                }

                // Iterate over entities in the layout and look for external references
                foreach (CadEntityBase entity in targetLayout.Entities)
                {
                    // Use the type name to identify possible external reference entities
                    string typeName = entity.GetType().Name;
                    if (typeName.IndexOf("ExternalReference", StringComparison.OrdinalIgnoreCase) >= 0 ||
                        typeName.IndexOf("Xref", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        // Try to get a 'Path' property via reflection
                        PropertyInfo pathProp = entity.GetType().GetProperty("Path", BindingFlags.Public | BindingFlags.Instance);
                        if (pathProp != null)
                        {
                            object value = pathProp.GetValue(entity);
                            if (value != null)
                            {
                                Console.WriteLine(value.ToString());
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}