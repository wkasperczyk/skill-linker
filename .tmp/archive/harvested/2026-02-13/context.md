# Task Context: Skill Linker Tool

Session ID: 2026-02-13-skill-linker
Created: 2026-02-13T00:00:00Z
Status: in_progress

## Current Request
Create a .NET Core CLI/TUI tool that manages symbolic links between a local skills repository and the `.opencode/skills` directory. Features:
- TUI with list of all skills in repository
- Link/unlink skills (enable/disable)
- Works with opencode - links created in `.opencode/skills`
- CLI mode: list skills, enable by name, disable by name, change repository path
- Configurable skill repository path (default: `~/.config/skill-linker/skills`)
- Bash integration for easy CLI usage
- Unit tests

## Context Files (Standards to Follow)
- `.opencode/context/core/standards/code-quality.md` - Modular, functional code principles
- `.opencode/context/core/standards/test-coverage.md` - Testing standards

## Reference Files (Source Material to Look At)
- `.opencode/skills/task-management/SKILL.md` - Example skill structure with router.sh

## External Docs Fetched
- Spectre.Console for .NET - CLI/TUI framework

## Components
1. **Core Domain** - Models: Skill, SkillLink, Configuration
2. **Configuration Service** - Load/save config, default path `~/.config/skill-linker/skills`
3. **Skill Repository Service** - Scan skills directory, detect existing links
4. **Link Manager** - Create/remove symbolic links in `.opencode/skills`
5. **CLI Commands** - List, enable, disable, config commands
6. **TUI Interface** - Interactive skill browser
7. **Bash Integration** - Shell wrapper script
8. **Unit Tests** - Test core services

## Constraints
- .NET Core (latest LTS)
- Spectre.Console for CLI/TUI
- Modular architecture with dependency injection
- Pure functions where possible
- Unit tests for core business logic

## Exit Criteria
- [ ] CLI commands work: list, enable, disable, config
- [ ] TUI interface displays skills and allows link/unlink
- [ ] Bash integration script provided
- [ ] Unit tests for core services pass
- [ ] Configuration persisted to ~/.config/skill-linker/
