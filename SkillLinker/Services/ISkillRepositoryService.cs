using SkillLinker.Domain.Models;

namespace SkillLinker.Services;

/// <summary>
/// Skill repository service interface
/// </summary>
public interface ISkillRepositoryService
{
    IEnumerable<Skill> GetAllSkills();
    Skill? GetSkillByName(string name);
    bool SkillExists(string name);
    IEnumerable<Skill> GetLinkedSkills();
    IEnumerable<Skill> GetUnlinkedSkills();
}
