namespace SkillLinker.Domain.Models;

/// <summary>
/// Application configuration
/// </summary>
public record AppConfiguration(
    string SkillsRepositoryPath,
    string OpencodeSkillsPath,
    string ConfigFilePath
)
{
    public static string DefaultRepositoryPath =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "skill-linker", "skills");

    public static string DefaultOpencodeSkillsPath =>
        Path.Combine(Directory.GetCurrentDirectory(), ".opencode", "skills");

    public static string DefaultConfigFilePath =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "skill-linker", "config.json");

    public static AppConfiguration Default => new(
        DefaultRepositoryPath,
        DefaultOpencodeSkillsPath,
        DefaultConfigFilePath
    );
}
