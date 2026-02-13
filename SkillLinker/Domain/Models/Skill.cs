namespace SkillLinker.Domain.Models;

/// <summary>
/// Represents a skill in the repository
/// </summary>
public record Skill(
    string Name,
    string Description,
    string Path,
    bool IsLinked
);
