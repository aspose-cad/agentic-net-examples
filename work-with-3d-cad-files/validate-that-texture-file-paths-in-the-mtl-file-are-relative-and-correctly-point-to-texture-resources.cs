using System;
using System.IO;
using Aspose.CAD;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder input OBJ file path
            string inputObjPath = "input.obj";

            // Verify that the OBJ file exists
            if (!File.Exists(inputObjPath))
            {
                Console.Error.WriteLine($"Error: OBJ file not found at path \"{inputObjPath}\".");
                return;
            }

            // Load the OBJ file using Aspose.CAD
            using (Image cadImage = Image.Load(inputObjPath))
            {
                // Determine the directory of the OBJ file
                string objDirectory = Path.GetDirectoryName(Path.GetFullPath(inputObjPath)) ?? string.Empty;

                // Locate the mtllib declaration inside the OBJ file
                string mtlFileName = null;
                using (StreamReader objReader = new StreamReader(inputObjPath))
                {
                    string line;
                    while ((line = objReader.ReadLine()) != null)
                    {
                        if (line.StartsWith("mtllib ", StringComparison.OrdinalIgnoreCase))
                        {
                            // The rest of the line after "mtllib " is the material library file name
                            mtlFileName = line.Substring(7).Trim();
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(mtlFileName))
                {
                    Console.Error.WriteLine("Error: No mtllib declaration found in the OBJ file.");
                    return;
                }

                // Build full path to the .mtl file
                string mtlPath = Path.Combine(objDirectory, mtlFileName);

                if (!File.Exists(mtlPath))
                {
                    Console.Error.WriteLine($"Error: Material library file not found at path \"{mtlPath}\".");
                    return;
                }

                // Prepare output directory and report file
                string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "output");
                Directory.CreateDirectory(outputDir);
                string reportPath = Path.Combine(outputDir, "validation_report.txt");

                using (StreamWriter reportWriter = new StreamWriter(reportPath, false))
                {
                    reportWriter.WriteLine($"Validation Report for \"{mtlPath}\"");
                    reportWriter.WriteLine($"Generated on {DateTime.Now}");
                    reportWriter.WriteLine(new string('-', 50));

                    // Read and validate texture paths inside the .mtl file
                    using (StreamReader mtlReader = new StreamReader(mtlPath))
                    {
                        string mtlLine;
                        int lineNumber = 0;
                        while ((mtlLine = mtlReader.ReadLine()) != null)
                        {
                            lineNumber++;
                            string trimmed = mtlLine.Trim();

                            // Check for texture map lines (e.g., map_Kd, map_Ka, map_Bump, etc.)
                            if (trimmed.StartsWith("map_", StringComparison.OrdinalIgnoreCase))
                            {
                                // Split the line by whitespace; the last token is the texture file path
                                string[] parts = trimmed.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                if (parts.Length < 2)
                                {
                                    reportWriter.WriteLine($"Line {lineNumber}: Invalid map declaration.");
                                    continue;
                                }

                                string texturePathToken = parts[parts.Length - 1];

                                // Determine if the path is relative
                                bool isRelative = !Path.IsPathRooted(texturePathToken);
                                string validationMessage = isRelative ? "Relative" : "Absolute";

                                // Resolve the full path relative to the .mtl file location
                                string resolvedTexturePath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(mtlPath) ?? string.Empty, texturePathToken));

                                bool textureExists = File.Exists(resolvedTexturePath);
                                validationMessage += textureExists ? " and file exists." : " but file does NOT exist.";

                                reportWriter.WriteLine($"Line {lineNumber}: \"{texturePathToken}\" -> {validationMessage}");
                            }
                        }
                    }

                    reportWriter.WriteLine(new string('-', 50));
                    reportWriter.WriteLine("Validation completed.");
                }

                Console.WriteLine($"Validation report written to \"{reportPath}\".");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}