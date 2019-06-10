using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpCompress.Common;
using SharpCompress.Archives;
using SharpCompress.Readers;
using SharpCompress.IO;


namespace MoveFiles
{
    public class Extract
    {
        private string _sourceDirectory;
        private string _destinationDirectory;
        private string _sourceFile;
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
        public string sourceFile
        {
            get { return _sourceFile; }
            set { _sourceDirectory = value; }
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
            sourceFile = SourceFile;
        }

        public Extract()
        {
            
        }

        public void ExtractAllArchives()
        {
            IEnumerable<string> archives = Directory.GetFiles(_sourceDirectory).ToList<string>();
            // we only care about the tgz files dropped
            int i = 0;
            foreach (var path in archives.Where(p => Path.GetExtension(p) == ".tgz"))
            {
                using (var stream = new NonDisposingStream(File.OpenRead(path), false))
                using (var archive = ArchiveFactory.Open(stream))
                {
                    foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory && entry.CompressionType == CompressionType.GZip))
                    {
                        var options = new ExtractionOptions { PreserveFileTime = false, Overwrite = true };
                        // added unique filename prefix
                        entry.WriteToFile(Path.Combine(_destinationDirectory, i.ToString() + "_" + entry.Key), options);
                        i++;
                    }
                }
            }
           
            //using (var stream = File.OpenRead(@"T:\Photos\Import\Imported\housingphotos.full.tar"))
            //using (var reader = TarReader.Open(stream))
            //{
            //    int i = 0;
            //    while (reader.MoveToNextEntry() && i < 10)
            //    {
            //        if (!reader.Entry.IsDirectory)
            //        {
            //            using (var entryStream = reader.OpenEntryStream())
            //            {
            //                string file = Path.GetFileName(reader.Entry.Key);
            //                string folder = Path.GetDirectoryName(reader.Entry.Key);
            //                string destdir = @"T:\Photos\Import\Imported\";
            //                if (!Directory.Exists(destdir))
            //                {
            //                    Directory.CreateDirectory(destdir);
            //                }
            //                string destinationFileName = Path.Combine(destdir, file);

            //                using (FileStream fs = File.OpenWrite(destinationFileName))
            //                {
            //                    entryStream.CopyTo(fs);
            //                }
            //            }
            //        }
            //        i++;
            //    }
            //}
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
    }
}
