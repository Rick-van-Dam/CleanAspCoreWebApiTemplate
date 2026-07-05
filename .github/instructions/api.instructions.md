---
applyTo: "CleanAspCore.Api/**/*.cs"
---

- Keep endpoint handlers minimal and focused on HTTP orchestration.
- Place new endpoint code under the relevant feature folder in `Endpoints`.
- Match route, naming, and result patterns used by neighboring endpoints.
- Reuse existing app configuration modules for auth, errors, and openapi.
- Avoid moving business logic into the API project.
