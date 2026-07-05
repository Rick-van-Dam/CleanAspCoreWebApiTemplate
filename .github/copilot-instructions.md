# Copilot instructions for CleanAspCoreWebApiTemplate

## Scope and style
- Keep changes small and task-focused.
- Do not refactor unrelated files.
- Follow existing naming and endpoint patterns in nearby feature files.
- Prefer extending existing abstractions over introducing new frameworks.

## Architecture boundaries
- `CleanAspCore.Api`: endpoint wiring, request/response contracts, validation and HTTP concerns.
- `CleanAspCore.Core`: domain models, data access, shared cross-cutting logic.
- `CleanAspCore.AppHost`: local orchestration and host setup.
- `Tests`: integration and scenario coverage.

## Rules
- Keep feature work grouped by feature folder in `CleanAspCore.Api/Endpoints/*`.
- Reuse existing validation and error handling patterns.
- Keep serialization/auth/openapi setup in existing configuration locations.
- Add or update tests when behavior changes.
- Run only existing build/test commands in this repository.
