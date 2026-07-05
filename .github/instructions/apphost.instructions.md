---
applyTo: "CleanAspCore.AppHost/**/*.cs"
---

- Keep AppHost focused on service composition and local development orchestration.
- Reuse existing configuration style for service registration and defaults.
- Avoid feature logic in AppHost.
- Keep environment-specific behavior in config files where possible.
