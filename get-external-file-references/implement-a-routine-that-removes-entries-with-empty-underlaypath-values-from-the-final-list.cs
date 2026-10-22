using System;
using System.Collections.Generic;

namespace UnderlayPathFilter
{
    // Represents an entry that may contain an underlay path.
    public class UnderlayEntry
    {
        public string UnderlayPath { get; set; }

        public UnderlayEntry(string underlayPath)
        {
            UnderlayPath = underlayPath;
        }
    }

    class Program
    {
        // Removes entries with empty or whitespace UnderlayPath values from the provided list.
        static void RemoveEmptyUnderlayPaths(List<UnderlayEntry> entries)
        {
            // RemoveAll modifies the list in place, deleting any entry whose UnderlayPath is null, empty, or whitespace.
            entries.RemoveAll(entry => string.IsNullOrWhiteSpace(entry.UnderlayPath));
        }

        static void Main(string[] args)
        {
            // Sample data with various UnderlayPath values.
            List<UnderlayEntry> entryList = new List<UnderlayEntry>
            {
                new UnderlayEntry(@"C:\Underlays\file1.dwf"),
                new UnderlayEntry(string.Empty),
                new UnderlayEntry(null),
                new UnderlayEntry(@"C:\Underlays\file2.dwf"),
                new UnderlayEntry("   ")
            };

            // Display count before filtering.
            Console.WriteLine($"Entries before filtering: {entryList.Count}");

            // Invoke the routine to remove empty UnderlayPath entries.
            RemoveEmptyUnderlayPaths(entryList);

            // Display count after filtering.
            Console.WriteLine($"Entries after filtering: {entryList.Count}");

            // List remaining entries.
            foreach (UnderlayEntry entry in entryList)
            {
                Console.WriteLine($"Remaining UnderlayPath: {entry.UnderlayPath}");
            }
        }
    }
}