using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Readers.Tar;

namespace MoveFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourcePath = @"C:\SourceCode\MoveFiles\SampleSource\";
            string destinationPath = @"C:\SourceCode\MoveFiles\SampleDestination\";
            ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = 4 };

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
                    Parallel.ForEach(files, options, f => {
                        string sourceFile = sourcePath + Path.GetFileName(f);
                        string destinationFile = destinationPath + i.ToString() + @"\" + j.ToString() + @"\" + Path.GetFileName(f);
                        if (!File.Exists(destinationFile))
                        {
                            if (File.Exists(sourceFile))
                                try
                                {
                                    File.Move(sourceFile, destinationFile);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex);
                                }
                        }
                        else if (File.Exists(sourceFile))
                            try
                            {
                                File.Delete(sourceFile);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        Console.WriteLine($"Processing {f} on thread {Thread.CurrentThread.ManagedThreadId}");
                        //close lambda expression and method invocation
                       

                    });

                }
            }

            using (var stream = File.OpenRead(@"T:\Photos\Import\Imported\housingphotos.full.20190525.tgz"))
            using (var archive = ArchiveFactory.Open(stream))
            {
                var entry = archive.Entries.First();
                entry.WriteToFile(Path.Combine(@"T:\Photos\Import\Imported\", entry.Key));
            }
            using (var stream = File.OpenRead(@"T:\Photos\Import\Imported\housingphotos.full.tar"))
            using (var reader = TarReader.Open(stream))
            {
                int i = 0;
                while (reader.MoveToNextEntry() && i < 10)
                {
                    if (!reader.Entry.IsDirectory)
                    {
                        using (var entryStream = reader.OpenEntryStream())
                        {
                            string file = Path.GetFileName(reader.Entry.Key);
                            string folder = Path.GetDirectoryName(reader.Entry.Key);
                            string destdir = @"T:\Photos\Import\Imported\";
                            if (!Directory.Exists(destdir))
                            {
                                Directory.CreateDirectory(destdir);
                            }
                            string destinationFileName = Path.Combine(destdir, file);

                            using (FileStream fs = File.OpenWrite(destinationFileName))
                            {
                                entryStream.CopyTo(fs);
                            }
                        }
                    }
                    i++;
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
