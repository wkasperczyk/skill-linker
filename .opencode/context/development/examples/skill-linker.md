# Example: Skill-Linker Implementation

**What**: Tool that creates symlinks from a skills repository (`~/.config/skill-linker/skills`) to `.opencode/skills` for enabling/disabling skills.

**Architecture**:
```
SkillLinker/
├── Domain/Models/     # Skill, SkillLink, AppConfiguration (records)
├── Services/         # ConfigurationService, SkillRepositoryService, LinkManager
├── Commands/         # ListCommand, EnableCommand, DisableCommand, ConfigCommand
└── Tui/             # SkillTui (interactive mode)
```

**Key Patterns**:
- **Records** for immutable models: `public record Skill(string Name, string Description, string Path, bool IsLinked);`
- **DI Container**: `services.AddSingleton<IService, Service>()` 
- **Config persistence**: JSON to `~/.config/skill-linker/config.json`
- **Symlink operations**: `Directory.CreateSymbolicLink()`

**CLI Commands**:
```bash
dotnet run -- list              # List skills
dotnet run -- enable <name>    # Create symlink
dotnet run -- disable <name>   # Remove symlink
dotnet run -- config            # Show/set config
dotnet run --                   # Launch TUI (default)
```

**Bash Integration**:
```bash
./scripts/skill-linker.sh list
./scripts/skill-linker.sh enable my-skill
./scripts/skill-linker.sh disable my-skill
```

**Reference**: core/concepts/dotnet-cli-tool.md
