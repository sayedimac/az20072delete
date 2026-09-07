# GitHub Copilot Instructions

- Follow the repository's existing architecture, naming conventions, and coding style.
- Make focused changes that directly address the requested task; avoid unrelated refactoring.
- Prefer clear, maintainable code and reuse existing helpers and dependencies where practical.
- Preserve existing behavior unless a change is explicitly requested.
- Never commit secrets, credentials, connection strings, or environment-specific values.
- Validate inputs and handle errors appropriately, especially for external services such as Azure Blob Storage.
- Add or update relevant tests when behavior changes, and ensure existing tests continue to pass.
- Update documentation or configuration examples when changes affect setup or usage.
