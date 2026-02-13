using SkillLinker.Domain.Models;

namespace SkillLinker.Services;

/// <summary>
/// Configuration service interface
/// </summary>
public interface IConfigurationService
{
    AppConfiguration GetConfiguration();
    void SaveConfiguration(AppConfiguration config);
    void UpdateRepositoryPath(string path);
    void UpdateOpencodePath(string path);
}
