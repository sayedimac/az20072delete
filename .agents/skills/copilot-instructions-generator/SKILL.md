---
name: copilot-instructions-generator
description: Analyses a repository and generates GitHub Copilot instruction files at repository, folder, application, language, framework, and file-type scope. Use when asked to create, improve, organise, or regenerate Copilot instructions for a repository.
---

# GitHub Copilot Instructions Generator

Generate concise, evidence-based GitHub Copilot instruction files that reflect how the repository is actually structured and developed.

## Workflow

1. Analyse the complete repository structure.
2. Read relevant source code, project files, configuration, documentation, tests, build scripts, workflows, and existing instruction files.
3. Identify:
 - Repository purpose and architecture
 - Applications, services, packages, and important folders
 - Languages, frameworks, and file types
 - Build, run, format, lint, and test commands
 - Coding, naming, testing, documentation, and security conventions
4. Separate guidance into repository-wide and scoped instructions.
5. Present a short proposed file plan.
6. Create or update the instruction files.
7. Validate paths, frontmatter, glob patterns, duplication, and contradictions.
8. Report the files created, evidence used, and any unresolved assumptions.

## Repository-level instructions

Create or update:

`.github/copilot-instructions.md`

Include only guidance that applies across the repository:

- Repository purpose
- Architecture and boundaries
- Shared coding conventions
- Approved technologies
- Build and test commands
- Security requirements
- Error handling and logging
- Documentation expectations
- Repository-wide restrictions

Do not include YAML frontmatter in this file.

## Scoped instructions

Create scoped files under:

`.github/instructions/<scope>.instructions.md`

Use scoped files for:

- Individual applications or services
- Specific folders or packages
- Programming languages
- Frameworks
- Tests
- Infrastructure and deployment files
- Documentation
- Configuration or data file types

Each scoped file must begin with:

```yaml
---
description: Concise explanation of this instruction scope
applyTo: "appropriate/glob/pattern"
---
