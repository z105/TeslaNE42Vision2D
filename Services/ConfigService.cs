using Newtonsoft.Json;
using System;
using System.IO;

namespace TeslaNE42Vision2D.Services
{
    public class ConfigService<T> where T : new()
    {
        private readonly string _path;
        private readonly string _configName;

        public ConfigService(string configName)
        {
            _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configs");
            _configName = configName;
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
        }

        public T Config { get; set; } = new T();

        public void Load()
        {
            string filePath = Path.Combine(_path, _configName + ".json");
            if (File.Exists(filePath))
                Config = JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
            else
                Save();
        }

        public void Save()
        {
            string filePath = Path.Combine(_path, _configName + ".json");
            File.WriteAllText(filePath, JsonConvert.SerializeObject(Config, Formatting.Indented));
        }
    }
}
