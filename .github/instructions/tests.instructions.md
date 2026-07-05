---
applyTo: "Tests/**/*.cs"
---

- Prefer integration-style tests using the existing fixture and faker setup.
- Keep test names explicit about scenario and expected outcome.
- Reuse existing API client interfaces and assertion helpers.
- Avoid introducing flaky timing-based checks.
- Cover behavior changes with targeted test additions.
