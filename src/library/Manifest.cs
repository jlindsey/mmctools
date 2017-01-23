using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace MMCTools
{
    public sealed class Manifest
    {
        public const string SOURCE_URI = "https://launchermeta.mojang.com/mc/game/version_manifest.json";

        public ManifestData Data { get; private set; }
        public MinecraftVersion LatestRelease { get { return this.Data.LatestRelease; } }
        public MinecraftVersion LatestSnapshot { get { return this.Data.LatestSnapshot; } }

        private HttpClient client;
        public HttpClient Client
        {
            get { return this.client; }

            set
            {
                if (this.client != null)
                    this.client.Dispose();

                this.client = value;
            }
        }

        public Manifest() : this(true) { }

        public Manifest(bool autoFetch)
        {
            this.client = new HttpClient();
            if (autoFetch)
                this.Fetch().Wait();
        }

        public async Task Fetch()
        {
            HttpResponseMessage resp = await this.Client.GetAsync(SOURCE_URI);
            resp.EnsureSuccessStatusCode();

            string bodyJson = await resp.Content.ReadAsStringAsync();
            this.Data = JsonConvert.DeserializeObject<ManifestData>(bodyJson);
        }

        public MinecraftVersion GetVersion(string id)
        {
            return this.Data.GetVersion(id);
        }
    }
}
