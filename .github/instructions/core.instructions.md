---
applyTo: "CleanAspCore.Core/**/*.cs"
---

- Keep domain/data code framework-light and deterministic.
- Use existing EF Core conventions and entity configuration patterns.
- Prefer extending existing common modules before adding new cross-cutting utilities.
- Keep public API surface minimal and aligned with current naming conventions.
- Do not introduce HTTP-specific logic in Core.
