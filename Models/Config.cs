using Newtonsoft.Json;
using System.IO;

namespace SupremeXE_UI
{
    public partial class Config
    {
        private static Config instance;
        private Config() { }
        public static Config GetInstance()
        {
            if (instance == null)
            {
                try
                {
                    instance = JsonConvert.DeserializeObject<Config>(File.ReadAllText(@"config.json"));
                }
                catch (IOException e)
                {
                    throw (e);
                }
            }
            return instance;
        }
    }

    public partial class Config
    {
        public string BrowserPath { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public Links Links { get; set; }
    }

    public class Links
    {
        public string StoreMain { get; set; }
        public string StoreCart { get; set; }
        public string StoreCheckout { get; set; }
        public string DroplistHostname { get; set; }
        public string Droplist { get; set; }
    }
}