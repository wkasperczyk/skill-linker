using SkillLinker.Domain.Models;
using SkillLinker.Services;

namespace SkillLinker.Tests;

public class ModelTests
{
    [Fact]
    public void Skill_PropertiesAreSetCorrectly()
    {
        // Arrange & Act
        var skill = new Skill("test-skill", "Test description", "/path/to/skill", true);

        // Assert
        Assert.Equal("test-skill", skill.Name);
        Assert.Equal("Test description", skill.Description);
        Assert.Equal("/path/to/skill", skill.Path);
        Assert.True(skill.IsLinked);
    }

    [Fact]
    public void SkillRecord_IsImmutable()
    {
        // Arrange
        var skill = new Skill("test", "desc", "/path", false);

        // Act - Create new instance with with expression
        var updated = skill with { IsLinked = true };

        // Assert
        Assert.False(skill.IsLinked);
        Assert.True(updated.IsLinked);
    }

    [Fact]
    public void SkillLink_PropertiesAreSetCorrectly()
    {
        // Arrange & Act
        var createdAt = DateTime.Now;
        var link = new SkillLink("my-skill", "/source/path", "/target/path", createdAt);

        // Assert
        Assert.Equal("my-skill", link.SkillName);
        Assert.Equal("/source/path", link.SourcePath);
        Assert.Equal("/target/path", link.TargetPath);
        Assert.Equal(createdAt, link.CreatedAt);
    }

    [Fact]
    public void AppConfiguration_DefaultValues()
    {
        // Arrange & Act
        var config = AppConfiguration.Default;

        // Assert
        Assert.NotNull(config.SkillsRepositoryPath);
        Assert.NotNull(config.OpencodeSkillsPath);
        Assert.Contains("skill-linker", config.SkillsRepositoryPath);
        Assert.Contains(".opencode", config.OpencodeSkillsPath);
    }

    [Fact]
    public void AppConfiguration_WithExpression()
    {
        // Arrange
        var config = AppConfiguration.Default;

        // Act
        var updated = config with { SkillsRepositoryPath = "/custom/path" };

        // Assert
        Assert.Equal("/custom/path", updated.SkillsRepositoryPath);
        Assert.Equal(config.OpencodeSkillsPath, updated.OpencodeSkillsPath);
    }
}
