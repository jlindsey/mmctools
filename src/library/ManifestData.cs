using System;
using System.Linq;
using System.Collections.Generic;

namespace MMCTools
{
    public class ManifestData
    {
        public LatestVersions latest { get; set; }
        public List<MinecraftVersion> versions { get; set; }
        public MinecraftVersion LatestRelease
        {
            get
            {
                try
                {
                    var version = this.GetVersion(this.latest.release);
                    return version;
                }
                catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentNullException)
                {
                    return null;
                }
            }
        }
        public MinecraftVersion LatestSnapshot
        {
            get
            {
                try
                {
                    var version = this.GetVersion(this.latest.snapshot);
                    return version;
                }
                catch (Exception ex) when (ex is InvalidOperationException || ex is ArgumentNullException)
                {
                    return null;
                }
            }
        }

        public MinecraftVersion GetVersion(string id)
        {
            var version = (from v in this.versions
                           where v.id == id
                           select v).Single();

            return version;
        }
    }
}
