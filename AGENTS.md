# AGENTS

## Goal
Keep agent-generated changes fast, small, and consistent with this template.

## Workflow
1. Identify the target feature folder first.
2. Limit edits to files required for the task.
3. Mirror nearby patterns for endpoint shape, validation, and tests.
4. Add/update tests when behavior changes.
5. Run existing commands only (`dotnet test` for full verification when needed).

## Guardrails
- Do not refactor unrelated files.
- Do not move logic across projects unless the task requires it.
- Keep API concerns in `CleanAspCore.Api` and core/domain concerns in `CleanAspCore.Core`.
