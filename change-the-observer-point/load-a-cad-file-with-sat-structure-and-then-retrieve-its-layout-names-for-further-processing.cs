using System;
using System.IO;
using System.Linq;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

class Program
{
    static int Main(string[] args)
    {
        if (args.Length > 1)
        {
            Console.Error.WriteLine("Usage: example <drawing-with-sat.dxf-or-dwg>");
            return 1;
        }

        string path = args.Length == 1 ? args[0] : "input.dxf";
        try
        {
            if (!File.Exists(path))
            {
                Console.Error.WriteLine("Input CAD file not found: " + path);
                return args.Length == 0 ? 0 : 1;
            }

            using (Image loaded = Image.Load(path))
            {
                if (!(loaded is CadImage drawing))
                    throw new InvalidDataException("Provide a native DWG/DXF containing SAT data. Standalone SAT does not expose native CAD layouts.");

                // Legacy text ACIS/SAT data is stored in groups 1 and 3.
                // The strings can be obfuscated by the CAD format. This sample
                // checks their presence; it does not decode or export SAT data.
                var objects = drawing.Entities.Concat(drawing.BlockEntities.Values
                    .Cast<CadBlockEntity>().SelectMany(block => block.Entities))
                    .OfType<CadObjectWithAcisData>()
                    .Where(entity => entity.ProprietaryDataParameterList != null &&
                        entity.ProprietaryDataParameterList.Any(parameter =>
                            (parameter.Key == CadEntityAttribute.Cad001 || parameter.Key == CadEntityAttribute.Cad003)
                            && !string.IsNullOrWhiteSpace(parameter.Value)))
                    .GroupBy(entity => entity.ObjectHandle)
                    .Select(group => group.First()).ToArray();
                if (objects.Length == 0)
                    throw new InvalidDataException("No text ACIS/SAT data was found in the loaded CAD objects. Binary-only ACIS and mesh data are outside this sample's scope.");

                string[] layoutNames = drawing.Layouts.Values
                    .Select(layout => layout.LayoutName).ToArray();
                if (layoutNames.Length == 0)
                    throw new InvalidDataException("The drawing has no native layout records.");

                Console.WriteLine("CAD objects containing text ACIS/SAT data: " + objects.Length);
                Console.WriteLine("Available layouts:");
                foreach (string name in layoutNames)
                    Console.WriteLine("- " + name);
                Console.WriteLine("Retrieved layout names: " + layoutNames.Length);
                // Read-only operation: do not save, replace or recreate a drawing.
                return 0;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Layout retrieval failed: " + ex.Message);
            return 1;
        }
    }
}
