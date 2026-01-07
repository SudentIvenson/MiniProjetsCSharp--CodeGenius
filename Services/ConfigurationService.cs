namespace DefaultNamespace;

using System;
using System.IO;
using System.Text.Json;

public class AppConfig
{
    public string DateFormat { get; set; } = "yyyy-MM-dd HH:mm";
    public bool EnablePersistence { get; set; } = false;
}

public class ConfigurationService
{
    private readonly string _path;
    private AppConfig _config;

    public ConfigurationService(string path = "config.json")
    {
        _path = path;
        _config = Load() ?? new AppConfig();
    }

    public AppConfig GetConfig() => _config;

    public void Update(Action<AppConfig> updater)
    {
        updater?.Invoke(_config);
    }

    public AppConfig? Load()
    {
        try
        {
            if (!File.Exists(_path)) return null;
            var json = File.ReadAllText(_path);
            var cfg = JsonSerializer.Deserialize<AppConfig>(json);
            if (cfg != null) _config = cfg;
            return cfg;
        }
        catch
        {
            return null;
        }
    }

    public void Save()
    {
        var opts = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(_config, opts);
        File.WriteAllText(_path, json);
    }
}

