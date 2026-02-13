# AGENTS.md - Agent Guidelines for skill-linker

This file provides coding guidelines and conventions for agentic coding agents operating in this repository.

---

## Project Overview

**Project**: OpenAgents Control - Skill Linker  
**Type**: OpenCode plugin with context system  
**Primary Language**: TypeScript, Markdown (context files)  
**Purpose**: Context management system for AI coding agents with pattern discovery and task management

---

## Build, Lint, and Test Commands

This is primarily a documentation/context plugin project with minimal TypeScript code.

### Running the Project

```bash
# No build commands - this is a markdown-based context system
# TypeScript files use Bun or Node.js directly

# Run a single TypeScript file
bun run .opencode/tool/env/index.ts
node --loader ts-node/esm .opencode/tool/env/index.ts

# Check TypeScript compilation (if tsconfig exists)
npx tsc --noEmit
```

### Task Management Commands

```bash
# Check task status
bash .opencode/skills/task-management/router.sh status

# Show next eligible tasks
bash .opencode/skills/task-management/router.sh next

# Show blocked tasks
bash .opencode/skills/task-management/router.sh blocked

# Mark task complete
bash .opencode/skills/task-management/router.sh complete <feature> <seq> "summary"

# Validate all tasks
bash .opencode/skills/task-management/router.sh validate
```

### Testing

```bash
# No formal test suite exists
# Manual testing via:
# - Running individual TypeScript modules
# - Bash script execution
# - Context file validation
```

---

## Code Style Guidelines

### TypeScript Conventions

**General Principles**:
- Modular, functional, maintainable code
- Pure functions where possible (same input = same output)
- Immutability: create new data, don't modify existing
- Small functions (< 50 lines)
- Explicit dependencies via dependency injection

**Naming Conventions**:
- Files: lowercase-with-dashes.ts (e.g., task-cli.ts)
- Functions: verbPhrases (getUser, validateEmail)
- Predicates: isValid, hasPermission, canAccess
- Variables: descriptive (userCount not uc), const by default
- Constants: UPPER_SNAKE_CASE
- Interfaces: PascalCase with descriptive names (e.g., EnvLoaderConfig)

**Imports**:
```typescript
// Use explicit relative imports
import { readFile } from "fs/promises"
import { resolve } from "path"

// Group imports: external first, then internal
import { z } from "zod"
import { readFile, writeFile } from "fs/promises"
import { resolve } from "path"
import { myLocalModule } from "./myLocalModule"
```

**Types**:
- Always use explicit types for function parameters and return types
- Use interfaces for object shapes
- Use type aliases for unions/intersections
```typescript
interface EnvLoaderConfig {
  searchPaths?: string[]
  verbose?: boolean
  override?: boolean
}

function loadEnvVariables(config: EnvLoaderConfig = {}): Promise<Record<string, string>>
```

**Formatting**:
- Use 2-space indentation
- Maximum line length: 100 characters
- Add blank lines between logical sections
- Use semicolons consistently

### Markdown/Context File Conventions

**File Structure**:
```markdown
---
name: skill-name
description: Brief description
version: 1.0.0
author: opencode
type: skill|category
tags: []
---

# Title

> Purpose statement

---

## Sections...
```

**Frontmatter** (for skills and agents):
```yaml
---
description: "Agent description"
model: anthropic/claude-sonnet-4-5
---
```

**Context File Metadata**:
```markdown
<!-- Context: standards/code | Priority: critical | Version: 2.0 | Updated: 2025-01-21 -->
```

**Navigation Files**:
- Use consistent navigation.md structure
- List files in logical order with descriptions

---

## Error Handling

### TypeScript

```typescript
// Use explicit error handling with result objects
function parseJSON(text: string): { success: true; data: unknown } | { success: false; error: string } {
  try {
    return { success: true, data: JSON.parse(text) }
  } catch (error) {
    return { success: false, error: error instanceof Error ? error.message : "Unknown error" }
  }
}

// Validate at boundaries
function createUser(userData: UserData): { success: true; user: User } | { success: false; errors: string[] } {
  const validation = validateUserData(userData)
  if (!validation.isValid) {
    return { success: false, errors: validation.errors }
  }
  return { success: true, user: saveUser(userData) }
}
```

### Error Messages
- Be specific and actionable
- Include context (what operation failed, why)
- Suggest remediation steps
```typescript
throw new Error(`${varName} not found. Please set it in your environment or .env file.

To fix this:
1. Add to .env file: ${varName}=your_value_here
2. Or export it: export ${varName}=your_value_here

Searched paths: ${searchPaths.join(', ')}`)
```

---

## Anti-Patterns to Avoid

**In TypeScript**:
- ❌ Mutation: Modifying data in place
- ❌ Side effects in pure functions (console.log, API calls)
- ❌ Deep nesting: Use early returns instead
- ❌ God modules: Split into focused modules
- ❌ Global state: Pass dependencies explicitly
- ❌ Large functions: Keep < 50 lines
- ❌ Implicit any: Always use explicit types

**In Markdown/Context**:
- ❌ Outdated information without "updated" dates
- ❌ Inconsistent formatting within files
- ❌ Missing examples for complex concepts
- ❌ Circular references in navigation

---

## Best Practices

### TypeScript
- ✅ Pure functions whenever possible
- ✅ Immutable data structures (spread operator, Object.freeze)
- ✅ Small, focused functions (< 50 lines)
- ✅ Compose small functions into larger ones
- ✅ Explicit dependencies (dependency injection)
- ✅ Validate at boundaries
- ✅ Self-documenting code with JSDoc for complex functions

### Documentation
- ✅ Explain WHY, not just WHAT
- ✅ Include working examples
- ✅ Show expected output
- ✅ Cover error handling
- ✅ Use consistent terminology
- ✅ Keep structure predictable
- ✅ Update when code changes

### Context Files
- ✅ Keep files under 200 lines (MVI principle)
- ✅ Use lazy loading (load what's needed, when needed)
- ✅ Include priority in frontmatter comments
- ✅ Version and date all standards documents
- ✅ Separate context_files (standards) from reference_files (source)

---

## Project Structure

```
.opencode/
├── agent/                    # Agent definitions (markdown)
│   ├── core/                 # Core agents (opencoder, openagent)
│   └── subagents/           # Specialized subagents
├── command/                 # Custom commands
├── context/                 # Context files (standards, guides)
│   ├── core/               # Core context (standards, workflows)
│   ├── development/       # Development guides
│   └── project-intelligence/ # Project-specific patterns
├── skills/                  # Skills (task-management, context7)
├── tool/                    # TypeScript tools
│   └── env/               # Environment variable loader
└── README.md              # Main documentation
```

---

## Agent Workflow

When working in this repository:

1. **Discover**: Use ContextScout to find relevant context files
2. **Propose**: Present implementation plan before execution
3. **Approve**: Wait for user approval before making changes
4. **Execute**: Implement incrementally with validation
5. **Validate**: Run type checks, validate task structure
6. **Document**: Update context files if new patterns discovered

---

## Key Files to Know

- `.opencode/context/core/standards/code-quality.md` - Code standards
- `.opencode/context/core/standards/documentation.md` - Documentation standards
- `.opencode/skills/task-management/SKILL.md` - Task tracking
- `.opencode/tool/env/index.ts` - Example TypeScript module
- `.opencode/agent/core/opencoder.md` - Main agent definition

---

**Golden Rule**: If you can't easily test it, refactor it.
