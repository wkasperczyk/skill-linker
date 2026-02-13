using SkillLinker.Domain.Models;

namespace SkillLinker.Services;

/// <summary>
/// Link manager service interface
/// </summary>
public interface ILinkManager
{
    bool CreateLink(string skillName);
    bool RemoveLink(string skillName);
    bool IsLinked(string skillName);
    IEnumerable<SkillLink> GetAllLinks();
}
