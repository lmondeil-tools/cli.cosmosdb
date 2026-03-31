# .NET 10.0 Upgrade Plan

**Solution**: `lmondeil.cli.cosmosdb`  
**Current Framework**: .NET 7  
**Target Framework**: .NET 10.0 (Long Term Support)  
**Branch**: `upgrade-to-NET10`  
**Strategy**: All-At-Once Strategy  

---

## Table of Contents

1. [Executive Summary](#executive-summary)
2. [Migration Strategy](#migration-strategy)
3. [Detailed Dependency Analysis](#detailed-dependency-analysis)
4. [Project-by-Project Plans](#project-by-project-plans)
5. [Package Update Reference](#package-update-reference)
6. [Breaking Changes Catalog](#breaking-changes-catalog)
7. [Testing & Validation Strategy](#testing--validation-strategy)
8. [Risk Management](#risk-management)
9. [Complexity & Effort Assessment](#complexity--effort-assessment)
10. [Source Control Strategy](#source-control-strategy)
11. [Success Criteria](#success-criteria)

---

## Executive Summary

### Solution Overview
- **Solution Name**: lmondeil.cli.cosmosdb
- **Current Target Framework**: .NET 7
- **Target Framework**: .NET 10.0 (Long Term Support)
- **Total Projects**: 4 (all SDK-style)
- **Total Codebase**: 1,237 LOC across 32 files
- **Project Types**: 1 DotNetCoreApp (main), 2 ClassLibrary (services, models), 1 Test project

### Scope Summary
| Metric | Value | Status |
|--------|-------|--------|
| Projects requiring upgrade | 4 | All |
| NuGet packages to update | 4 | Identified |
| Deprecated packages | 1 | Needs replacement |
| Security vulnerabilities | 0 | None |
| API incompatibilities | 0 | None |
| Estimated code changes | Minimal | <1% of codebase |

### Selected Strategy
**All-At-Once Strategy** - All 4 projects upgraded simultaneously in a single atomic operation.

#### Rationale
- ✅ **Small, focused solution** (4 projects)
- ✅ **Clear dependency structure** (3-level linear hierarchy, no cycles)
- ✅ **All SDK-style projects** (consistent format, no legacy conversion needed)
- ✅ **Excellent compatibility** (759 APIs analyzed, 0 incompatibilities)
- ✅ **Low complexity codebase** (1,237 LOC)
- ✅ **No security vulnerabilities** blocking upgrade
- ✅ **Clear upgrade path** for all NuGet packages

This approach allows completing the entire upgrade as a unified operation, minimizing risk from intermediate states and enabling comprehensive testing in a single phase.

### Key Issues Addressed
1. **4 NuGet packages** require version updates (Microsoft.Extensions.*, Newtonsoft.Json)
2. **1 deprecated package** (Serilog.Sinks.ColoredConsole) requires replacement
3. **All 4 projects** need TargetFramework update from net7.0 to net10.0

### Critical Success Factors
- All projects build successfully with zero errors
- All package updates compatible with .NET 10.0
- All unit tests pass after upgrade
- No breaking changes in APIs (assessment indicates 0 incompatibilities)

---

## Migration Strategy

### Approach: All-At-Once Strategy

**Definition**: All 4 projects are upgraded to .NET 10.0 simultaneously in a single coordinated operation. No intermediate multi-targeted or partially-upgraded states.

**Rationale**:
- Small solution (4 projects) enables comprehensive atomic upgrade
- Clear linear dependency structure (no complex chains)
- Excellent compatibility assessment (0 API incompatibilities detected)
- All packages have defined upgrade paths
- Single testing phase after all updates applied
- Fastest time to completion while maintaining safety

### Execution Approach

#### Phase 1: Atomic Upgrade Operation

**All operations performed as single coordinated batch**:

1. **Update Project Files** (all 4 projects simultaneously)
   - Change TargetFramework from `net7.0` to `net10.0`
   - Update any conditional compilation targets if present

2. **Update NuGet Package References** (all packages across all projects)
   - Microsoft.Extensions.* packages: 7.0.0 → 10.0.5
   - Newtonsoft.Json: 10.0.3 → 13.0.4
   - Replace deprecated Serilog.Sinks.ColoredConsole
   - All test framework packages: compatible (no change)

3. **Restore Dependencies**
   - Execute `dotnet restore` for entire solution
   - Verify all package dependencies resolve correctly

4. **Build Solution**
   - Compile all projects with new TargetFramework and packages
   - Capture all compilation errors for systematic resolution

5. **Fix Compilation Errors**
   - Address any breaking changes (expected: minimal based on assessment)
   - Update code patterns affected by framework or package changes
   - Rebuild to verify fixes applied

6. **Verify Build Success**
   - Solution compiles with 0 errors and 0 warnings
   - No unresolved package dependencies
   - All projects produce valid output

#### Phase 2: Test Validation

After successful build:

1. **Run Unit Tests**
   - Execute test project: lmondeil.cli.cosmosdb.services.tests
   - All tests pass
   - Verify no runtime issues from framework/package changes

2. **Comprehensive Validation**
   - No package conflicts
   - No runtime breaking changes
   - Application starts successfully
   - Core functionality verified

### Why Not Incremental?

For this solution, incremental migration is **not necessary** because:
- ✅ Only 4 projects (well under practical limit of 20+ for incremental)
- ✅ Simple dependency structure (linear, no cycles)
- ✅ No integration complexity between projects
- ✅ All-at-once completes faster with equal safety for small solutions
- ✅ Single testing phase is more efficient than phase-by-phase testing

### Success Indicators

**Phase 1 Success**: Solution builds with 0 errors and 0 warnings  
**Phase 2 Success**: All tests pass, application runs normally  
**Overall Success**: Entire upgrade completed in single atomic operation

---

## Detailed Dependency Analysis

### Dependency Graph Summary

```
lmondeil.cli.cosmosdb (DotNetCoreApp - ROOT)
├── lmondeil.cli.cosmosdb.services (ClassLibrary)
│   └── lmondeil.cli.cosmosdb.models (ClassLibrary - LEAF)
└── lmondeil.cli.cosmosdb.models (ClassLibrary - LEAF)

lmondeil.cli.cosmosdb.services.tests (ClassLibrary - TEST)
└── lmondeil.cli.cosmosdb.services
    └── lmondeil.cli.cosmosdb.models
```

### Project Migration Phases

**All projects upgraded simultaneously** (All-At-Once Strategy):

#### Phase 1: Atomic Upgrade (All Projects)
All 4 projects updated to .NET 10.0 in single coordinated operation:

1. **lmondeil.cli.cosmosdb.models** (net7.0 → net10.0)
   - No project dependencies
   - Leaf node in dependency tree
   - Updates: 1 NuGet package (Newtonsoft.Json)

2. **lmondeil.cli.cosmosdb.services** (net7.0 → net10.0)
   - Depends on: lmondeil.cli.cosmosdb.models
   - Updates: 1 NuGet package (Microsoft.Extensions.Logging.Abstractions)

3. **lmondeil.cli.cosmosdb** (net7.0 → net10.0)
   - Main application (root node)
   - Depends on: lmondeil.cli.cosmosdb.services, lmondeil.cli.cosmosdb.models
   - Updates: 3 NuGet packages (Microsoft.Extensions.Hosting, Microsoft.Extensions.Http)
   - Handles deprecated package replacement (Serilog.Sinks.ColoredConsole)

4. **lmondeil.cli.cosmosdb.services.tests** (net7.0 → net10.0)
   - Test project
   - Depends on: lmondeil.cli.cosmosdb.services
   - No NuGet updates required (all test packages compatible)

### Critical Path Analysis

**Shortest path to completion**: 1 phase

Since all projects are updated atomically, there is one critical path:
1. Update all project files (TargetFramework: net7.0 → net10.0)
2. Update all NuGet packages to net10.0-compatible versions
3. Restore dependencies
4. Build entire solution (tests breaking changes discovery)
5. Fix all compilation errors
6. Run all tests to verify functionality

### Dependency Constraints

✅ **No circular dependencies** - Linear upgrade path confirmed  
✅ **No complex dependency chains** - Maximum 2 levels of downstream dependencies  
✅ **All SDK-style projects** - Consistent upgrade approach applies to all

---

## Project-by-Project Plans

### Project 1: lmondeil.cli.cosmosdb.models

**Current State**
- Target Framework: net7.0
- Type: ClassLibrary (SDK-style)
- Dependencies: None (leaf node)
- Dependants: 2 (services, main app)
- LOC: 59
- Files: 3

**Target State**
- Target Framework: net10.0
- All packages compatible
- Updated packages: 1 (Newtonsoft.Json)

**Migration Steps**
1. Update TargetFramework to net10.0 in `lmondeil.cli.cosmosdb.models.csproj`
2. Update NuGet package: Newtonsoft.Json from 10.0.3 to 13.0.4
3. Restore dependencies
4. Build project (expect: success, no breaking changes)
5. Validate: Build success, 0 errors/warnings

**Expected Breaking Changes**: None (all APIs compatible per assessment)

---

### Project 2: lmondeil.cli.cosmosdb.services

**Current State**
- Target Framework: net7.0
- Type: ClassLibrary (SDK-style)
- Dependencies: 1 (lmondeil.cli.cosmosdb.models)
- Dependants: 2 (main app, tests)
- LOC: 308
- Files: 6

**Target State**
- Target Framework: net10.0
- Updated packages: 1 (Microsoft.Extensions.Logging.Abstractions)

**Migration Steps**
1. Update TargetFramework to net10.0 in `lmondeil.cli.cosmosdb.services.csproj`
2. Update NuGet package: Microsoft.Extensions.Logging.Abstractions from 7.0.0 to 10.0.5
3. Build project (expect: success, no breaking changes)
4. Validate: Build success, 0 errors/warnings

**Expected Breaking Changes**: None (all APIs compatible per assessment)

---

### Project 3: lmondeil.cli.cosmosdb (Main Application)

**Current State**
- Target Framework: net7.0
- Type: DotNetCoreApp (SDK-style)
- Dependencies: 2 (lmondeil.cli.cosmosdb.models, lmondeil.cli.cosmosdb.services)
- Dependants: None (root application)
- LOC: 830
- Files: 22

**Target State**
- Target Framework: net10.0
- Updated packages: 3 (Microsoft.Extensions.Hosting, Microsoft.Extensions.Http, others)
- Deprecated package replacement: Serilog.Sinks.ColoredConsole

**Migration Steps**
1. Update TargetFramework to net10.0 in `lmondeil.cli.cosmosdb.csproj`
2. Update NuGet packages:
   - Microsoft.Extensions.Hosting: 7.0.0 → 10.0.5
   - Microsoft.Extensions.Http: 7.0.0 → 10.0.5
3. Replace deprecated package: Serilog.Sinks.ColoredConsole (consider alternative colored console sink or remove if not essential)
4. Build project (expect: success, no breaking changes)
5. Validate: Build success, 0 errors/warnings

**Expected Breaking Changes**: 
- Serilog.Sinks.ColoredConsole is deprecated; need to replace or remove
- Microsoft.Extensions packages: verify no configuration pattern changes

---

### Project 4: lmondeil.cli.cosmosdb.services.tests

**Current State**
- Target Framework: net7.0
- Type: ClassLibrary (SDK-style) - Test Project
- Dependencies: 1 (lmondeil.cli.cosmosdb.services)
- Dependants: None
- LOC: 40
- Files: 1

**Target State**
- Target Framework: net10.0
- No package updates required (all compatible)

**Migration Steps**
1. Update TargetFramework to net10.0 in `lmondeil.cli.cosmosdb.services.tests.csproj`
2. Build project (expect: success, all test framework packages compatible)
3. Run tests (expect: all pass)
4. Validate: Build success, all tests pass

**Expected Breaking Changes**: None (test frameworks compatible)

---

## Package Update Reference

### Summary of NuGet Changes

| Status | Count | Packages |
|--------|-------|----------|
| ✅ No update needed | 9 | coverlet.collector, FluentAssertions, McMaster.Extensions.Hosting.CommandLine, Microsoft.Azure.Cosmos, Microsoft.NET.Test.Sdk, MSTest.TestAdapter, MSTest.TestFramework, Serilog.Extensions.Hosting, Serilog.Sinks.File |
| 🔄 Update required | 4 | Microsoft.Extensions.Hosting, Microsoft.Extensions.Http, Microsoft.Extensions.Logging.Abstractions, Newtonsoft.Json |
| ⚠️ Deprecated | 1 | Serilog.Sinks.ColoredConsole (requires replacement or removal) |
| ***Total*** | ***14*** | |

### Critical NuGet Updates

#### 1. Microsoft.Extensions.Hosting
- **Current**: 7.0.0
- **Target**: 10.0.5
- **Project**: lmondeil.cli.cosmosdb
- **Reason**: Framework compatibility (core dependency for .NET 10.0 hosting)
- **Breaking Changes**: None expected (provider implementations generally stable)
- **Action**: Direct update to 10.0.5

#### 2. Microsoft.Extensions.Http
- **Current**: 7.0.0
- **Target**: 10.0.5
- **Project**: lmondeil.cli.cosmosdb
- **Reason**: Framework compatibility
- **Breaking Changes**: None expected
- **Action**: Direct update to 10.0.5

#### 3. Microsoft.Extensions.Logging.Abstractions
- **Current**: 7.0.0
- **Target**: 10.0.5
- **Project**: lmondeil.cli.cosmosdb.services
- **Reason**: Framework compatibility
- **Breaking Changes**: None expected (interface changes unlikely)
- **Action**: Direct update to 10.0.5

#### 4. Newtonsoft.Json
- **Current**: 10.0.3 ⚠️ (Very old version)
- **Target**: 13.0.4
- **Project**: lmondeil.cli.cosmosdb.models
- **Reason**: Framework compatibility + substantial bug fixes and improvements
- **Breaking Changes**: Possible API changes (older → current is large jump)
- **Action**: Update with verification of JsonConvert API usage
- **Review Areas**: Check for deprecated properties/methods usage

### Deprecated Package Handling

#### Serilog.Sinks.ColoredConsole (v3.0.1)
- **Status**: Deprecated
- **Project**: lmondeil.cli.cosmosdb
- **Current Impact**: Package exists but is no longer maintained
- **Options**:
  1. **Remove** - If colored console output not essential
  2. **Replace** - Use Serilog.Sinks.Console with colored output configuration (net6.0+ supports ANSI colors)
  3. **Alternative** - Consider Serilog.Sinks.Async or other maintained sinks

**Recommendation**: Review usage in Program.cs/Configuration. For .NET 10.0, standard Console sink with ANSI color support is sufficient. Consider replacement with `Serilog.Sinks.Console` if colored output needed.

### Compatible Packages (No Update)

These packages are compatible with .NET 10.0 and require no changes:

- **coverlet.collector** 6.0.0 - Code coverage tool
- **FluentAssertions** 6.12.0 - Assertion framework
- **McMaster.Extensions.Hosting.CommandLine** 4.0.2 - Command-line parsing
- **Microsoft.Azure.Cosmos** 3.33.0 - Azure Cosmos DB SDK
- **Microsoft.NET.Test.Sdk** 17.7.2 - Test framework
- **MSTest.TestAdapter** 3.1.1 - Test adapter
- **MSTest.TestFramework** 3.1.1 - Test framework
- **Serilog.Extensions.Hosting** 5.0.1 - Serilog hosting
- **Serilog.Sinks.File** 5.0.0 - Serilog file sink

---

## Breaking Changes Catalog

### Framework Breaking Changes (.NET 7 → .NET 10)

#### Known Compatibility Areas
- ✅ **Async/await patterns** - No breaking changes
- ✅ **LINQ expressions** - No breaking changes
- ✅ **Dependency Injection** - No breaking changes
- ✅ **Configuration system** - No breaking changes
- ✅ **JSON serialization** - No breaking changes (unless using System.Text.Json advanced features)

#### Specific .NET 10 Considerations
| Area | Change | Impact | Mitigation |
|------|--------|--------|-----------|
| .NET Runtime | Version 10.0 LTS | Low | No code changes required |
| C# Language | C# 14 features available | Low | Optional to use; existing code unaffected |
| NativeAOT | Improved support | Low | Not applicable unless using AOT compilation |
| Trimming | Enhanced | Low | Not applicable for standard applications |

### Package Breaking Changes

#### Microsoft.Extensions.* (7.0 → 10.0)
**Status**: Generally stable API surface

| Package | Breaking Changes | Likelihood | Mitigation |
|---------|-----------------|------------|-----------|
| Microsoft.Extensions.Hosting | None expected | Low | Direct update should work |
| Microsoft.Extensions.Http | None expected | Low | Direct update should work |
| Microsoft.Extensions.Logging.Abstractions | None expected | Low | Direct update should work |

**Verification Points**:
- IHostBuilder usage
- HttpClient factory registration
- ILogger interface usage

#### Newtonsoft.Json (10.0.3 → 13.0.4)
**Status**: Major version jump (3 major versions)

| Item | Status | Action |
|------|--------|--------|
| JsonConvert API | Stable | Verify usage in code |
| JsonProperty attributes | Stable | Check custom serialization settings |
| Settings object | Possible changes | Review JsonSerializerSettings usage |
| Null handling | Changed in v11 | Verify NullValueHandling behavior |

**Review Checklist**:
- [ ] Search code for `JsonConvert.` usage
- [ ] Check for `JsonProperty` attributes on model properties
- [ ] Verify any custom `JsonSerializerSettings` configurations
- [ ] Test serialization/deserialization of models in lmondeil.cli.cosmosdb.models

#### Serilog.Sinks.ColoredConsole (Deprecated)
**Status**: Package no longer maintained

**Impact**: 
- ⚠️ Will continue to work but receives no updates
- ⚠️ Potential incompatibility with future .NET versions

**Resolution Required**: 
- Option 1: Remove if colored output not essential
- Option 2: Replace with `Serilog.Sinks.Console` (supports ANSI colors in .NET 10)
- Option 3: Find alternative community sink

### Dependency Compatibility Matrix

✅ All 4 projects can be built against .NET 10.0  
✅ All NuGet packages have .NET 10 compatible versions  
✅ No circular dependency issues  
✅ No assembly version conflicts expected

### Assessment Validation

Per the comprehensive assessment:
- **API Compatibility Score**: 759 APIs analyzed, **0 incompatibilities** found
- **Risk Level**: 🟢 **Low** (no binary or source incompatibilities detected)
- **Estimated Code Changes**: <1% of codebase

This indicates a smooth upgrade path with minimal code modifications required.

---

## Testing & Validation Strategy

### Multi-Level Validation Approach

#### Level 1: Build Verification

**After atomic upgrade and dependency restoration**:

```
Step 1: Build entire solution
Expected: 0 errors, 0 warnings
Command: dotnet build --configuration Release
```

**Validation Checklist**:
- [ ] All 4 projects compile successfully
- [ ] No unresolved package dependencies
- [ ] No project reference errors
- [ ] All output binaries generated
- [ ] No warnings in output

**If Build Fails**:
- Capture full error output
- Identify affected projects
- Reference Breaking Changes Catalog for API changes
- Apply targeted fixes to specific projects
- Rebuild and repeat until success

#### Level 2: Unit Test Execution

**After build success**:

```
Step 2: Run test project
Expected: All tests pass
Command: dotnet test --configuration Release
Project: tests\lmondeil.cli.cosmosdb.services.tests
```

**Test Coverage**:
- Services functionality (lmondeil.cli.cosmosdb.services.tests)
- Model serialization (includes Newtonsoft.Json usage)
- Any mocking/async patterns

**Validation Checklist**:
- [ ] All unit tests pass
- [ ] No test runtime errors
- [ ] No flaky or intermittent failures
- [ ] Code coverage maintained
- [ ] Test assertions still valid

**If Tests Fail**:
- Analyze failure messages
- Determine if framework or package API change
- Update test or implementation code as needed
- Re-run until all pass

#### Level 3: Application Functional Verification

**After test success**:

```
Step 3: Verify application startup and basic functionality
Expected: Application starts normally, core features work
```

**Verification Points**:
1. **Application Startup**
   - [ ] Application initializes without errors
   - [ ] No unhandled exceptions in startup sequence
   - [ ] Dependency injection container resolves all services

2. **Configuration & Logging**
   - [ ] Configuration loaded from appsettings.json
   - [ ] Logging initialized and output visible
   - [ ] Console output displays correctly (with/without colors)

3. **Core Service Operations**
   - [ ] Main service methods execute without errors
   - [ ] Model serialization works (Newtonsoft.Json)
   - [ ] Azure Cosmos DB connections (if tested) function normally
   - [ ] HTTP client factory works (if used)

4. **Error Handling**
   - [ ] Exception handling patterns work as before
   - [ ] Logging captures errors correctly
   - [ ] No unexpected runtime exceptions

### Performance Validation

After successful upgrade, optionally verify:
- Application startup time (should be similar to .NET 7)
- Runtime performance (typically stable or improved)
- Memory usage patterns (benchmark if critical)
- Response times for core operations

### Validation Sequence

```
┌─────────────────────────────────────────────────────────┐
│  Atomic Upgrade: Update all projects + packages + build │
└─────────────────────┬───────────────────────────────────┘
                      │
                      ▼
┌─────────────────────────────────────────────────────────┐
│  Level 1: Build Solution (0 errors, 0 warnings)        │
└─────────────────────┬───────────────────────────────────┘
                      │ Success
                      ▼
┌─────────────────────────────────────────────────────────┐
│  Level 2: Run Unit Tests (all pass)                     │
└─────────────────────┬───────────────────────────────────┘
                      │ Success
                      ▼
┌─────────────────────────────────────────────────────────┐
│  Level 3: Functional Verification                       │
│  - Application startup                                  │
│  - Core service operations                              │
│  - Configuration & logging                              │
└─────────────────────┬───────────────────────────────────┘
                      │ Success
                      ▼
┌─────────────────────────────────────────────────────────┐
│  ✅ UPGRADE COMPLETE - All validation passed            │
└─────────────────────────────────────────────────────────┘
```

### Success Criteria

| Milestone | Criteria | Status |
|-----------|----------|--------|
| **Build Success** | 0 errors, 0 warnings | Required ✅ |
| **Tests Pass** | 100% test pass rate | Required ✅ |
| **Application Runs** | No startup errors | Required ✅ |
| **Services Functional** | Core features work | Required ✅ |
| **No Regressions** | Same behavior as .NET 7 | Required ✅ |

---

## Risk Management

### Overall Risk Assessment

**Risk Level**: 🟢 **LOW**

**Justification**:
- Small codebase (1,237 LOC) - easy to review and fix
- 0 API incompatibilities detected in assessment
- All packages have upgrade paths available
- No security vulnerabilities blocking upgrade
- Clear dependency structure (no cycles)
- Comprehensive test coverage available

---

### Identified Risks & Mitigation

#### Risk 1: Newtonsoft.Json Major Version Jump (10.0.3 → 13.0.4)
**Severity**: 🟡 **Medium**
**Likelihood**: Low (but needs verification)

**Potential Issues**:
- JSON serialization behavior changes
- Property naming conventions
- Null handling in serialization
- Custom JsonConverter implementations

**Mitigation**:
1. Review all JsonConvert usage before upgrade
2. Run focused tests on model serialization
3. Test JSON deserialization from external sources
4. Verify any custom settings (NullValueHandling, DateFormatString, etc.)

**Rollback**: If critical: revert to version 12.x (compatible with .NET 10)

---

#### Risk 2: Serilog.Sinks.ColoredConsole Deprecation
**Severity**: 🟡 **Medium** (if colored output is critical)
**Likelihood**: High (package is deprecated)

**Potential Issues**:
- Package continues to work but receives no updates
- Future .NET versions might break compatibility
- No alternative sink bundled with Serilog

**Mitigation**:
1. Before upgrade: decide colored console necessity
2. If needed: use Serilog.Sinks.Console with ANSI color codes
3. If not needed: remove ColoredConsole sink, use Console sink only
4. Document the replacement in code comments

**Options**:
- Option A: Remove (simplest)
- Option B: Replace with Console sink (1 line change in configuration)
- Option C: Use community-maintained sink (more complex)

---

#### Risk 3: Configuration Changes in Microsoft.Extensions
**Severity**: 🟢 **Low**
**Likelihood**: Low

**Potential Issues**:
- Extension method names or signatures changed
- Service registration patterns changed
- Configuration reading patterns altered

**Mitigation**:
1. After build, if compilation errors occur, check Breaking Changes Catalog
2. Focus on Program.cs and any service registration code
3. Verify DI container resolves all services in tests

---

#### Risk 4: Azure Cosmos DB SDK Compatibility
**Severity**: 🟢 **Low**
**Likelihood**: Low (SDK is compatible, but verify)

**Current**: Microsoft.Azure.Cosmos 3.33.0 (compatible, no upgrade)

**Mitigation**:
1. Package is marked as compatible
2. If Azure operations fail in tests, check Azure Cosmos DB documentation
3. May need to update if specific .NET 10 features required

---

### Testing Strategy to Mitigate Risks

| Risk | Tested By | Expected Result |
|------|-----------|-----------------|
| Newtonsoft.Json changes | Unit tests + model serialization checks | Tests pass without modification |
| Serilog deprecation | Application startup verification | Logging output appears correctly |
| Microsoft.Extensions changes | Build process + startup verification | Application starts without errors |
| Azure SDK compatibility | Unit tests (if mocked) | Tests pass as before |

### Contingency Plans

#### If Build Fails
1. Capture error messages
2. Check which project(s) have errors
3. Review Breaking Changes Catalog for that error type
4. Apply targeted fix
5. Rebuild incrementally to isolate issues

#### If Tests Fail
1. Identify failing test
2. Review test error message (assertion vs. exception)
3. Determine root cause (framework, package, or logic)
4. Update test or code accordingly
5. Re-run affected tests

#### If Application Won't Start
1. Check startup exceptions
2. Verify dependency injection registration
3. Check configuration file paths
4. Review logging output for hints
5. Comment out features progressively to isolate cause

#### If Serilog Colored Output Breaks
1. Comment out ColoredConsole sink temporarily
2. Verify application works with Console sink
3. Decide: keep Console sink or implement alternative
4. Update Program.cs configuration
5. Test logging output

---

### Success Validation

**After addressing any issues**, confirm:
- ✅ Solution builds with 0 errors, 0 warnings
- ✅ All unit tests pass
- ✅ Application starts without exceptions
- ✅ Logging output appears correctly
- ✅ No runtime errors during functionality tests

If all validations pass, the upgrade is complete.

---

## Complexity & Effort Assessment

### Project Complexity Ratings

| Project | Complexity | Dependencies | Risk | Rationale |
|---------|-----------|--------------|------|-----------|
| lmondeil.cli.cosmosdb.models | 🟢 **Low** | 0 | Low | 59 LOC, no deps, 1 pkg update |
| lmondeil.cli.cosmosdb.services | 🟢 **Low** | 1 | Low | 308 LOC, simple dep tree, 1 pkg update |
| lmondeil.cli.cosmosdb | 🟢 **Low** | 2 | Low | 830 LOC, main app, 3 pkg updates |
| lmondeil.cli.cosmosdb.services.tests | 🟢 **Low** | 1 | Low | 40 LOC, test framework compatible |

### Overall Solution Complexity: 🟢 **LOW**

**Why This Solution is Simple**:
- All projects are small-to-medium (59-830 LOC each)
- Total codebase is 1,237 lines (easily reviewable in one day)
- All SDK-style projects (no legacy format conversion)
- Linear dependency structure (no complex chains)
- 0 API incompatibilities detected
- All packages have clear upgrade paths
- Excellent test coverage available

### Phase Complexity Breakdown

#### Phase 1: Atomic Upgrade
**Complexity**: 🟢 **Low**  
**Scope**: All 4 projects upgraded simultaneously

**Effort Breakdown**:
| Task | Complexity | Effort |
|------|-----------|--------|
| Update 4 project files (TargetFramework) | Trivial | Minutes |
| Update 4 NuGet packages | Trivial | Minutes |
| Initial build (identify issues) | Low | Minutes |
| Fix compilation errors (if any) | Low | 0-15 min (expected: 0-5) |
| Rebuild to verify | Trivial | Minutes |

**Expected Duration**: All-at-once phase takes combined time for all project updates + single build pass

#### Phase 2: Test & Validation
**Complexity**: 🟢 **Low**  
**Scope**: Run tests, verify functionality

**Effort Breakdown**:
| Task | Complexity | Effort |
|------|-----------|--------|
| Run unit tests | Trivial | Minutes |
| Verify application startup | Low | Minutes |
| Test core services | Low | 5-10 min |
| Functional validation | Low | 10-15 min |

**Expected Duration**: Validation phase takes 20-30 minutes total

### Effort Estimation Model

**Note**: The following represents relative complexity scaling only. Actual wall-clock time depends on developer experience, environment setup, and unforeseen issues.

| Aspect | Relative Effort | Notes |
|--------|-----------------|-------|
| **Project file updates** | 1x | 4 files, trivial changes |
| **NuGet updates** | 1x | 4 packages, straightforward |
| **Build & fix errors** | 1-2x | Low expected issues (0 API incompats) |
| **Testing** | 1x | Small test suite (1 test project) |
| **Verification** | 1x | Simple application (no complex features) |
| **TOTAL RELATIVE EFFORT** | **5-6x** | Linear, small projects |

### Dependency Order (All Projects Simultaneously)

Due to All-At-Once strategy, all projects are handled in single atomic operation:

```
┌─────────────────────────────────────────────┐
│  Atomic Upgrade (All 4 Projects)            │
│  - Update all project files                 │
│  - Update all packages                      │
│  - Single build pass                        │
│  - Fix any issues once                      │
└─────────────────────────────────────────────┘
         │
         ▼
┌─────────────────────────────────────────────┐
│  Testing Phase (All Projects)               │
│  - Run all tests                            │
│  - Verify all functionality                 │
└─────────────────────────────────────────────┘
```

### Resource Requirements

**Skill Level Required**:
- 🟢 **Junior/Intermediate** - This upgrade is straightforward enough for developers with basic .NET experience
- No advanced architecture knowledge needed
- Standard project file editing
- Standard NuGet package management

**Tools/Environment**:
- Visual Studio 2022 (latest) or VS Code with C# extension
- .NET SDK 10.0+ installed
- Git (for version control)
- Command-line familiarity (for `dotnet build`, `dotnet test`)

**Time Availability**:
- Can be completed in single working session
- No multi-day coordination needed
- No complex testing infrastructure required

### Quality Metrics

**Code Quality**: Expected to remain stable or improve
- Small codebase review possible
- All existing tests run as validation
- No refactoring needed (unless desired)

**Test Coverage**: Expected to remain stable
- Existing unit tests should still pass
- No new test writing required
- Coverage % should not change

---

## Source Control Strategy

### Branching Strategy

**Current State**:
- Source branch: `main`
- Upgrade branch: `upgrade-to-NET10` (already created)
- Current branch: `upgrade-to-NET10` ✅

**All changes will be committed to** `upgrade-to-NET10` branch.

### Commit Strategy

#### Single Commit Approach (Recommended for All-At-Once)

**Rationale**: All-At-Once strategy performs atomic upgrade, so single commit best represents the operation.

**Commit Message**:
```
Upgrade solution to .NET 10.0 (LTS)

- Update all 4 projects from net7.0 to net10.0
- Update NuGet packages:
  * Microsoft.Extensions.Hosting: 7.0.0 → 10.0.5
  * Microsoft.Extensions.Http: 7.0.0 → 10.0.5
  * Microsoft.Extensions.Logging.Abstractions: 7.0.0 → 10.0.5
  * Newtonsoft.Json: 10.0.3 → 13.0.4
- Replace deprecated Serilog.Sinks.ColoredConsole
- All unit tests pass
- No API incompatibilities found
```

**Files Changed**:
- `src/lmondeil.cli.cosmosdb/lmondeil.cli.cosmosdb.csproj`
- `src/lmondeil.cli.cosmosdb.models/lmondeil.cli.cosmosdb.models.csproj`
- `src/lmondeil.cli.cosmosdb.services/lmondeil.cli.cosmosdb.services.csproj`
- `tests/lmondeil.cli.cosmosdb.services.tests/lmondeil.cli.cosmosdb.services.tests.csproj`
- `src/lmondeil.cli.cosmosdb/Program.cs` (if Serilog config change needed)
- Any other code files affected by breaking changes (expected: 0-1)

### Merge Process

#### Before Merge:
1. ✅ All 4 projects build successfully (0 errors, 0 warnings)
2. ✅ All unit tests pass
3. ✅ Application starts successfully
4. ✅ No regressions in functionality
5. ✅ Code review completed (if required)

#### Merge to Main:
```bash
# Switch to main
git checkout main

# Merge upgrade branch
git merge upgrade-to-NET10

# Push to remote
git push origin main
```

#### Post-Merge:
- Delete upgrade branch (optional)
- Tag release if performing version bump: `git tag v10.0.0`
- Update documentation if needed

### Code Review Checklist

If code review is required, verify:

- [ ] **Project Files Updated Correctly**
  - [ ] All 4 project files have `<TargetFramework>net10.0</TargetFramework>`
  - [ ] No syntax errors in project files
  - [ ] Package versions correctly updated

- [ ] **NuGet Packages**
  - [ ] Microsoft.Extensions.* updated to 10.0.5
  - [ ] Newtonsoft.Json updated to 13.0.4
  - [ ] Deprecated Serilog.Sinks.ColoredConsole replaced or removed
  - [ ] No extra packages accidentally added

- [ ] **Code Changes**
  - [ ] Any required code updates applied (e.g., JsonConvert patterns)
  - [ ] Serilog configuration updated if needed
  - [ ] No breaking change incompatibilities introduced
  - [ ] Comments updated if any patterns changed

- [ ] **Build & Tests**
  - [ ] Solution builds with 0 errors, 0 warnings
  - [ ] All unit tests pass
  - [ ] No test modifications needed (expect: true)

### Rollback Plan

If post-merge issues arise:

1. **Immediate Rollback** (within minutes):
   ```bash
   git revert HEAD
   git push origin main
   ```

2. **Investigation**:
   - Identify specific issue
   - Work on fix in new branch
   - Re-test thoroughly
   - Attempt re-merge

3. **Quick Recovery**:
   - Revert to commit before upgrade
   - Maintains main branch stability
   - Allows parallel investigation in separate branch

### Branch Lifecycle

```
main (stable)
  │
  └──> upgrade-to-NET10 (created: before assessment)
       │
       ├─ Commit: Upgrade to .NET 10.0
       │
       └──> (merge back to main after validation)
            │
            └─ Delete upgrade branch (optional)
```

### Documentation Updates

**If Applicable**:
- [ ] Update README.md with .NET 10.0 requirement
- [ ] Update CONTRIBUTING.md if SDK version required
- [ ] Update CI/CD pipeline to use .NET 10.0
- [ ] Update any deployment documentation

---

## Success Criteria

### Technical Success Criteria

All of the following **MUST** be achieved for the upgrade to be considered complete:

#### 1. Framework Upgrade
- ✅ **All 4 projects** updated to `<TargetFramework>net10.0</TargetFramework>`
- ✅ **No multi-targeting** or conditional framework references remain
- ✅ All projects explicitly set to net10.0 only

#### 2. Build Success
- ✅ Solution builds with **0 errors**
- ✅ Solution builds with **0 warnings**
- ✅ `dotnet build --configuration Release` completes successfully
- ✅ All output binaries generated correctly

#### 3. Package Updates
- ✅ Microsoft.Extensions.Hosting: Updated to 10.0.5
- ✅ Microsoft.Extensions.Http: Updated to 10.0.5
- ✅ Microsoft.Extensions.Logging.Abstractions: Updated to 10.0.5
- ✅ Newtonsoft.Json: Updated to 13.0.4
- ✅ Serilog.Sinks.ColoredConsole: Replaced or removed
- ✅ All package dependencies resolve without conflicts

#### 4. Test Success
- ✅ **All unit tests pass**: `dotnet test` shows 100% pass rate
- ✅ **No test modifications required** (expect 0 test changes)
- ✅ No intermittent or flaky test failures
- ✅ All MSTest tests execute and pass

#### 5. Application Functionality
- ✅ Application starts **without exceptions**
- ✅ Dependency injection container **resolves all services**
- ✅ Configuration system **loads correctly**
- ✅ Logging system **initializes and produces output**
- ✅ Core services **function as expected**
- ✅ No runtime breaking changes detected

#### 6. API Compatibility
- ✅ **No source incompatibilities** encountered (assessment: 0 found)
- ✅ **No binary incompatibilities** (assessment: 0 found)
- ✅ Code compilation requires **0 changes** for API compatibility
- ✅ Optional: Code refactoring/improvement changes are acceptable

#### 7. No Regressions
- ✅ Application behavior **identical to .NET 7 version**
- ✅ Performance characteristics **stable or improved**
- ✅ All existing features **work as before**
- ✅ No new bugs introduced
- ✅ Test coverage **maintained or improved**

### Quality Metrics

| Metric | Requirement | Validation |
|--------|------------|-----------|
| Build Errors | **0** | `dotnet build` log |
| Build Warnings | **0** | `dotnet build` log |
| Test Pass Rate | **100%** | `dotnet test` output |
| API Incompatibilities | **0** | Compilation + Assessment |
| Runtime Errors | **0** | Application startup |
| Code Changes Required | **Minimal** (<1% LOC) | Code review |

### Completion Checklist

Before declaring upgrade complete, verify all items:

- [ ] **Atomic Upgrade Performed**
  - [ ] All 4 projects' TargetFramework updated
  - [ ] All NuGet packages updated
  - [ ] Single coordinated build pass executed
  - [ ] All changes in single commit

- [ ] **Build Validation**
  - [ ] `dotnet build` produces 0 errors
  - [ ] `dotnet build` produces 0 warnings
  - [ ] All 4 projects compile successfully
  - [ ] No dependency conflicts

- [ ] **Test Validation**
  - [ ] `dotnet test` shows all tests pass
  - [ ] Test project targets net10.0
  - [ ] No test code modifications needed
  - [ ] Coverage maintained

- [ ] **Functional Validation**
  - [ ] Application starts without exceptions
  - [ ] All services initialize
  - [ ] Logging functions properly
  - [ ] Core operations work correctly

- [ ] **Breaking Changes Addressed**
  - [ ] All compilation errors resolved
  - [ ] Serilog deprecated package handled
  - [ ] Newtonsoft.Json usage verified
  - [ ] Microsoft.Extensions usage verified

- [ ] **Source Control**
  - [ ] All changes committed to `upgrade-to-NET10` branch
  - [ ] Commit message descriptive and complete
  - [ ] Ready for merge to `main`

- [ ] **Documentation Updated** (if applicable)
  - [ ] README.md updated with .NET 10.0
  - [ ] Contributing guide updated if needed
  - [ ] CI/CD pipeline configured for .NET 10.0

### Post-Upgrade Validation

After merge to main:

1. **CI/CD Validation**
   - ✅ All automated tests pass
   - ✅ Build pipeline succeeds
   - ✅ Deployment pipeline validates

2. **Stakeholder Validation**
   - ✅ Application deployed to test environment
   - ✅ Application tested by QA (if applicable)
   - ✅ No unexpected runtime issues

3. **Documentation**
   - ✅ All documentation reflects .NET 10.0
   - ✅ Examples updated if needed
   - ✅ API documentation current

### Success Declaration

**The upgrade is SUCCESSFUL when**:

✅ All technical criteria met (sections 1-7 above)  
✅ All quality metrics achieved  
✅ All checklist items verified  
✅ All tests pass  
✅ All validations complete  

**Expected Outcome**: Production-ready .NET 10.0 solution with identical functionality and improved platform capabilities.

---
