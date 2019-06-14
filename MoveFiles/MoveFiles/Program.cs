using System;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;

namespace MoveFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourcePath = @"";
            string destinationPath = @"";

            using (StreamReader r = new StreamReader("config.json"))
            {
                string json = r.ReadToEnd();
                configVals vals = JsonConvert.DeserializeObject<configVals>(json);
                sourcePath = vals.sourcePath;
                destinationPath = vals.destinationPath;
                Console.WriteLine(vals.destinationPath);
            }


            MoveExtracted Archived = new MoveExtracted { sourceDirectory = sourcePath, destinationDirectory = destinationPath };
            Archived.ExtractAllArchives(true);
            Archived.ExtractArchivedFiles(true);
            Archived.MoveJpgByName();
            


            var x = Console.ReadKey();
        }
        private class configVals
        {
            public string sourcePath { get; set; }
            public string destinationPath { get; set; }
            public string sourceTgzFile { get; set; }
        }
    }
}
