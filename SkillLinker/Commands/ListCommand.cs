using Spectre.Console;

namespace SkillLinker.Commands;

/// <summary>
/// List command - displays all available skills
/// </summary>
public class ListCommand
{
    private readonly Services.ISkillRepositoryService _skillRepository;

    public ListCommand(Services.ISkillRepositoryService skillRepository)
    {
        _skillRepository = skillRepository;
    }

    public Task<int> ExecuteAsync(string[] args)
    {
        var linkedOnly = args.Contains("--linked") || args.Contains("-l");
        var unlinkedOnly = args.Contains("--unlinked") || args.Contains("-u");

        var skills = linkedOnly
            ? _skillRepository.GetLinkedSkills()
            : unlinkedOnly
                ? _skillRepository.GetUnlinkedSkills()
                : _skillRepository.GetAllSkills();

        var table = new Table()
            .AddColumn("Name")
            .AddColumn("Status")
            .AddColumn("Description");

        foreach (var skill in skills)
        {
            var status = skill.IsLinked
                ? "[green]Linked[/]"
                : "[dim]Not linked[/]";

            table.AddRow(
                skill.Name,
                status,
                skill.Description.Length > 50
                    ? skill.Description[..50] + "..."
                    : skill.Description
            );
        }

        AnsiConsole.Write(table);
        return Task.FromResult(0);
    }
}
