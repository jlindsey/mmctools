using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace MMCTools
{
    public struct LatestVersions
    {
        public string snapshot;
        public string release;
    }

    public struct MinecraftVersion
    {
        public string id;
        public string type;
        public DateTime time;
        public DateTime releaseTime;
        public Uri url;
    }

    public struct ManifestData
    {
        public LatestVersions latest;
        public List<MinecraftVersion> versions;
    }

    public sealed class Manifest
    {
        private const string sourceURI = "https://launchermeta.mojang.com/mc/game/version_manifest.json";
        private static volatile Manifest instance;
        private static object syncRoot = new Object();

        public static Manifest Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Manifest();
                    }
                }

                return instance;
            }
        }

        public ManifestData data
        {
            get;
            private set;
        }

        private Manifest()
        {
            this.fetch();
        }

        public async void fetch()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage resp = await client.GetAsync(sourceURI);
            resp.EnsureSuccessStatusCode();

            string bodyJson = await resp.Content.ReadAsStringAsync();
            this.data = JsonConvert.DeserializeObject<ManifestData>(bodyJson);
        }
    }
}
