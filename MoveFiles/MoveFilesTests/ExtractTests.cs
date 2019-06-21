using System;
using Xunit;
using MoveFiles;
using System.Linq;
using System.IO;

namespace MoveFilesTests
{
    public class ExtractTests : TestBase
    {
        [Fact]
        public void ExtractBasics()
        {
            // test that invalid directories return exception
            _ = Assert.Throws<ArgumentException>(testCode: () => { Extract FailExtract = new Extract("foo", "bar"); });
            _ = Assert.Throws<ArgumentException>(testCode: () => {
                Extract FailExtract = new Extract
                {
                    destinationDirectory = "foo"
                };
            });
            _ = Assert.Throws<ArgumentException>(testCode: () => {
                Extract FailExtract = new Extract
                {
                    sourceDirectory = "bar"
                };
            });

            // test that valid directories are OK
            Extract TestExtract = new Extract(TEST_ARCHIVES_PATH,SCRATCH_FILES_PATH);
        }

        [Fact]
        public void ExtractAllArchives()
        {
            Extract TestExtract = new Extract(TEST_ARCHIVES_PATH, SCRATCH_FILES_PATH);
            TestExtract.ExtractAllArchives();
            Assert.True(File.Exists(Path.Combine(SCRATCH_FILES_PATH, "0_tar.tar")));
            // clean up
            File.Delete(Path.Combine(SCRATCH_FILES_PATH, "0_tar.tar"));
        }

        [Fact]
        public void ExtractArchivedFiles()
        {
            Extract TestExtract = new Extract(TEST_ARCHIVES_PATH, SCRATCH_FILES_PATH);
            TestExtract.ExtractArchivedFiles();

            var scratchFile = SCRATCH_FILES_PATH;
            var extracted =
                Directory.EnumerateFiles(SCRATCH_FILES_PATH, "*.*", SearchOption.AllDirectories)
                .ToLookup(path => Path.GetExtension(path));
            var original =
                Directory.EnumerateFiles(ORIGINAL_FILES_PATH, "*.*", SearchOption.AllDirectories)
                .ToLookup(path => Path.GetExtension(path));

            Assert.Equal(extracted.Count, original.Count);

            foreach (var orig in original)
            {
                Assert.True(extracted.Contains(orig.Key));

                CompareFilesByPath(orig.First(), extracted[orig.Key].First());
            }
        }

    }
}
