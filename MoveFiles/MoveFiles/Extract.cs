using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Readers.Tar;

namespace MoveFiles
{
    public class Extract
    {
        private string _sourceDirectory;
        private string _destinationDirectory;
        public string sourceDirectory
        {
            get { return _sourceDirectory; }
            set {
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

        public Extract()
        {
            //using (var stream = File.OpenRead(@"T:\Photos\Import\Imported\housingphotos.full.20190525.tgz"))
            //using (var archive = ArchiveFactory.Open(stream))
            //{
            //    var entry = archive.Entries.First();
            //    entry.WriteToFile(Path.Combine(@"T:\Photos\Import\Imported\", entry.Key));
            //}
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
    }
}
