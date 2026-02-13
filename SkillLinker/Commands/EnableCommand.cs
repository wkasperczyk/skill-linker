using Spectre.Console;

namespace SkillLinker.Commands;

/// <summary>
/// Enable command - links a skill to .opencode/skills
/// </summary>
public class EnableCommand
{
    private readonly Services.ILinkManager _linkManager;
    private readonly Services.ISkillRepositoryService _skillRepository;

    public EnableCommand(Services.ILinkManager linkManager, Services.ISkillRepositoryService skillRepository)
    {
        _linkManager = linkManager;
        _skillRepository = skillRepository;
    }

    public async Task<int> ExecuteAsync(string[] args)
    {
        if (args.Length < 2)
        {
            AnsiConsole.MarkupLine("[red]Usage: skill-linker enable <skill-name>[/]");
            return 1;
        }

        var skillName = args[1];

        if (!_skillRepository.SkillExists(skillName))
        {
            AnsiConsole.MarkupLine($"[red]Skill '{skillName}' not found in repository[/]");
            return 1;
        }

        if (_linkManager.IsLinked(skillName))
        {
            AnsiConsole.MarkupLine($"[yellow]Skill '{skillName}' is already linked[/]");
            return 0;
        }

        var success = _linkManager.CreateLink(skillName);

        if (success)
        {
            AnsiConsole.MarkupLine($"[green]Successfully linked skill '{skillName}'[/]");
            return 0;
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]Failed to link skill '{skillName}'[/]");
            return 1;
        }
    }
}
