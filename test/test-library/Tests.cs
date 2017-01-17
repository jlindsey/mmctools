using System;
using System.Linq;
using MMCTools;
using Xunit;

namespace TestMMCTools
{
    public class LibraryTests
    {
        [Fact]
        public void TestSingleton()
        {
            var manifest = Manifest.Instance;
            Assert.Equal(manifest, Manifest.Instance);
        }

        [Fact]
        public void TestData()
        {
            var manifest = Manifest.Instance;
            Assert.NotNull(manifest.data);
            Assert.NotNull(manifest.data.latest.release);
            Assert.NotEmpty(manifest.data.versions);

            var version = (from v in manifest.data.versions
                           where v.id == "1.10"
                           select v).FirstOrDefault();

            Assert.NotNull(version);
            Assert.Equal(version.type, "release");
            Assert.Equal(version.time, DateTime.Parse("2016-07-22T08:46:23+00:00"));
            Assert.Equal(version.releaseTime, DateTime.Parse("2016-06-08T13:06:18+00:00"));
            Assert.Equal(version.url, new Uri("https://launchermeta.mojang.com/mc/game/281697b6f88d757066b5f0427b40ffabc50e79b9/1.10.json"));
        }
    }
}
