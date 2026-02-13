namespace SkillLinker.Domain.Models;

/// <summary>
/// Represents a symbolic link between repository and target
/// </summary>
public record SkillLink(
    string SkillName,
    string SourcePath,
    string TargetPath,
    DateTime CreatedAt
);
