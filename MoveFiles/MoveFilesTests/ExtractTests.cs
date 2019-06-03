using System;
using Xunit;
using MoveFiles;

namespace MoveFilesTests
{
    public class ExtractTests
    {
        [Fact]
        public void ExtractBasics()
        {
            _ = Assert.Throws<ArgumentException>(testCode: () => { Extract TestExtract = new Extract("foo", "bar"); });
        }
    }
}
