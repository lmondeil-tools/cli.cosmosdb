# lmondeil.cli.cosmosdb .NET 10.0 Upgrade Tasks

## Overview

This document tracks the execution of the lmondeil.cli.cosmosdb solution upgrade from .NET 7 to .NET 10.0. All 4 projects will be upgraded simultaneously in a single atomic operation, followed by comprehensive testing.

**Progress**: 2/3 tasks complete (67%) ![0%](https://progress-bar.xyz/67)

---

## Tasks

### [✓] TASK-001: Atomic framework and dependency upgrade with compilation fixes *(Completed: 2026-03-31 11:54)*
**References**: Plan §Migration Strategy Phase 1, Plan §Project-by-Project Plans, Plan §Package Update Reference

- [✓] (1) Update TargetFramework to net10.0 in all 4 project files per Plan §Project-by-Project Plans (lmondeil.cli.cosmosdb.models, lmondeil.cli.cosmosdb.services, lmondeil.cli.cosmosdb, lmondeil.cli.cosmosdb.services.tests)
- [✓] (2) All project files updated to net10.0 (**Verify**)
- [✓] (3) Update package references per Plan §Package Update Reference (Microsoft.Extensions.Hosting 7.0.0→10.0.5, Microsoft.Extensions.Http 7.0.0→10.0.5, Microsoft.Extensions.Logging.Abstractions 7.0.0→10.0.5, Newtonsoft.Json 10.0.3→13.0.4)
- [✓] (4) Replace or remove deprecated Serilog.Sinks.ColoredConsole per Plan §Package Update Reference §Deprecated Package Handling
- [✓] (5) All package references updated (**Verify**)
- [✓] (6) Restore all dependencies for entire solution
- [✓] (7) All dependencies restored successfully (**Verify**)
- [✓] (8) Build entire solution and fix all compilation errors per Plan §Breaking Changes Catalog
- [✓] (9) Solution builds with 0 errors (**Verify**)

---

### [✓] TASK-002: Run full test suite and validate upgrade *(Completed: 2026-03-31 11:55)*
**References**: Plan §Testing & Validation Strategy, Plan §Migration Strategy Phase 2

- [✓] (1) Run tests in lmondeil.cli.cosmosdb.services.tests project
- [✓] (2) Fix any test failures (reference Plan §Breaking Changes Catalog for common issues)
- [✓] (3) Re-run tests after fixes
- [✓] (4) All tests pass with 0 failures (**Verify**)

---

### [▶] TASK-003: Final commit
**References**: Plan §Source Control Strategy

- [ ] (1) Commit all changes with message: "Upgrade solution to .NET 10.0 (LTS) - Update all 4 projects from net7.0 to net10.0 - Update NuGet packages: Microsoft.Extensions.Hosting 7.0.0→10.0.5, Microsoft.Extensions.Http 7.0.0→10.0.5, Microsoft.Extensions.Logging.Abstractions 7.0.0→10.0.5, Newtonsoft.Json 10.0.3→13.0.4 - Replace deprecated Serilog.Sinks.ColoredConsole - All unit tests pass - No API incompatibilities found"

---




