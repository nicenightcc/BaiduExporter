using System.Configuration;
using System.Linq;

namespace BaiduExporter
{
    public class Config
    {
        private Configuration config = null;
        public Config()
        {
            this.config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        }

        public void Save()
        {
            config.Save();
        }

        public int GetInt(string name)
        {
            if (config.AppSettings.Settings.AllKeys.Contains(name))
                return int.Parse(config.AppSettings.Settings[name].Value);
            else
                return 0;
        }

        public string Get(string name)
        {
            if (config.AppSettings.Settings.AllKeys.Contains(name))
                return config.AppSettings.Settings[name].Value;
            else
                return null;
        }

        public void Set(string name, object value)
        {
            if (config.AppSettings.Settings.AllKeys.Contains(name))
                config.AppSettings.Settings[name].Value = value.ToString();
            else
                config.AppSettings.Settings.Add(name, value.ToString());
        }
    }
}
