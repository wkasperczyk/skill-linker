# Concept: .NET Core CLI/TUI Tool with Spectre.Console

**Core Idea**: Build modular CLI applications using Spectre.Console for rich terminal UI, supporting both command-line and interactive TUI modes with dependency injection.

**Key Points**:
- Use `Spectre.Console` for rich output (tables, selections, prompts)
- Implement command pattern for CLI: each command is a class with `ExecuteAsync()`
- Use `Microsoft.Extensions.DependencyInjection` for service registration
- Support both CLI args (`list`, `enable`, `disable`) and TUI mode
- Default to TUI when no arguments provided

**Minimal Example**:
```csharp
// Program.cs
var services = new ServiceCollection();
services.AddSingleton<IConfigService, ConfigService>();
// ... register services
var sp = services.BuildServiceProvider();

return args[0] switch {
    "list" => await sp.GetRequiredService<ListCommand>().ExecuteAsync(args),
    "tui" => await sp.GetRequiredService<SkillTui>().RunAsync(),
    _ => ShowHelp()
};
```

**Reference**: https://spectreconsole.net/

**Related**:
- development/examples/skill-linker.md
- core/standards/code-quality.md
