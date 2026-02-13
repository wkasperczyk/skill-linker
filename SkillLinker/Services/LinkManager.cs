using SkillLinker.Domain.Models;

namespace SkillLinker.Services;

/// <summary>
/// Link manager implementation
/// </summary>
public class LinkManager : ILinkManager
{
    private readonly IConfigurationService _configService;
    private readonly ISkillRepositoryService _skillRepository;

    public LinkManager(IConfigurationService configService, ISkillRepositoryService skillRepository)
    {
        _configService = configService;
        _skillRepository = skillRepository;
    }

    public bool CreateLink(string skillName)
    {
        var config = _configService.GetConfiguration();
        var skill = _skillRepository.GetSkillByName(skillName);

        if (skill == null)
            return false;

        if (!Directory.Exists(skill.Path))
            return false;

        var targetPath = Path.Combine(config.OpencodeSkillsPath, skillName);

        if (Directory.Exists(targetPath) || File.Exists(targetPath))
            return false;

        try
        {
            Directory.CreateDirectory(config.OpencodeSkillsPath);
            Directory.CreateSymbolicLink(targetPath, skill.Path);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool RemoveLink(string skillName)
    {
        var config = _configService.GetConfiguration();
        var targetPath = Path.Combine(config.OpencodeSkillsPath, skillName);

        if (!Directory.Exists(targetPath) && !File.Exists(targetPath))
            return false;

        try
        {
            if (Directory.Exists(targetPath))
            {
                var dirInfo = new DirectoryInfo(targetPath);
                if (!dirInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                    return false;
                Directory.Delete(targetPath);
            }
            else if (File.Exists(targetPath))
            {
                var fileInfo = new FileInfo(targetPath);
                if (!fileInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
                    return false;
                File.Delete(targetPath);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool IsLinked(string skillName)
    {
        var config = _configService.GetConfiguration();
        var targetPath = Path.Combine(config.OpencodeSkillsPath, skillName);
        return Directory.Exists(targetPath) || File.Exists(targetPath);
    }

    public IEnumerable<SkillLink> GetAllLinks()
    {
        var config = _configService.GetConfiguration();
        var opencodePath = config.OpencodeSkillsPath;

        if (!Directory.Exists(opencodePath))
            return Enumerable.Empty<SkillLink>();

        return Directory.GetDirectories(opencodePath)
            .Concat(Directory.GetFiles(opencodePath))
            .Select(path =>
            {
                var info = new DirectoryInfo(path);
                var target = info.LinkTarget;
                return new SkillLink(
                    SkillName: Path.GetFileName(path),
                    SourcePath: target ?? path,
                    TargetPath: path,
                    CreatedAt: info.CreationTime
                );
            });
    }
}
