using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        const string inputPath = "input.txt";

        if (!File.Exists(inputPath))
        {
            // Create a minimal placeholder file with sample URLs
            File.WriteAllText(inputPath, "Visit https://example.com and http://test.org for more info.\nInvalid link: htp://bad");
        }

        try
        {
            var extractedUrls = new List<string>();
            foreach (var line in File.ReadAllLines(inputPath))
            {
                extractedUrls.AddRange(ExtractUrls(line));
            }

            foreach (var url in extractedUrls)
            {
                if (IsValidUrl(url))
                {
                    Console.WriteLine($"Valid URL: {url}");
                }
                else
                {
                    Console.WriteLine($"Malformed URL: {url}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Processing failed: {ex.Message}");
        }
    }

    static IEnumerable<string> ExtractUrls(string input)
    {
        if (string.IsNullOrEmpty(input))
            yield break;

        const string pattern = @"\b(?:https?://|www\.)\S+\b";
        foreach (Match match in Regex.Matches(input, pattern))
        {
            yield return match.Value;
        }
    }

    static bool IsValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri result) &&
               (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }
}