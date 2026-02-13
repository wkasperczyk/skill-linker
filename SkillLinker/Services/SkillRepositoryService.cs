using SkillLinker.Domain.Models;

namespace SkillLinker.Services;

/// <summary>
/// Skill repository service implementation
/// </summary>
public class SkillRepositoryService : ISkillRepositoryService
{
    private readonly IConfigurationService _configService;

    public SkillRepositoryService(IConfigurationService configService)
    {
        _configService = configService;
    }

    public IEnumerable<Skill> GetAllSkills()
    {
        var config = _configService.GetConfiguration();
        var repositoryPath = config.SkillsRepositoryPath;

        if (!Directory.Exists(repositoryPath))
        {
            return Enumerable.Empty<Skill>();
        }

        var opencodePath = config.OpencodeSkillsPath;

        return Directory.GetDirectories(repositoryPath)
            .Select(dir => Path.GetFileName(dir))
            .Where(name => !string.IsNullOrEmpty(name))
            .Select(name => new Skill(
                Name: name,
                Description: GetSkillDescription(repositoryPath, name),
                Path: Path.Combine(repositoryPath, name),
                IsLinked: IsSkillLinked(opencodePath, name)
            ));
    }

    public Skill? GetSkillByName(string name) =>
        GetAllSkills().FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public bool SkillExists(string name) =>
        GetAllSkills().Any(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<Skill> GetLinkedSkills() =>
        GetAllSkills().Where(s => s.IsLinked);

    public IEnumerable<Skill> GetUnlinkedSkills() =>
        GetAllSkills().Where(s => !s.IsLinked);

    private static string GetSkillDescription(string repositoryPath, string skillName)
    {
        var readmePath = Path.Combine(repositoryPath, skillName, "SKILL.md");
        if (File.Exists(readmePath))
        {
            var lines = File.ReadLines(readmePath).Take(5);
            return string.Join(" ", lines).Trim();
        }
        return $"Skill: {skillName}";
    }

    private static bool IsSkillLinked(string opencodePath, string skillName)
    {
        var linkPath = Path.Combine(opencodePath, skillName);
        return File.Exists(linkPath) || (Directory.Exists(linkPath) && new DirectoryInfo(linkPath).LinkTarget != null);
    }
}
