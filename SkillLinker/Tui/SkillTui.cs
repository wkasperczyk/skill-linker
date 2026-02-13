using Spectre.Console;
using SkillLinker.Domain.Models;

namespace SkillLinker.Tui;

/// <summary>
/// Interactive TUI for skill management
/// </summary>
public class SkillTui
{
    private readonly Services.ISkillRepositoryService _skillRepository;
    private readonly Services.ILinkManager _linkManager;
    private readonly Services.IConfigurationService _configService;

    public SkillTui(
        Services.ISkillRepositoryService skillRepository,
        Services.ILinkManager linkManager,
        Services.IConfigurationService configService)
    {
        _skillRepository = skillRepository;
        _linkManager = linkManager;
        _configService = configService;
    }

    public async Task<int> RunAsync()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[bold blue]Skill Linker[/]"));

            var skills = _skillRepository.GetAllSkills().ToList();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices(new[]
                    {
                        "List all skills",
                        "Enable a skill",
                        "Disable a skill",
                        "View configuration",
                        "Exit"
                    }));

            switch (choice)
            {
                case "List all skills":
                    await ListSkillsAsync(skills);
                    break;
                case "Enable a skill":
                    await EnableSkillAsync(skills);
                    break;
                case "Disable a skill":
                    await DisableSkillAsync(skills);
                    break;
                case "View configuration":
                    ShowConfiguration();
                    break;
                case "Exit":
                    return 0;
            }
        }
    }

    private Task ListSkillsAsync(List<Skill> skills)
    {
        var table = new Table()
            .AddColumn("Name")
            .AddColumn("Status")
            .AddColumn("Description");

        foreach (var skill in skills)
        {
            var status = skill.IsLinked
                ? "[green]Linked[/]"
                : "[dim]Not linked[/]";

            table.AddRow(skill.Name, status, skill.Description);
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
        AnsiConsole.Ask<string>("Press [[Enter]] to continue...");

        return Task.CompletedTask;
    }

    private async Task EnableSkillAsync(List<Skill> skills)
    {
        var unlinkedSkills = skills.Where(s => !s.IsLinked).ToList();

        if (!unlinkedSkills.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No skills available to enable.[/]");
            AnsiConsole.Ask<string>("Press [[Enter]] to continue...");
            return;
        }

        var skillName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select a skill to enable:")
                .AddChoices(unlinkedSkills.Select(s => s.Name)));

        var success = _linkManager.CreateLink(skillName);

        if (success)
        {
            AnsiConsole.MarkupLine($"[green]Successfully linked skill '{skillName}'[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]Failed to link skill '{skillName}'[/]");
        }

        await Task.CompletedTask;
        AnsiConsole.Ask<string>("Press [[Enter]] to continue...");
    }

    private async Task DisableSkillAsync(List<Skill> skills)
    {
        var linkedSkills = skills.Where(s => s.IsLinked).ToList();

        if (!linkedSkills.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No skills currently linked.[/]");
            AnsiConsole.Ask<string>("Press [[Enter]] to continue...");
            return;
        }

        var skillName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select a skill to disable:")
                .AddChoices(linkedSkills.Select(s => s.Name)));

        var success = _linkManager.RemoveLink(skillName);

        if (success)
        {
            AnsiConsole.MarkupLine($"[green]Successfully unlinked skill '{skillName}'[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]Failed to unlink skill '{skillName}'[/]");
        }

        await Task.CompletedTask;
        AnsiConsole.Ask<string>("Press [[Enter]] to continue...");
    }

    private void ShowConfiguration()
    {
        var config = _configService.GetConfiguration();

        var table = new Table();
        table.AddColumn("Setting");
        table.AddColumn("Value");

        table.AddRow("Repository Path", config.SkillsRepositoryPath);
        table.AddRow("OpenCode Skills Path", config.OpencodeSkillsPath);

        AnsiConsole.Write(table);
        AnsiConsole.Ask<string>("Press [[Enter]] to continue...");
    }
}
