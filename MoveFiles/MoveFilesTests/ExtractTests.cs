using System;
using Xunit;
using MoveFiles;
using System.Diagnostics;
using Microsoft.VisualStudio;



namespace MoveFilesTests
{
    public class ExtractTests : TestBase
    {
        [Fact]
        public void ExtractBasics()
        {

            // test that invalid directories return exception
            _ = Assert.Throws<ArgumentException>(testCode: () => { Extract TestExtract = new Extract("foo", "bar"); });
            _ = Assert.Throws<ArgumentException>(testCode: () => { Extract TestExtract = new Extract(); TestExtract.destinationDirectory = "foo"; });
            _ = Assert.Throws<ArgumentException>(testCode: () => { Extract TestExtract = new Extract(); TestExtract.sourceDirectory = "bar"; });

            // test that valid directories are OK

        }
    }
}
