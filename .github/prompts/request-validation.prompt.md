# Add request validation

Add or update validation for an endpoint request.

## Requirements
- Reuse existing validation extension patterns.
- Keep validation rules near the feature endpoint code.
- Return errors through the existing global error handling flow.
- Add integration test coverage for invalid request payloads.
- Do not change unrelated endpoint behavior.

## Inputs
- Feature/endpoint:
- Fields to validate:
- Validation rules:
