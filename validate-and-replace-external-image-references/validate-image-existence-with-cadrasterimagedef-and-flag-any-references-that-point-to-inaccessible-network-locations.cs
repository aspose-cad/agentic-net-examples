using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.FileFormats.Cad;
using Aspose.CAD.FileFormats.Cad.CadObjects;

namespace CadRasterImageValidator
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath = "input.dwg";

            try
            {
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Error: Input file '{inputPath}' does not exist.");
                    return;
                }

                using (Image image = Image.Load(inputPath))
                {
                    CadImage cadImage = image as CadImage;
                    if (cadImage == null)
                    {
                        Console.Error.WriteLine("Error: Loaded file is not a CAD image.");
                        return;
                    }

                    // Validate CadRasterImageDef objects
                    foreach (CadBaseObject baseObj in cadImage.Objects)
                    {
                        CadRasterImageDef rasterDef = baseObj as CadRasterImageDef;
                        if (rasterDef != null)
                        {
                            string fileName = rasterDef.FileName;
                            bool exists = false;
                            try
                            {
                                exists = File.Exists(fileName);
                            }
                            catch
                            {
                                exists = false;
                            }

                            if (!exists)
                            {
                                Console.WriteLine($"Warning: Raster image definition '{rasterDef.ObjectHandle}' references missing file '{fileName}'.");
                            }
                        }
                    }

                    // Validate CadRasterImage entities referencing definitions
                    foreach (CadEntityBase entity in cadImage.Entities)
                    {
                        CadRasterImage rasterImage = entity as CadRasterImage;
                        if (rasterImage != null)
                        {
                            string refHandle = rasterImage.ImageDefReference;
                            CadRasterImageDef referencedDef = null;

                            foreach (CadBaseObject obj in cadImage.Objects)
                            {
                                CadRasterImageDef def = obj as CadRasterImageDef;
                                if (def != null && def.ObjectHandle == refHandle)
                                {
                                    referencedDef = def;
                                    break;
                                }
                            }

                            if (referencedDef == null)
                            {
                                Console.WriteLine($"Warning: Raster image entity references undefined definition handle '{refHandle}'.");
                            }
                            else
                            {
                                string fileName = referencedDef.FileName;
                                bool exists = false;
                                try
                                {
                                    exists = File.Exists(fileName);
                                }
                                catch
                                {
                                    exists = false;
                                }

                                if (!exists)
                                {
                                    Console.WriteLine($"Warning: Raster image entity references missing file '{fileName}'.");
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
}