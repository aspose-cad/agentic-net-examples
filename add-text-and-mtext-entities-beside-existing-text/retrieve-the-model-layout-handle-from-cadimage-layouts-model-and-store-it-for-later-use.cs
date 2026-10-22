using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;

class Program
{
    static void Main()
    {
        const string filePath = "sample.dwg";

        if (!File.Exists(filePath))
        {
            Console.Error.WriteLine($"File not found: {filePath}");
            return;
        }

        try
        {
            using (Image image = Image.Load(filePath))
            {
                var cadImage = (CadImage)image;

                // Retrieve the "Model" layout handle
                var modelLayout = cadImage.Layouts["Model"];

                // Attempt to get the layout name via reflection (if the property exists)
                var nameProp = modelLayout?.GetType().GetProperty("Name");
                var layoutName = nameProp?.GetValue(modelLayout) as string ?? "Unnamed";

                Console.WriteLine($"Model layout name: {layoutName}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"CAD processing failed: {ex.Message}");
        }
    }
}