# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.2.2] - 2025-03-25

## Fixed
- Call parameterless listeners for events sent with parameters that have no parameterized listeners

## [1.2.1] - 2024-05-07

## Fixed
- Remove unnecessary using directives.
- Package documentation URL

## [1.2.0] - 2023-02-08

### Added
- Queue Add/Remove listener actions if called during event invocation.
- Support for target on EventfulListener component.
- Allow EventfulListener to be configured from code.
- Tests for EventfulListener component.

## Changed
- Send global event on `null` target in Eventful.Add/RemoveListener calls.
- Send global event on `null` target in Eventful.Send calls.

## [1.1.1] - 2023-01-30

### Fixed
- Missing key exception when sending targeted events with no listeners.

## [1.1.0] - 2023-01-29

### Added
- Targeted events.

### Changed
- Refactor Eventful static class for clarity.

## [1.0.0] - 2023-01-28

### Added
- Initial release!
- Send/receive events with up to 5 parameters.
- EventfulListener component for receiving events in the editor.
