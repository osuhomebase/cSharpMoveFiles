﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpCompress.Common;
using SharpCompress.Archives;
using SharpCompress.Readers;
using SharpCompress.IO;
using SharpCompress.Readers.Tar;

namespace MoveFiles
{
    public class Extract
    {
        protected string _sourceDirectory;
        protected string _destinationDirectory;
        public string sourceDirectory
        {
            get { return _sourceDirectory; }
            set
            {
                DirectoryInfo dir = new DirectoryInfo(Path.GetFullPath(value));
                if (!dir.Exists)
                    throw new ArgumentException($"{nameof(value)} must be a valid directory. C'mon man!");
                _sourceDirectory = value;
            }
        }
        public string destinationDirectory
        {
            get { return _destinationDirectory; }
            set
            {
                DirectoryInfo dir = new DirectoryInfo(Path.GetFullPath(value));
                if (!dir.Exists)
                    throw new ArgumentException($"{nameof(value)} must be a valid directory. C'mon man!");
                _destinationDirectory = value;
            }
        }

        public Extract(string SourceDirectory, string DestinationDirectory)
        {
            sourceDirectory = SourceDirectory;
            destinationDirectory = DestinationDirectory;
        }

        public Extract(string SourceDirectory, string DestinationDirectory, string SourceFile)
        {
            sourceDirectory = SourceDirectory;
            destinationDirectory = DestinationDirectory;
        }

        public Extract()
        {
            
        }
        public void ExtractArchivedFiles()
        {
            ExtractArchivedFiles(false);
        }
        public void ExtractArchivedFiles(bool removeAfterConsumption)
        {

            IEnumerable<string> archives = Directory.GetFiles(_sourceDirectory).ToList<string>();
            // we only care about the tgz files dropped
            foreach (var path in archives.Where(p => Path.GetExtension(p) == ".tar"))
            {
                using (var files = File.OpenRead(path))
                using (var stream = File.OpenRead(path))
                using (var reader = TarReader.Open(stream))
                {
                    int i = 0;
                    while (reader.MoveToNextEntry())
                    {
                        if (!reader.Entry.IsDirectory)
                        {
                            using (var entryStream = reader.OpenEntryStream())
                            {
                                string file = Path.GetFileName(reader.Entry.Key);
                                string folder = Path.GetDirectoryName(reader.Entry.Key);
                                if (!Directory.Exists(_destinationDirectory))
                                {
                                    Directory.CreateDirectory(_destinationDirectory);
                                }
                                string destinationFileName = Path.Combine(_destinationDirectory, file);

                                using (FileStream fs = File.OpenWrite(destinationFileName))
                                {
                                    entryStream.CopyTo(fs);
                                }
                            }
                        }
                        i++;
                    }
                    files.Close();
                }
                if (removeAfterConsumption)
                {
                   while(FileIsLocked(path))
                    {
                        // wait half a second until the file is done extracting.. some async schtuff going on here somewhere.
                        System.Threading.Thread.Sleep(500);
                    }
                    File.Delete(path);
                }
            }
        }
        public void ExtractAllArchives()
        {
            ExtractAllArchives(false);
        }
        public void ExtractAllArchives(bool removeAfterConsumption)
        {

            IEnumerable<string> archives = Directory.GetFiles(_sourceDirectory).ToList<string>();
            // we only care about the tgz files dropped
            int i = 0;
            foreach (var path in archives.Where(p => Path.GetExtension(p) == ".tgz"))
            {
                using (var fs = File.OpenRead(path))
                using (var stream = new NonDisposingStream(fs, false))
                using (var archive = ArchiveFactory.Open(stream))
                {
                    foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory && entry.CompressionType == CompressionType.GZip))
                    {
                        // added unique filename prefix
                        entry.WriteToFile(Path.Combine(_destinationDirectory, i.ToString() + "_" + entry.Key));
                        i++;
                    }
                    fs.Close();
                }

                if (removeAfterConsumption)
                {
                    bool locked = FileIsLocked(path);
                    while (locked)
                    {

                        // wait half a second until the file is done extracting.. some async schtuff going on here somewhere.
                        System.Threading.Thread.Sleep(500);
                        locked = FileIsLocked(path);
                    }
                    File.Delete(path);
                }
            }

        }

        public void WriteFiles(IReader reader)
        {
            while (reader.MoveToNextEntry())
            {
                if (!reader.Entry.IsDirectory)
                {
                    reader.WriteEntryToDirectory(_destinationDirectory, new ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                }
            }
        }

        public static bool FileIsLocked(string strFullFileName)
        {
            bool blnReturn = false;
            FileStream fs;
            try
            {
                fs = File.Open(strFullFileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
                fs.Close();
            }
            catch (IOException ex)
            {
                blnReturn = true;
            }
            return blnReturn;
        }
    }
}
