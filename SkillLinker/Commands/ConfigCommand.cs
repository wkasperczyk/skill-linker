using Spectre.Console;

namespace SkillLinker.Commands;

/// <summary>
/// Config command - manage configuration
/// </summary>
public class ConfigCommand
{
    private readonly Services.IConfigurationService _configService;

    public ConfigCommand(Services.IConfigurationService configService)
    {
        _configService = configService;
    }

    public Task<int> ExecuteAsync(string[] args)
    {
        if (args.Length >= 2 && args[1] == "show")
        {
            ShowConfig();
            return Task.FromResult(0);
        }

        if (args.Length >= 3)
        {
            var action = args[1];
            var value = args[2];

            switch (action)
            {
                case "repository":
                    _configService.UpdateRepositoryPath(value);
                    AnsiConsole.MarkupLine($"[green]Repository path updated to: {value}[/]");
                    return Task.FromResult(0);

                case "opencode":
                    _configService.UpdateOpencodePath(value);
                    AnsiConsole.MarkupLine($"[green]OpenCode skills path updated to: {value}[/]");
                    return Task.FromResult(0);

                default:
                    ShowHelp();
                    return Task.FromResult(1);
            }
        }

        ShowConfig();
        return Task.FromResult(0);
    }

    private void ShowConfig()
    {
        var config = _configService.GetConfiguration();

        var table = new Table();
        table.AddColumn("Setting");
        table.AddColumn("Value");

        table.AddRow("Repository Path", config.SkillsRepositoryPath);
        table.AddRow("OpenCode Skills Path", config.OpencodeSkillsPath);
        table.AddRow("Config File", config.ConfigFilePath);

        AnsiConsole.Write(table);
    }

    private void ShowHelp()
    {
        AnsiConsole.MarkupLine("[bold]Usage:[/]");
        AnsiConsole.MarkupLine("  skill-linker config show                    # Show current configuration");
        AnsiConsole.MarkupLine("  skill-linker config repository <path>     # Set repository path");
        AnsiConsole.MarkupLine("  skill-linker config opencode <path>       # Set OpenCode skills path");
    }
}
