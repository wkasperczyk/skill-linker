using Spectre.Console;

namespace SkillLinker.Commands;

/// <summary>
/// Disable command - unlinks a skill from .opencode/skills
/// </summary>
public class DisableCommand
{
    private readonly Services.ILinkManager _linkManager;

    public DisableCommand(Services.ILinkManager linkManager)
    {
        _linkManager = linkManager;
    }

    public Task<int> ExecuteAsync(string[] args)
    {
        if (args.Length < 2)
        {
            AnsiConsole.MarkupLine("[red]Usage: skill-linker disable <skill-name>[/]");
            return Task.FromResult(1);
        }

        var skillName = args[1];

        if (!_linkManager.IsLinked(skillName))
        {
            AnsiConsole.MarkupLine($"[yellow]Skill '{skillName}' is not linked[/]");
            return Task.FromResult(0);
        }

        var success = _linkManager.RemoveLink(skillName);

        if (success)
        {
            AnsiConsole.MarkupLine($"[green]Successfully unlinked skill '{skillName}'[/]");
            return Task.FromResult(0);
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]Failed to unlink skill '{skillName}'[/]");
            return Task.FromResult(1);
        }
    }
}
