# GitHub Copilot Instructions for CleanAspCoreWebApiTemplate

This file guides GitHub Copilot to match suggestions with the repository's actual conventions and architecture.

## Repository Purpose

- **Template for ASP.NET Minimal API**: Demonstrates clean and productive API development using ASP.NET minimal APIs.
- **Feature Focus**: Emphasis is on developer productivity and simplicity, not on a large set of API features.

## Architecture

- **Vertical Slice Architecture**: Features are grouped by domain/feature rather than technical layer. See [Vertical Slice architecture](https://www.jimmybogard.com/vertical-slice-architecture/).
- **Minimal API**: Uses ASP.NET minimal APIs, so entry points are in `Program.cs` and route configuration is done via extension methods (see `AppConfiguration.cs` and endpoint configs).
- **Directory Structure**:
  - `CleanAspCore.Api/Endpoints/<Feature>`: Endpoints grouped per feature (e.g., Departments, Employees)
  - `CleanAspCore.Api/`: Entry point (`Program.cs`), configuration, error handling.
  - `CleanAspCore.Core/`: Shared logic, OpenAPI helpers, validation.
  - `CleanAspCore.Data/`: Data access and context.
  - `CleanAspCore.AppHost/`: Setup for running the distributed application.
  - `Tests/`: Integration tests using Docker via a fast setup.

## Coding Style & Conventions

- **Language**: C#
- **Framework**: ASP.NET Core (.NET Aspire for distributed app hosting)
- **Naming**:
  - Classes: PascalCase
  - Methods: PascalCase
  - Variables: camelCase
- **Endpoints**: Use feature-based extension methods like `AddDepartmentsRoutes`.
- **Dependency Injection**: All services registered in `Program.cs`/`AppConfiguration.cs`.
- **Validation**: Use FluentValidation for request validation.
- **Authentication**: JWT tokens, with claims used for authorization (see `AppConfiguration.cs` for policies).
- **OpenAPI**: Configured in `OpenApiExtensions.cs`.

## Testing

- **Integration Tests**: Located in `Tests/`; run with Docker via `dotnet test`. Uses custom `TestWebApi` factory for setup.
- **JWT Testing**: Test JWTs are generated and used to verify authentication and claims.

## Patterns to Prefer

- Use feature folders and vertical slice architecture: add new features as new folders in `Endpoints/`.
- Register routes for features via extension methods in `AppConfiguration.cs`.
- Use minimal API pattern: avoid controllers; use route handlers directly.
- Favor extension methods (`AddAuthServices`, `AddAppRoutes`, etc.) for configuration.

## Example Prompts

- "Create a new endpoint in `Endpoints/Weapons` for listing weapons."
- "Add a JWT claim requirement to the Employees endpoints."
- "Write integration tests for the Departments feature using the existing test setup."

---

*Update this file as the architecture or conventions evolve to keep Copilot suggestions accurate!*