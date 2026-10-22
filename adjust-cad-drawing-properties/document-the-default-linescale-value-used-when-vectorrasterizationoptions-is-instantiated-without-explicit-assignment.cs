using Aspose.CAD;
using System;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        // Create a concrete rasterization options object (CadRasterizationOptions)
        // without assigning LineScale. This will use the SDK's default value.
        CadRasterizationOptions rasterOptions = new CadRasterizationOptions();

        // Retrieve the default LineScale value.
        double defaultLineScale = rasterOptions.LineScale;

        // Display the default LineScale value.
        Console.WriteLine($"Default LineScale value: {defaultLineScale}");
    }
}
