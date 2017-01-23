using System;
using System.Linq;
using System.Reflection;
using System.IO;
using MMCTools;
using RichardSzalay.MockHttp;
using Xunit;

namespace TestMMCTools
{
    public class ManifestTests
    {
        [Fact]
        public void TestData()
        {
            var manifest = this.setup(true);

            Assert.NotNull(manifest.Data);
            Assert.NotNull(manifest.Data.latest.release);
            Assert.NotEmpty(manifest.Data.versions);

            var version = (from v in manifest.Data.versions
                           where v.id == "1.10"
                           select v).FirstOrDefault();

            Assert.NotNull(version);
            Assert.Equal(version.type, "release");
            Assert.Equal(version.time, DateTime.Parse("2016-07-22T08:46:23+00:00"));
            Assert.Equal(version.releaseTime, DateTime.Parse("2016-06-08T13:06:18+00:00"));
            Assert.Equal(version.url, new Uri("https://launchermeta.mojang.com/mc/game/281697b6f88d757066b5f0427b40ffabc50e79b9/1.10.json"));
        }

        [Fact]
        public void TestLatestRelease()
        {
            var manifest = this.setup(true);
            var latest = manifest.LatestRelease;

            Assert.NotNull(latest);
            Assert.Equal(latest.id, "1.11.2");
        }

        [Fact]
        public void TestLatestSnapshot()
        {
            var manifest = this.setup(true);
            var latest = manifest.LatestSnapshot;

            Assert.NotNull(latest);
            Assert.Equal(latest.id, "16w50a");
        }

        private Manifest setup(bool autoFetch) {
            Assembly assembly = typeof(Manifest).GetTypeInfo().Assembly;
            var codeBaseURL = new Uri(assembly.CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseURL.AbsolutePath);
            string codeLoc = Path.GetDirectoryName(codeBasePath);
            string jsonOutput = File.ReadAllText(Path.Combine(codeLoc, "version_manifest.json"));

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.Expect(Manifest.SOURCE_URI)
                .Respond("application/json", jsonOutput);

            var client = mockHttp.ToHttpClient();
            var manifest = new Manifest(false);
            manifest.Client = client;
            if (autoFetch)
                manifest.Fetch().Wait();

            return manifest;
        }
    }
}
