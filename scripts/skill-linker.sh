#!/bin/bash
# SkillLinker - Bash integration script
# Provides CLI shortcuts for skill-linker operations

set -e

# Get the directory where this script is located (go up to project root)
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
SKILL_LINKER_DLL="$SCRIPT_DIR/SkillLinker/bin/Release/net10.0/SkillLinker.dll"

# Find dotnet
if ! command -v dotnet &> /dev/null; then
    echo "Error: dotnet is not installed" >&2
    exit 1
fi

# Check if the DLL exists, build if not
if [ ! -f "$SKILL_LINKER_DLL" ]; then
    echo "SkillLinker.dll not found. Building release..." >&2
    dotnet build "$SCRIPT_DIR/SkillLinker/SkillLinker.csproj" -c Release
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
        dotnet "$SKILL_LINKER_DLL" list "$@"
        ;;
    enable)
        if [ -z "$1" ]; then
            echo "Error: Please specify a skill name" >&2
            echo "Usage: skill-linker enable <name>" >&2
            exit 1
        fi
        dotnet "$SKILL_LINKER_DLL" enable "$@"
        ;;
    disable)
        if [ -z "$1" ]; then
            echo "Error: Please specify a skill name" >&2
            echo "Usage: skill-linker disable <name>" >&2
            exit 1
        fi
        dotnet "$SKILL_LINKER_DLL" disable "$@"
        ;;
    config)
        dotnet "$SKILL_LINKER_DLL" config "$@"
        ;;
    tui)
        dotnet "$SKILL_LINKER_DLL" tui "$@"
        ;;
    help|--help|-h)
        show_help
        ;;
    "")
        # No command - launch TUI by default
        dotnet "$SKILL_LINKER_DLL" tui "$@"
        ;;
    *)
        echo "Unknown command: $COMMAND" >&2
        echo "Run 'skill-linker help' for usage information" >&2
        exit 1
        ;;
esac
