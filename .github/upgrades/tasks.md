# TriviaSpark .NET 10 Migration Tasks

## Overview

This task list executes the Big Bang upgrade of the TriviaSpark solution from .NET 9.0 to .NET 10.0 (Preview), updating all 6 projects and 10 packages in a single atomic operation, followed by automated test and validation steps. All tasks are fully automatable and reference the migration plan for details.

**Progress**: 2/3 tasks complete (67%) ![67%](https://progress-bar.xyz/67)

## Tasks

### [✓] TASK-001: Verify .NET 10 SDK and environment readiness *(Completed: 2025-12-01 20:44)*
**References**: Plan §12 Phase 0

- [✓] (1) Verify .NET 10 SDK (10.0.x) is installed using `dotnet --list-sdks`
- [✓] (2) Confirm `upgrade-to-NET10` branch is active and working directory is clean
- [✓] (3) Ensure build tools and IDE support .NET 10 preview
- [✓] (4) All environment checks pass (**Verify**)

### [✓] TASK-002: Atomic upgrade of all projects and packages to .NET 10.0 *(Completed: 2025-12-01 20:46)*
**References**: Plan §12 Phase 1, Plan §5 Package Update Reference, Plan §6 Breaking Changes Catalog

- [✓] (1) Update `<TargetFramework>` to `net10.0` in all 6 project files per Plan §12 Phase 1
- [✓] (2) Update all package references to 10.0.0 versions as listed in Plan §5
- [✓] (3) Restore dependencies for the solution
- [✓] (4) Build the entire solution and fix all compilation errors per Plan §6
- [✓] (5) Rebuild solution in Release configuration
- [✓] (6) Solution builds with 0 errors (**Verify**)

### [▶] TASK-003: Automated test execution and commit upgraded solution
**References**: Plan §12 Phase 2, Plan §7 Testing and Validation, Plan §9.3 Commit Strategy

- [ ] (1) Run all unit tests in `TriviaSpark.Core.Tests` project
- [ ] (2) If test failures occur, fix issues related to upgrade per Plan §6, then re-run tests once to confirm all pass (**Verify**)
- [ ] (3) Run automated integration tests for TriviaSpark.Web and Console applications per Plan §7 (if available)
- [ ] (4) All automated tests and integration checks pass (**Verify**)
- [ ] (5) Commit all changes with message: "Upgrade solution to .NET 10.0"
- [ ] (6) Changes committed successfully (**Verify**)