﻿using System.Collections.Generic;
using System.IO;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Readers.Tar;
using StreamHelpers;
using Xunit;
using System.Diagnostics;

namespace MoveFilesTests
{
    /// <summary>
    /// Class <c>TarReaderTests</c> borrowed from https://github.com/adamhathcock/sharpcompress/blob/master/tests/SharpCompress.Test/Tar/TarReaderTests.cs
    /// </summary>
    public class TarReaderTests : ReaderTests
    {
        public TarReaderTests()
        {
            UseExtensionInsteadOfNameToVerify = true;
        }

        [Fact]
        public void Tar_Reader()
        {
            Read("tar.tar", CompressionType.None);
        }

        [Fact]
        public void Tar_Skip()
        {
            using (Stream stream = new ForwardOnlyStream(File.OpenRead(Path.Combine(TEST_ARCHIVES_PATH, "Tar.tar"))))
            using (IReader reader = ReaderFactory.Open(stream))
            {
                int x = 0;
                while (reader.MoveToNextEntry())
                {
                    if (!reader.Entry.IsDirectory)
                    {
                        x++;
                        if (x % 2 == 0)
                        {
                            reader.WriteEntryToDirectory(SCRATCH_FILES_PATH,
                                                         new ExtractionOptions()
                                                         {
                                                             ExtractFullPath = true,
                                                             Overwrite = true
                                                         });
                        }
                    }
                }
            }
        }

        

        [Fact]
        public void Tar_LongNamesWithLongNameExtension()
        {
            var filePaths = new List<string>();

            using (Stream stream = File.OpenRead(Path.Combine(TEST_ARCHIVES_PATH, "Tar.LongPathsWithLongNameExtension.tar")))
            using (var reader = TarReader.Open(stream))
            {
                while (reader.MoveToNextEntry())
                {
                    if (!reader.Entry.IsDirectory)
                    {
                        filePaths.Add(reader.Entry.Key);
                    }
                }
            }

            Assert.Equal(30, filePaths.Count);
            Assert.Contains("N33333333.jpg", filePaths);
            Assert.Contains("wp-content/plugins/gravityformsextend/lib/Aws/Symfony/Component/ClassLoader/Tests/Fixtures/Apc/beta/Apc/ApcPrefixCollision/A/B/N77777777.jpg", filePaths);
            Assert.Contains("wp-content/plugins/gravityformsextend/lib/Aws/Symfony/Component/ClassLoader/Tests/Fixtures/Apc/beta/Apc/ApcPrefixCollision/A/B/N88888888.jpg", filePaths);
            Assert.Contains("wp-content/plugins/gravityformsextend/lib/Aws/Symfony/Component/ClassLoader/Tests/Fixtures/Apc/beta/Apc/ApcPrefixCollision/A/B/N99999999.jpg", filePaths);
        }

    }
}
