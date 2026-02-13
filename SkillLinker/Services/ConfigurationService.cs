using System.Text.Json;
using SkillLinker.Domain.Models;

namespace SkillLinker.Services;

/// <summary>
/// Configuration service implementation
/// </summary>
public class ConfigurationService : IConfigurationService
{
    private readonly string _configPath;
    private AppConfiguration _config;

    public ConfigurationService()
    {
        _configPath = AppConfiguration.DefaultConfigFilePath;
        _config = LoadOrDefault();
        // Override the config's internal path with the actual path we're using
        _config = _config with { ConfigFilePath = _configPath };
    }

    private AppConfiguration LoadOrDefault()
    {
        if (File.Exists(_configPath))
        {
            try
            {
                var json = File.ReadAllText(_configPath);
                return JsonSerializer.Deserialize<AppConfiguration>(json) ?? AppConfiguration.Default;
            }
            catch
            {
                return AppConfiguration.Default;
            }
        }
        return AppConfiguration.Default;
    }

    public AppConfiguration GetConfiguration() => _config;

    public void SaveConfiguration(AppConfiguration config)
    {
        _config = config;
        var directory = Path.GetDirectoryName(_configPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_configPath, json);
    }

    public void UpdateRepositoryPath(string path) =>
        SaveConfiguration(_config with { SkillsRepositoryPath = path });

    public void UpdateOpencodePath(string path) =>
        SaveConfiguration(_config with { OpencodeSkillsPath = path });
}
