// SkillLinker - CLI/TUI tool for managing skill symbolic links
// Entry point

using Microsoft.Extensions.DependencyInjection;
using SkillLinker.Services;
using SkillLinker.Commands;
using SkillLinker.Tui;

namespace SkillLinker;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var services = ConfigureServices();
        var serviceProvider = services.BuildServiceProvider();

        return await ExecuteAsync(args, serviceProvider);
    }

    private static IServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();

        // Register services
        services.AddSingleton<IConfigurationService, ConfigurationService>();
        services.AddSingleton<ISkillRepositoryService, SkillRepositoryService>();
        services.AddSingleton<ILinkManager, LinkManager>();

        // Register commands
        services.AddTransient<ListCommand>();
        services.AddTransient<EnableCommand>();
        services.AddTransient<DisableCommand>();
        services.AddTransient<ConfigCommand>();

        // Register TUI
        services.AddTransient<SkillTui>();

        return services;
    }

    private static async Task<int> ExecuteAsync(string[] args, ServiceProvider serviceProvider)
    {
        // If no arguments, launch TUI
        if (args.Length == 0 || args[0] == "tui")
        {
            var tui = serviceProvider.GetRequiredService<SkillTui>();
            return await tui.RunAsync();
        }

        // Route to appropriate command
        return args[0].ToLowerInvariant() switch
        {
            "list" => await serviceProvider.GetRequiredService<ListCommand>().ExecuteAsync(args),
            "enable" => await serviceProvider.GetRequiredService<EnableCommand>().ExecuteAsync(args),
            "disable" => await serviceProvider.GetRequiredService<DisableCommand>().ExecuteAsync(args),
            "config" => await serviceProvider.GetRequiredService<ConfigCommand>().ExecuteAsync(args),
            "--help" or "-h" or "help" => ShowHelp(),
            _ => ShowHelp()
        };
    }

    private static int ShowHelp()
    {
        Console.WriteLine("SkillLinker - Manage skill symbolic links");
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine("  skill-linker                    # Launch interactive TUI");
        Console.WriteLine("  skill-linker list               # List all skills");
        Console.WriteLine("  skill-linker enable <name>      # Enable a skill");
        Console.WriteLine("  skill-linker disable <name>     # Disable a skill");
        Console.WriteLine("  skill-linker config             # Manage configuration");
        return 0;
    }
}
