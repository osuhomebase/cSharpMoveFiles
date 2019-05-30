using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace MoveFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourcePath = @"C:\SourceCode\MoveFiles\SampleSource\";
            string destinationPath = @"C:\SourceCode\MoveFiles\SampleDestination\";

            using (StreamReader r = new StreamReader("config.json"))
            {
                string json = r.ReadToEnd();


            }


            for(int i = 0; i < 10; i++)
            {
                Regex reg = new Regex(@"\S*" + i.ToString() + ".jpg$");
                var files = Directory.GetFiles(sourcePath, "*.jpg")
                     .Where(path => reg.IsMatch(path))
                     .ToList();
                for(int j=0; j<=10; j++)
                {
                    IEnumerable<string> subFiles = files.Where(f => f.EndsWith(j.ToString() + i.ToString() + ".jpg"));

                    foreach (string f in subFiles)
                    {
                        string sourceFile = sourcePath + Path.GetFileName(f);
                        string destinationFile = destinationPath + i.ToString() + @"\" + Path.GetFileName(f);
                        File.Move(sourceFile, destinationFile);
                        Console.WriteLine(f);
                    }
                }
            }
            var x = Console.ReadKey();
        }
        private class configVals
        {
            public string sourcePath { get; set; }
            public string destinationPath { get; set; }
        }
    }
}
