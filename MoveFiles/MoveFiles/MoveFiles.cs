using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MoveFiles
{
    public class MoveExtracted : Extract
    {

        public void MoveJpgByName()
        {
            ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = 4 };
            int total = Directory.GetFiles(_sourceDirectory, "*.jpg").Length;
            for (int i = 0; i < 10; i++)
            {
                Regex reg = new Regex(@"\S*" + i.ToString() + ".jpg$");
                var files = Directory.GetFiles(_sourceDirectory, "*.jpg")
                     .Where(path => reg.IsMatch(path))
                     .ToList();

                for (int j = 0; j <= 10; j++)
                {
                    IEnumerable<string> subFiles = files.Where(f => f.EndsWith(j.ToString() + i.ToString() + ".jpg"));
                    Parallel.ForEach(files, options, f => {
                        string sourceFile = _sourceDirectory + Path.GetFileName(f);
                        string destinationFile = _destinationDirectory + i.ToString() + @"\" + j.ToString() + @"\" + Path.GetFileName(f);
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
        }

    }
}
