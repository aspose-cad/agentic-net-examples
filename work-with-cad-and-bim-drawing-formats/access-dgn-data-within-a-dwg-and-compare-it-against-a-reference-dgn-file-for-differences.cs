using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;
using Aspose.CAD.FileFormats.Cad.CadObjects.UnderlayDefinition;
using Aspose.CAD.FileFormats.Dgn;

class Program
{
    static int Main(string[] args)
    {
        if (args.Length != 0 && args.Length != 2)
        {
            Console.Error.WriteLine("Usage: example <drawing.dwg> <reference.dgn>");
            return 1;
        }

        string dwgPath = args.Length == 2 ? args[0] : "input.dwg";
        string referencePath = args.Length == 2 ? args[1] : "reference.dgn";
        if (!File.Exists(dwgPath) || !File.Exists(referencePath))
        {
            Console.Error.WriteLine("Input DWG or reference DGN file not found.");
            // A no-argument demonstration can explain how to supply inputs.
            // Explicitly requested comparisons must report missing inputs as failures.
            return args.Length == 0 ? 0 : 1;
        }

        try
        {
            dwgPath = Path.GetFullPath(dwgPath);
            referencePath = Path.GetFullPath(referencePath);
            string drawingDirectory = Path.GetDirectoryName(dwgPath);

            using (Image referenceImage = Image.Load(referencePath))
            {
                if (!(referenceImage is DgnImage reference))
                    throw new InvalidDataException("Reference input is not a DGN model.");
                using (Image drawingImage = Image.Load(dwgPath))
                {
                    if (!(drawingImage is CadImage drawing))
                        throw new InvalidDataException("Drawing input is not a native DWG/DXF model.");

                    // A DGN underlay refers to an external file. Its definition is
                    // an object in Objects; it is not native DWG geometry or a CadImage.
                    var definitions = drawing.Objects.OfType<CadDgnDefinition>()
                        .Where(d => !string.IsNullOrWhiteSpace(d.ObjectHandle))
                        .ToDictionary(d => d.ObjectHandle, StringComparer.OrdinalIgnoreCase);
                    var underlays = drawing.Entities.OfType<CadDgnUnderlay>().ToArray();
                    if (underlays.Length == 0)
                        throw new InvalidDataException("No DGN underlays found in the drawing entity collection.");

                    byte[] referenceHash = Hash(referencePath);
                    Dictionary<string, int> referenceTypes = EntityTypes(reference);
                    int compared = 0;
                    foreach (CadDgnUnderlay underlay in underlays)
                    {
                        string handle = underlay.AcDbUnderlayDefinitionObjectId;
                        if (string.IsNullOrWhiteSpace(handle) || !definitions.TryGetValue(handle, out CadDgnDefinition definition))
                            throw new InvalidDataException("DGN underlay has no matching definition object.");
                        string externalPath = definition.UnderlayPathAndFileName;
                        if (string.IsNullOrWhiteSpace(externalPath))
                            throw new InvalidDataException("DGN underlay definition has no external file path.");
                        // DWG references commonly contain Windows path separators,
                        // even when this utility is running on macOS or Linux.
                        externalPath = externalPath.Replace('\\', Path.DirectorySeparatorChar)
                            .Replace('/', Path.DirectorySeparatorChar);
                        // Relative references belong to the drawing's directory,
                        // independent of the process's current working directory.
                        externalPath = Path.GetFullPath(Path.IsPathRooted(externalPath)
                            ? externalPath : Path.Combine(drawingDirectory, externalPath));
                        if (!File.Exists(externalPath))
                            throw new FileNotFoundException("Referenced DGN file not found.", externalPath);

                        using (Image actualImage = Image.Load(externalPath))
                        {
                            if (!(actualImage is DgnImage actual))
                                throw new InvalidDataException("Referenced input is not a DGN model.");
                            Dictionary<string, int> actualTypes = EntityTypes(actual);
                            Console.WriteLine("Referenced DGN: " + externalPath);
                            Console.WriteLine("Reference SDK-visible entities: " + reference.Entities.Count());
                            Console.WriteLine("Referenced SDK-visible entities: " + actual.Entities.Count());
                            foreach (string type in actualTypes.Keys.Union(referenceTypes.Keys).OrderBy(t => t))
                            {
                                actualTypes.TryGetValue(type, out int actualCount);
                                referenceTypes.TryGetValue(type, out int referenceCount);
                                Console.WriteLine($"{type}: referenced={actualCount}, reference={referenceCount}");
                            }
                            bool sameTypes = actualTypes.Count == referenceTypes.Count &&
                                actualTypes.All(p => referenceTypes.TryGetValue(p.Key, out int count) && count == p.Value);
                            bool sameBytes = Hash(externalPath).SequenceEqual(referenceHash);
                            Console.WriteLine("SDK-visible entity type counts: " + (sameTypes ? "equal" : "different"));
                            Console.WriteLine("File bytes: " + (sameBytes ? "identical" : "different"));
                            Console.WriteLine("DGN versions: referenced=" + actual.Version + ", reference=" + reference.Version);
                            compared++;
                        }
                    }
                    Console.WriteLine("Compared DGN underlays: " + compared);
                    Console.WriteLine("Type counts describe SDK-visible entities and can be evaluation-limited; " +
                        "matching counts do not prove geometry equivalence. File hashes compare full file contents.");
                    // Read-only comparison: no drawing or referenced file is saved.
                    return 0;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("DGN comparison failed: " + ex.Message);
            return 1;
        }
    }

    static Dictionary<string, int> EntityTypes(DgnImage image)
    {
        return image.Entities.GroupBy(e => e.GetType().Name)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    static byte[] Hash(string path)
    {
        using (FileStream stream = File.OpenRead(path))
            return SHA256.HashData(stream);
    }
}
