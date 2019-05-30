using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

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
                configVals vals = JsonConvert.DeserializeObject<configVals>(json);
                sourcePath = vals.sourcePath;
                destinationPath = vals.destinationPath;
                Console.WriteLine(vals.destinationPath);
            }

            int total = Directory.GetFiles(sourcePath,"*.jpg").Length;
            int counter = 0;


            for (int i = 0; i < 10; i++)
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
                        string destinationFile = destinationPath + i.ToString() + @"\" + j.ToString() + @"\" + Path.GetFileName(f);
                        if (!File.Exists(destinationFile) && File.Exists(sourceFile))
                        {
                            File.Move(sourceFile, destinationFile);
                        }
                        counter++;
                        Console.WriteLine(f);
                        Console.WriteLine(counter.ToString() + @"/" + total + " complete");
                        decimal percent = (decimal)counter / (decimal)total;
                        Console.WriteLine(percent.ToString() + "%");
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
