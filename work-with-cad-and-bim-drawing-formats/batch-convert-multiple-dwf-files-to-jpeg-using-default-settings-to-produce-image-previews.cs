using System;
using System.IO;
using Aspose.CAD;
using Aspose.CAD.ImageOptions;

class Program
{
    static void Main()
    {
        try
        {
            string inputFolderPath = "InputDwfFiles";
            string outputFolderPath = "OutputJpegPreviews";

            if (!Directory.Exists(inputFolderPath))
            {
                Console.Error.WriteLine($"Input folder does not exist: {inputFolderPath}");
                return;
            }

            if (!Directory.Exists(outputFolderPath))
            {
                Directory.CreateDirectory(outputFolderPath);
            }

            string[] dwfFiles = Directory.GetFiles(inputFolderPath, "*.dwf", SearchOption.TopDirectoryOnly);
            if (dwfFiles.Length == 0)
            {
                Console.WriteLine("No DWF files found to convert.");
                return;
            }

            foreach (string dwfFilePath in dwfFiles)
            {
                using (Image cadImage = Image.Load(dwfFilePath))
                {
                    JpegOptions jpegOptions = new JpegOptions();

                    string fileNameWithoutExt = Path.GetFileNameWithoutExtension(dwfFilePath);
                    string jpegFilePath = Path.Combine(outputFolderPath, fileNameWithoutExt + ".jpg");

                    cadImage.Save(jpegFilePath, jpegOptions);
                    Console.WriteLine($"Converted '{dwfFilePath}' to '{jpegFilePath}'.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}