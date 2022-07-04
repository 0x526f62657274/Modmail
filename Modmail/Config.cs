using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Modmail
{
    public class Modal
    {
        public string Title { get; set; }

        public string TextboxPlaceholder { get; set; }

        public string TextboxTitle { get; set; }
    }

    public class Embed
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ButtonName { get; set; }
    }

    public class Internal
    {
        public ulong? CreateModmailMessageId { get; set; }
    }

    public class Config
    {

        public string Token { get; set; }

        public ulong ModmailChannel { get; set; }

        public ulong ModPingId { get; set; }

        public Modal Modal { get; set; }

        public Internal Internal { get; set; }

        public Embed Embed { get; set; }


        private static Config? configCache;

        // so we don't have to load the config every time a value needs to be read from it
        public static Config GetCachedConfig()
        {
            if (configCache == null)
            {
                LoadConfig();
            }
            return configCache;
        }



        //loads config, forcing the cache to update
        public static Config LoadConfig()
        {
            string fileContents = File.ReadAllText("config.yml");
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var p = deserializer.Deserialize<Config>(fileContents);
            configCache = p;
            return p;
        }

        public static void SaveConfig(Config cfg)
        {
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var yml = serializer.Serialize(cfg);
            File.WriteAllText("config.yml", yml);
            LoadConfig(); // update the cache
        }

    }
}
