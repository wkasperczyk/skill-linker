#!/bin/bash
# SkillLinker - Bash integration script
# Provides CLI shortcuts for skill-linker operations

set -e

# Get the directory where this script is located (go up to project root)
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
SKILL_LINKER_DLL="$SCRIPT_DIR/SkillLinker/bin/Debug/net10.0/SkillLinker.dll"

# Find dotnet
if ! command -v dotnet &> /dev/null; then
    echo "Error: dotnet is not installed" >&2
    exit 1
fi

# Check if the DLL exists
if [ ! -f "$SKILL_LINKER_DLL" ]; then
    echo "Error: SkillLinker.dll not found. Please build the project first:" >&2
    echo "  cd SkillLinker && dotnet build" >&2
    exit 1
fi

# Function to display help
show_help() {
    cat << EOF
SkillLinker - Manage skill symbolic links

Usage:
    skill-linker <command> [options]

Commands:
    list               List all skills in the repository
    enable <name>     Enable (link) a skill by name
    disable <name>    Disable (unlink) a skill by name
    config            Show current configuration
    config set-repo <path>   Set skills repository path
    config set-opencode <path>  Set opencode skills path
    tui                Launch interactive TUI interface
    help               Show this help message

Examples:
    skill-linker list
    skill-linker enable my-skill
    skill-linker disable my-skill
    skill-linker config set-repo ~/my-skills

EOF
}

# Parse command
COMMAND="${1:-}"
shift || true

case "$COMMAND" in
    list)
        dotnet run --project "$SCRIPT_DIR/SkillLinker/SkillLinker.csproj" -- list "$@"
        ;;
    enable)
        if [ -z "$1" ]; then
            echo "Error: Please specify a skill name" >&2
            echo "Usage: skill-linker enable <name>" >&2
            exit 1
        fi
        dotnet run --project "$SCRIPT_DIR/SkillLinker/SkillLinker.csproj" -- enable "$@"
        ;;
    disable)
        if [ -z "$1" ]; then
            echo "Error: Please specify a skill name" >&2
            echo "Usage: skill-linker disable <name>" >&2
            exit 1
        fi
        dotnet run --project "$SCRIPT_DIR/SkillLinker/SkillLinker.csproj" -- disable "$@"
        ;;
    config)
        dotnet run --project "$SCRIPT_DIR/SkillLinker/SkillLinker.csproj" -- config "$@"
        ;;
    tui)
        dotnet run --project "$SCRIPT_DIR/SkillLinker/SkillLinker.csproj" -- tui "$@"
        ;;
    help|--help|-h)
        show_help
        ;;
    "")
        # No command - launch TUI by default
        dotnet run --project "$SCRIPT_DIR/SkillLinker/SkillLinker.csproj" -- tui "$@"
        ;;
    *)
        echo "Unknown command: $COMMAND" >&2
        echo "Run 'skill-linker help' for usage information" >&2
        exit 1
        ;;
esac
