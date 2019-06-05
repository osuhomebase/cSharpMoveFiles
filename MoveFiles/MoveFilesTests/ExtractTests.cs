using System;
using Xunit;
using MoveFiles;



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
    }
}
