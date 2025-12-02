# TriviaSpark .NET 10 Migration Plan

## 1. Executive Summary

### Scenario
Upgrade all projects in the TriviaSpark solution from .NET 9.0 to .NET 10.0 (Preview).

### Scope
- **Total Projects**: 6
- **Current State**: All projects targeting net9.0
- **Target State**: All projects targeting net10.0

**Projects:**
1. HttpClientDecorator - Class Library (222 LOC)
2. TriviaSpark.Core - Class Library (1,937 LOC)
3. JeopardyData.Console - Console Application (76 LOC)
4. TriviaSpark.Console - Console Application (3 LOC)
5. TriviaSpark.Core.Tests - Test Project (908 LOC)
6. TriviaSpark.Web - ASP.NET Core Web Application (6,905 LOC)

**Total Lines of Code**: ~10,051

### Selected Strategy
**Big Bang Strategy** - All projects upgraded simultaneously in a single atomic operation.

**Rationale**:
- Small solution (6 projects)
- All projects currently on .NET 9.0
- Clear, simple dependency structure with minimal complexity
- Total codebase manageable size (~10K LOC)
- All required packages have .NET 10.0 versions available
- No security vulnerabilities detected
- Clean upgrade path from .NET 9 to .NET 10

### Complexity Assessment
**Overall Complexity: Low**

**Justification**:
- Straightforward framework version increment (9.0 → 10.0)
- All Microsoft packages have direct version updates available
- No complex dependency chains or circular dependencies
- Well-structured solution with clear separation of concerns
- No legacy .NET Framework projects to convert
- Test coverage exists for validation

### Critical Issues
**None** - No security vulnerabilities or blocking issues detected in the current package dependencies.

### Recommended Approach
Big Bang migration - update all projects and packages simultaneously, followed by comprehensive testing.

---

## 2. Migration Strategy

### 2.1 Approach Selection

**Chosen Strategy**: Big Bang Strategy

**Justification**:
- **Project Count**: Only 6 projects - ideal for simultaneous upgrade
- **Dependency Depth**: Simple 2-level dependency hierarchy
- **Codebase Size**: ~10K LOC total - manageable in single operation
- **Framework Jump**: Minor version increment (9.0 → 10.0) with minimal breaking changes expected
- **Package Compatibility**: All packages have confirmed .NET 10.0 versions
- **Risk Level**: Low - straightforward upgrade path

The Big Bang approach minimizes total migration time and avoids complexity of maintaining multi-targeted projects during migration.

### 2.2 Dependency-Based Ordering

**Dependency Analysis**:

The solution has a clear dependency hierarchy:

```
Level 1 (Foundation - No Dependencies):
  └─ HttpClientDecorator.csproj
  └─ JeopardyData.Console.csproj (independent)

Level 2 (Mid-Tier):
  └─ TriviaSpark.Core.csproj (depends on HttpClientDecorator)

Level 3 (Applications & Tests):
  └─ TriviaSpark.Web.csproj (depends on Core + HttpClientDecorator)
  └─ TriviaSpark.Console.csproj (depends on Core)
  └─ TriviaSpark.Core.Tests.csproj (depends on Core)
```

**Big Bang Execution**: While respecting this hierarchy conceptually, all project file updates and package updates will be performed as a single atomic operation. The dependency order informs our validation strategy but does not split the implementation into phases.

### 2.3 Parallel vs Sequential Execution

**Strategy Considerations**: 
Under Big Bang Strategy, all project framework updates and package updates are performed simultaneously in a single coordinated operation. This is NOT a parallel execution of separate tasks - it's one atomic upgrade operation that updates all projects together.

**Single Atomic Operation**:
- All 6 projects updated to net10.0 simultaneously
- All package references updated in one pass
- Single restore and build operation
- All compilation errors addressed together

**Testing Sequence** (after atomic upgrade completes):
- Unit tests execution (TriviaSpark.Core.Tests)
- Integration validation
- Web application smoke testing

---

## 3. Detailed Dependency Analysis

### 3.1 Dependency Graph Summary

**Migration Execution Structure** (Big Bang Strategy):

**Phase 0: Preparation**
- Verify .NET 10 SDK installation
- Validate environment readiness

**Phase 1: Atomic Upgrade** (Single Operation)
- All projects: Update TargetFramework from net9.0 → net10.0
- All packages: Update to 10.0.0 versions
- Restore dependencies
- Build entire solution
- Fix all compilation errors discovered
- Validate build succeeds

**Phase 2: Validation**
- Execute all tests
- Verify application functionality
- Confirm no regressions

### 3.2 Project Groupings

**Big Bang Strategy Grouping**: All projects upgraded in single atomic operation.

**For organizational clarity, projects categorized by type**:

**Foundation Libraries** (2 projects):
- HttpClientDecorator - Utility library for HTTP operations
- TriviaSpark.Core - Core business logic and data access

**Applications** (3 projects):
- TriviaSpark.Web - Main ASP.NET Core web application
- TriviaSpark.Console - Console application for batch operations
- JeopardyData.Console - Independent data processing console app

**Testing** (1 project):
- TriviaSpark.Core.Tests - Unit tests for core library

**Note**: This grouping is for documentation purposes. Under Big Bang Strategy, all projects are updated simultaneously.

---

## 4. Project-by-Project Migration Plans

### Project: HttpClientDecorator

**Current State**
- **Target Framework**: net9.0
- **Project Type**: Class Library
- **Dependencies**: None (leaf node)
- **Dependants**: TriviaSpark.Core, TriviaSpark.Web
- **Package Count**: 1
- **LOC**: 222
- **Files**: 4

**Target State**
- **Target Framework**: net10.0
- **Updated Packages**: 1

**Migration Steps**

1. **Prerequisites**
   - None - this is a leaf node project with no dependencies
   - .NET 10 SDK must be installed on the system

2. **Framework Update**
   - File: `HttpClientDecorator\HttpClientDecorator.csproj`
   - Change: `<TargetFramework>net9.0</TargetFramework>` → `<TargetFramework>net10.0</TargetFramework>`

3. **Package Updates**

   | Package | Current Version | Target Version | Reason |
   |---------|----------------|----------------|---------|
   | Microsoft.Extensions.Http | 9.0.2 | 10.0.0 | Framework compatibility - required for .NET 10 |

4. **Expected Breaking Changes**
   - Minimal breaking changes expected for Microsoft.Extensions.Http 9.0 → 10.0
   - HTTP client factory patterns remain stable across versions
   - No API surface changes anticipated in this minor version increment
   - Note: Specific breaking changes will be identified during compilation/testing

5. **Code Modifications**
   - Review HttpClient-related implementations for any obsolete API usage
   - Verify dependency injection patterns remain compatible
   - Check for any compiler warnings related to HTTP operations
   - No configuration changes expected for this library project

6. **Testing Strategy**
   - Unit tests: Verify HTTP client decoration functionality (if tests exist)
   - Integration tests: Ensure dependent projects (Core, Web) can properly utilize the decorator
   - Manual testing: Not applicable for library project
   - Performance validation: Monitor HTTP operation performance during dependent project testing

7. **Validation Checklist**
   - [ ] Dependencies resolve correctly
   - [ ] Project builds without errors
   - [ ] Project builds without warnings
   - [ ] All unit tests pass (if applicable)
   - [ ] No security warnings

---

### Project: TriviaSpark.Core

**Current State**
- **Target Framework**: net9.0
- **Project Type**: Class Library
- **Dependencies**: HttpClientDecorator
- **Dependants**: TriviaSpark.Web, TriviaSpark.Console, TriviaSpark.Core.Tests
- **Package Count**: 7 explicit packages
- **LOC**: 1,937
- **Files**: 34

**Target State**
- **Target Framework**: net10.0
- **Updated Packages**: 7

**Migration Steps**

1. **Prerequisites**
   - HttpClientDecorator must be upgraded first (completed in same atomic operation)
   - .NET 10 SDK installed
   - Entity Framework migrations may need attention post-upgrade

2. **Framework Update**
   - File: `TriviaSpark.Core\TriviaSpark.Core.csproj`
   - Change: `<TargetFramework>net9.0</TargetFramework>` → `<TargetFramework>net10.0</TargetFramework>`

3. **Package Updates**

   | Package | Current Version | Target Version | Reason |
   |---------|----------------|----------------|---------|
   | System.Drawing.Common | 9.0.2 | 10.0.0 | Framework compatibility |
   | System.Runtime.Caching | 9.0.2 | 10.0.0 | Framework compatibility |
   | Microsoft.AspNetCore.Identity.EntityFrameworkCore | 9.0.2 | 10.0.0 | Framework compatibility - Identity system |
   | Microsoft.AspNetCore.Identity.UI | 9.0.2 | 10.0.0 | Framework compatibility - Identity UI |
   | Microsoft.EntityFrameworkCore.Sqlite | 9.0.2 | 10.0.0 | Framework compatibility - Data access |
   | Microsoft.EntityFrameworkCore.Tools | 9.0.2 | 10.0.0 | Framework compatibility - Migration tooling |
   | System.Text.Json | 9.0.2 | 10.0.0 | Framework compatibility - Serialization |

4. **Expected Breaking Changes**
   - **Entity Framework Core 10.0**:
     - Potential query behavior refinements
     - Migration generation may produce different output
     - Check for any EF Core interceptor or convention changes
   - **ASP.NET Core Identity 10.0**:
     - Identity API surface generally stable
     - Review any custom UserManager or SignInManager extensions
   - **System.Text.Json 10.0**:
     - Serialization behavior improvements may affect edge cases
     - Review custom JsonConverter implementations
   - **System.Drawing.Common**:
     - Cross-platform considerations (Windows-only warnings may appear)
   - Note: Specific breaking changes discovered during compilation will be addressed

5. **Code Modifications**
   - Review Entity Framework DbContext configurations
   - Check Identity-related service registrations
   - Verify JSON serialization settings and custom converters
   - Review any uses of System.Drawing.Common (consider migration warnings)
   - Update any obsolete API calls flagged by compiler
   - Validate caching implementations using System.Runtime.Caching
   - Check for any breaking changes in LINQ query patterns

6. **Testing Strategy**
   - Unit tests: Execute TriviaSpark.Core.Tests project (908 LOC of tests)
   - Integration tests: Verify data access layer functions correctly
   - Database migrations: Ensure EF migrations work with SQLite
   - Identity operations: Test authentication and authorization flows
   - JSON serialization: Validate serialization/deserialization scenarios
   - Performance benchmarks: Monitor query performance with EF Core 10.0

7. **Validation Checklist**
   - [ ] Dependencies resolve correctly
   - [ ] Project builds without errors
   - [ ] Project builds without warnings
   - [ ] All unit tests in TriviaSpark.Core.Tests pass
   - [ ] Database connection and migrations work
   - [ ] Identity operations function correctly
   - [ ] No security warnings

---

### Project: JeopardyData.Console

**Current State**
- **Target Framework**: net9.0
- **Project Type**: Console Application (DotNetCoreApp)
- **Dependencies**: None (independent project)
- **Dependants**: None
- **Package Count**: 0 explicit packages
- **LOC**: 76
- **Files**: 4

**Target State**
- **Target Framework**: net10.0
- **Updated Packages**: 0 (no package updates required)

**Migration Steps**

1. **Prerequisites**
   - None - independent project with no dependencies
   - .NET 10 SDK must be installed

2. **Framework Update**
   - File: `JeopardyData.Console\JeopardyData.Console.csproj`
   - Change: `<TargetFramework>net9.0</TargetFramework>` → `<TargetFramework>net10.0</TargetFramework>`

3. **Package Updates**
   - No explicit package references require updates
   - Framework reference automatically updates with TargetFramework change

4. **Expected Breaking Changes**
   - Minimal breaking changes expected for simple console application
   - .NET 10 runtime behavior changes may affect console I/O or file operations
   - Review any use of newer C# language features
   - Note: Specific issues discovered during compilation will be addressed

5. **Code Modifications**
   - Review console application entry point (Program.cs)
   - Check for any file I/O operations that may behave differently
   - Verify any command-line argument parsing logic
   - Update any obsolete API calls flagged by compiler

6. **Testing Strategy**
   - Manual testing: Execute console application with typical workflows
   - Integration tests: Verify data processing functionality
   - Performance validation: Ensure processing time remains acceptable
   - Edge case testing: Test with various input scenarios

7. **Validation Checklist**
   - [ ] Dependencies resolve correctly
   - [ ] Project builds without errors
   - [ ] Project builds without warnings
   - [ ] Console application runs successfully
   - [ ] Data processing functionality works correctly
   - [ ] No security warnings

---

### Project: TriviaSpark.Console

**Current State**
- **Target Framework**: net9.0
- **Project Type**: Console Application (DotNetCoreApp)
- **Dependencies**: TriviaSpark.Core
- **Dependants**: None
- **Package Count**: 0 explicit packages
- **LOC**: 3
- **Files**: 1

**Target State**
- **Target Framework**: net10.0
- **Updated Packages**: 0 (inherits from TriviaSpark.Core)

**Migration Steps**

1. **Prerequisites**
   - TriviaSpark.Core must be upgraded first (completed in same atomic operation)
   - .NET 10 SDK installed

2. **Framework Update**
   - File: `TriviaSpark.Console\TriviaSpark.Console.csproj`
   - Change: `<TargetFramework>net9.0</TargetFramework>` → `<TargetFramework>net10.0</TargetFramework>`

3. **Package Updates**
   - No explicit package references
   - Inherits packages transitively from TriviaSpark.Core

4. **Expected Breaking Changes**
   - Inherits any breaking changes from TriviaSpark.Core
   - Console-specific runtime behavior changes in .NET 10
   - Very minimal codebase (3 LOC) reduces risk significantly
   - Note: Specific issues discovered during compilation will be addressed

5. **Code Modifications**
   - Review Program.cs (single file, 3 lines)
   - Verify Core library integration still functions
   - Update any obsolete API calls flagged by compiler
   - Check dependency injection setup if present

6. **Testing Strategy**
   - Manual testing: Execute console application
   - Integration tests: Verify Core library functionality works through console
   - Smoke testing: Ensure application starts and exits cleanly

7. **Validation Checklist**
   - [ ] Dependencies resolve correctly (TriviaSpark.Core)
   - [ ] Project builds without errors
   - [ ] Project builds without warnings
   - [ ] Console application runs successfully
   - [ ] Core library integration works
   - [ ] No security warnings

---

### Project: TriviaSpark.Core.Tests

**Current State**
- **Target Framework**: net9.0
- **Project Type**: Test Project (ClassLibrary with MSTest)
- **Dependencies**: TriviaSpark.Core
- **Dependants**: None
- **Package Count**: 4 explicit packages (all compatible)
- **LOC**: 908
- **Files**: 7

**Target State**
- **Target Framework**: net10.0
- **Updated Packages**: 0 (all test packages already compatible)

**Migration Steps**

1. **Prerequisites**
   - TriviaSpark.Core must be upgraded first (completed in same atomic operation)
   - .NET 10 SDK installed
   - Test runner must support .NET 10

2. **Framework Update**
   - File: `TriviaSpark.Core.Tests\TriviaSpark.Core.Tests.csproj`
   - Change: `<TargetFramework>net9.0</TargetFramework>` → `<TargetFramework>net10.0</TargetFramework>`

3. **Package Updates**

   | Package | Current Version | Target Version | Reason |
   |---------|----------------|----------------|---------|
   | Microsoft.NET.Test.Sdk | 17.13.0 | (no change) | ✅ Compatible with .NET 10 |
   | MSTest.TestAdapter | 3.8.2 | (no change) | ✅ Compatible with .NET 10 |
   | MSTest.TestFramework | 3.8.2 | (no change) | ✅ Compatible with .NET 10 |
   | coverlet.collector | 6.0.4 | (no change) | ✅ Compatible with .NET 10 |

   **Note**: All test framework packages are already compatible with .NET 10.0. No version updates required.

4. **Expected Breaking Changes**
   - Test framework packages are stable and compatible
   - Tests may reveal breaking changes in TriviaSpark.Core after upgrade
   - Test execution behavior should remain consistent
   - Code coverage collection should work unchanged
   - Note: Test failures may indicate issues in Core library, not test project itself

5. **Code Modifications**
   - Review test initialization and cleanup code
   - Verify mock/stub implementations remain compatible
   - Check for any test-specific obsolete API usage
   - Update assertions if Core library API changes require it
   - Validate test data setup for EF Core 10.0 compatibility

6. **Testing Strategy**
   - Execute all 908 LOC of unit tests
   - Verify all tests pass with upgraded Core library
   - Check test execution time for performance regressions
   - Validate code coverage metrics remain consistent
   - Run tests in CI/CD pipeline if available

7. **Validation Checklist**
   - [ ] Dependencies resolve correctly (TriviaSpark.Core)
   - [ ] Project builds without errors
   - [ ] Project builds without warnings
   - [ ] All unit tests execute successfully
   - [ ] All unit tests pass (0 failures)
   - [ ] Code coverage collection works
   - [ ] No security warnings

---

### Project: TriviaSpark.Web

**Current State**
- **Target Framework**: net9.0
- **Project Type**: ASP.NET Core Web Application
- **Dependencies**: TriviaSpark.Core, HttpClientDecorator
- **Dependants**: None (top-level application)
- **Package Count**: 3 explicit packages (2 need updates, 1 compatible)
- **LOC**: 6,905
- **Files**: 152

**Target State**
- **Target Framework**: net10.0
- **Updated Packages**: 2

**Migration Steps**

1. **Prerequisites**
   - TriviaSpark.Core must be upgraded first (completed in same atomic operation)
   - HttpClientDecorator must be upgraded first (completed in same atomic operation)
   - .NET 10 SDK installed
   - ASP.NET Core 10.0 runtime available

2. **Framework Update**
   - File: `TriviaSpark.Web\TriviaSpark.Web.csproj`
   - Change: `<TargetFramework>net9.0</TargetFramework>` → `<TargetFramework>net10.0</TargetFramework>`

3. **Package Updates**

   | Package | Current Version | Target Version | Reason |
   |---------|----------------|----------------|---------|
   | Microsoft.Extensions.Diagnostics.HealthChecks | 9.0.2 | 10.0.0 | Framework compatibility - Health monitoring |
   | Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore | 9.0.2 | 10.0.0 | Framework compatibility - EF health checks |
   | NLog.Web.AspNetCore | 5.4.0 | (no change) | ✅ Compatible with .NET 10 |

4. **Expected Breaking Changes**
   - **ASP.NET Core 10.0**:
     - Middleware registration order changes (review Startup.cs / Program.cs)
     - Authentication/Authorization pipeline modifications
     - Endpoint routing behavior refinements
     - Minimal API improvements (if used)
   - **Health Checks 10.0**:
     - Health check API surface generally stable
     - Review custom health check implementations
   - **Entity Framework (inherited from Core)**:
     - Database health checks may behave differently
   - **Configuration System**:
     - Review appsettings.json and configuration binding
   - **Dependency Injection**:
     - Service registration patterns should remain compatible
   - Note: Specific breaking changes discovered during compilation will be addressed

5. **Code Modifications**
   - Review Program.cs / Startup.cs for ASP.NET Core initialization
   - Check middleware registration order and compatibility
   - Verify authentication and authorization configuration
   - Update any obsolete API calls flagged by compiler
   - Review Razor pages and views for any breaking changes
   - Validate JavaScript interop if using Blazor components
   - Check static file serving and routing configuration
   - Review health check endpoint registrations
   - Verify logging configuration with NLog
   - Update any custom middleware implementations
   - Check API controllers for breaking changes

6. **Testing Strategy**
   - Unit tests: Execute any web-specific unit tests
   - Integration tests: Test HTTP endpoints and middleware pipeline
   - Manual testing: 
     - Application starts successfully
     - Home page loads
     - Authentication/authorization flows work
     - Health check endpoints respond correctly
     - Database operations function properly
   - Performance validation: 
     - Monitor response times
     - Check memory usage
     - Validate concurrent request handling
   - Browser testing: Test UI in target browsers
   - API testing: Verify all API endpoints work correctly

7. **Validation Checklist**
   - [ ] Dependencies resolve correctly (Core, HttpClientDecorator)
   - [ ] Project builds without errors
   - [ ] Project builds without warnings
   - [ ] Web application starts without errors
   - [ ] Health check endpoints respond
   - [ ] Authentication/authorization works
   - [ ] Database connectivity confirmed
   - [ ] All web pages render correctly
   - [ ] API endpoints function properly
   - [ ] Logging works correctly
   - [ ] No security warnings
   - [ ] Performance metrics acceptable

---

## 5. Package Update Reference

### Common Package Updates (affecting multiple projects)

**Note**: Under Big Bang Strategy, all package updates are performed simultaneously in a single atomic operation.

| Package | Current | Target | Projects Affected | Update Reason |
|---------|---------|--------|-------------------|---------------|
| Microsoft.Extensions.Http | 9.0.2 | 10.0.0 | HttpClientDecorator (1) | .NET 10 framework compatibility |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 9.0.2 | 10.0.0 | TriviaSpark.Core (1) | .NET 10 framework compatibility |
| Microsoft.AspNetCore.Identity.UI | 9.0.2 | 10.0.0 | TriviaSpark.Core (1) | .NET 10 framework compatibility |
| Microsoft.EntityFrameworkCore.Sqlite | 9.0.2 | 10.0.0 | TriviaSpark.Core (1) | .NET 10 framework compatibility |
| Microsoft.EntityFrameworkCore.Tools | 9.0.2 | 10.0.0 | TriviaSpark.Core (1) | .NET 10 framework compatibility |
| System.Drawing.Common | 9.0.2 | 10.0.0 | TriviaSpark.Core (1) | .NET 10 framework compatibility |
| System.Runtime.Caching | 9.0.2 | 10.0.0 | TriviaSpark.Core (1) | .NET 10 framework compatibility |
| System.Text.Json | 9.0.2 | 10.0.0 | TriviaSpark.Core (1) | .NET 10 framework compatibility |
| Microsoft.Extensions.Diagnostics.HealthChecks | 9.0.2 | 10.0.0 | TriviaSpark.Web (1) | .NET 10 framework compatibility |
| Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore | 9.0.2 | 10.0.0 | TriviaSpark.Web (1) | .NET 10 framework compatibility |

### Compatible Packages (no update required)

| Package | Current Version | Projects Affected | Compatibility Status |
|---------|----------------|-------------------|---------------------|
| NLog.Web.AspNetCore | 5.4.0 | TriviaSpark.Web (1) | ✅ Compatible with .NET 10 |
| Microsoft.NET.Test.Sdk | 17.13.0 | TriviaSpark.Core.Tests (1) | ✅ Compatible with .NET 10 |
| MSTest.TestAdapter | 3.8.2 | TriviaSpark.Core.Tests (1) | ✅ Compatible with .NET 10 |
| MSTest.TestFramework | 3.8.2 | TriviaSpark.Core.Tests (1) | ✅ Compatible with .NET 10 |
| coverlet.collector | 6.0.4 | TriviaSpark.Core.Tests (1) | ✅ Compatible with .NET 10 |

**Total Packages Requiring Updates**: 10  
**Total Packages Already Compatible**: 5

---

## 6. Breaking Changes Catalog

### Framework Breaking Changes (.NET 9 → .NET 10)

Since .NET 10 is a preview release, breaking changes are still being documented. Key areas to watch:

1. **Runtime Changes**
   - Performance improvements may change timing-sensitive code
   - Garbage collection behavior refinements
   - JIT compilation optimizations

2. **ASP.NET Core Changes**
   - Middleware pipeline optimizations
   - Authentication/authorization enhancements
   - Minimal API improvements
   - Endpoint routing refinements

3. **Entity Framework Core Changes**
   - Query translation improvements
   - Migration generation changes
   - Performance optimizations
   - LINQ provider enhancements

4. **C# Language Features**
   - New C# 13 features available
   - Potential analyzer warnings for new patterns
   - Improved nullable reference type analysis

### Package-Specific Breaking Changes

#### Microsoft.Extensions.Http (9.0.2 → 10.0.0)
- Expected: Minimal breaking changes
- Watch for: HTTP client factory behavior changes
- Action: Review custom HTTP message handlers

#### Entity Framework Core (9.0.2 → 10.0.0)
- Expected: Query behavior improvements
- Watch for: Migration generation differences
- Action: Test all database operations thoroughly

#### ASP.NET Core Identity (9.0.2 → 10.0.0)
- Expected: Stable API surface
- Watch for: Authentication flow changes
- Action: Test login, registration, and authorization

#### System.Text.Json (9.0.2 → 10.0.0)
- Expected: Serialization performance improvements
- Watch for: Edge case behavior changes in custom converters
- Action: Validate all JSON serialization scenarios

#### System.Drawing.Common (9.0.2 → 10.0.0)
- Expected: Cross-platform warnings
- Watch for: Windows-specific functionality warnings
- Action: Consider alternatives for cross-platform scenarios

### Discovery Process

Breaking changes will be discovered during:
1. **Compilation**: Compiler errors and warnings
2. **Testing**: Test failures revealing behavioral changes
3. **Runtime**: Issues discovered during application execution
4. **Performance**: Metrics showing unexpected changes

All breaking changes discovered will be documented and addressed during the atomic upgrade operation.

---

## 7. Testing and Validation Strategy

### Phase-by-Phase Testing

Under Big Bang Strategy, testing occurs after the atomic upgrade operation completes.

#### Post-Upgrade Testing (Phase 2)

**Unit Test Execution**
- Project: TriviaSpark.Core.Tests (908 LOC)
- Scope: All unit tests for core business logic
- Success Criteria: 100% tests pass, no failures
- Duration Estimate: 5-10 minutes

**Integration Testing**
- Database operations (SQLite with EF Core 10.0)
- Identity system (authentication/authorization)
- HTTP client decorator functionality
- Health check endpoints
- Success Criteria: All integration points function correctly

**Application Testing**
- TriviaSpark.Web application startup and operation
- TriviaSpark.Console application execution
- JeopardyData.Console application execution
- Success Criteria: All applications run without errors

### Smoke Tests

**Quick validation after atomic upgrade**:

**Build Validation**
- [ ] Entire solution builds successfully
- [ ] Zero compilation errors
- [ ] Zero warnings (or documented acceptable warnings)
- [ ] All package dependencies resolve correctly

**Application Start Validation**
- [ ] TriviaSpark.Web starts and responds to HTTP requests
- [ ] Health check endpoints return healthy status
- [ ] Database connectivity confirmed
- [ ] Logging system functions

**Core Functionality Smoke Tests**
- [ ] User authentication works
- [ ] Database queries execute
- [ ] HTTP operations function
- [ ] Console applications execute

### Comprehensive Validation

**Before marking migration complete**:

**Automated Testing**
- [ ] All 908 LOC of unit tests pass
- [ ] Code coverage maintained or improved
- [ ] Test execution time acceptable

**Manual Testing**
- [ ] Web application UI loads and functions
- [ ] Authentication flows work (login, logout, register)
- [ ] Trivia game functionality operates correctly
- [ ] Admin/management features accessible
- [ ] Console tools execute successfully

**Performance Metrics**
- [ ] Web application response times within acceptable range
- [ ] Database query performance acceptable
- [ ] Memory usage stable
- [ ] No performance regressions detected

**Security Validation**
- [ ] No new security warnings from packages
- [ ] Authentication and authorization work correctly
- [ ] HTTPS configuration valid
- [ ] Health check endpoints don't expose sensitive data

**Cross-Cutting Concerns**
- [ ] Logging captures appropriate information
- [ ] Error handling functions correctly
- [ ] Configuration system loads settings properly
- [ ] Dependency injection resolves all services

---

## 8. Risk Management

### 8.1 High-Risk Changes

**Strategy Risk Factors**: Big Bang Strategy concentrates risk in a single atomic operation, requiring thorough testing after the upgrade.

| Project | Risk Level | Risk Description | Mitigation Strategy |
|---------|-----------|------------------|---------------------|
| TriviaSpark.Web | Medium | Large codebase (6,905 LOC), ASP.NET Core breaking changes | Comprehensive manual and automated testing; review middleware and authentication carefully |
| TriviaSpark.Core | Medium | Core business logic (1,937 LOC), EF Core changes may affect data access | Execute all unit tests (908 LOC); test database operations thoroughly |
| TriviaSpark.Core.Tests | Low | Test project - failures indicate Core issues | Use test results as primary validation mechanism |
| HttpClientDecorator | Low | Small, focused library (222 LOC) | Integration testing through dependent projects |
| TriviaSpark.Console | Low | Minimal code (3 LOC) | Quick smoke testing |
| JeopardyData.Console | Low | Independent, small app (76 LOC) | Manual execution validation |

**Overall Risk Level: Low-Medium**
- Straightforward version increment (9.0 → 10.0)
- No security vulnerabilities to address
- Good test coverage exists
- Clear dependency structure
- All packages have .NET 10 versions available

### 8.2 Contingency Plans

#### Contingency: Big Bang Strategy Challenges

**Challenge 1: Multiple Compilation Errors After Atomic Update**
- **Scenario**: Upgrading all projects simultaneously reveals widespread breaking changes
- **Indicators**: 
  - More than 50 compilation errors across projects
  - Errors span multiple categories (API changes, configuration, etc.)
  - Error fixes create cascading issues
- **Mitigation**:
  - Prioritize errors by project dependency order (HttpClientDecorator → Core → Web)
  - Group similar errors and fix by category
  - Use compiler error guidance and breaking change documentation
  - If errors exceed 2 hours of troubleshooting, consider rollback and incremental approach

**Challenge 2: Test Failures Reveal Deep Behavioral Changes**
- **Scenario**: Unit/integration tests fail after upgrade due to framework behavior changes
- **Indicators**:
  - More than 20% of tests fail
  - Failures indicate logic changes, not just API updates
  - Test fixes require significant refactoring
- **Mitigation**:
  - Analyze test failures by category
  - Consult .NET 10 breaking changes documentation
  - Engage with .NET community forums (preview version)
  - Consider deferring upgrade if behavioral changes are too significant

**Challenge 3: Performance Degradation**
- **Scenario**: Application performance degrades after upgrade
- **Indicators**:
  - Response times increase > 20%
  - Memory usage increases significantly
  - Database queries slower
- **Mitigation**:
  - Profile application to identify bottlenecks
  - Review EF Core 10.0 query changes
  - Check for new warnings about inefficient patterns
  - Consult .NET 10 performance documentation

**Challenge 4: Third-Party Package Incompatibility**
- **Scenario**: NLog.Web.AspNetCore or other compatible packages have runtime issues
- **Indicators**:
  - Builds succeed but runtime exceptions occur
  - Logging or other features fail silently
- **Mitigation**:
  - Check package GitHub issues for .NET 10 compatibility
  - Look for preview/beta versions supporting .NET 10
  - Temporarily disable problematic features
  - Contact package maintainers for guidance

### 8.3 Rollback Strategy

**Rollback Trigger Conditions**:
- Critical compilation errors cannot be resolved within 3 hours
- More than 30% of tests fail after fixes attempted
- Blocking runtime issues discovered in .NET 10 preview
- Performance degradation > 30% with no clear resolution
- Security issues introduced by .NET 10 preview

**Rollback Procedure**:

1. **Git Branch Rollback** (Recommended - Clean State)
   ```
   git checkout main
   git branch -D upgrade-to-NET10
   ```
   - Immediately returns solution to .NET 9 state
   - All changes discarded
   - Can restart upgrade with different strategy if needed

2. **Git Revert** (If commits made to upgrade branch)
   ```
   git log  # Identify commit hashes
   git revert <commit-hash-N> <commit-hash-N-1> ... <commit-hash-1>
   ```
   - Reverts commits in reverse order
   - Preserves history
   - May require manual conflict resolution

3. **Post-Rollback Validation**
   - [ ] Solution builds on main branch
   - [ ] All tests pass
   - [ ] Applications run correctly
   - [ ] Confirm .NET 9 SDK active

4. **Alternative Strategy Evaluation**
   - Document specific issues encountered
   - Assess if incremental migration would help
   - Consider waiting for .NET 10 GA release
   - Evaluate if staying on .NET 9 LTS is preferable

**Recovery Time Objective**: < 15 minutes for rollback to main branch

---

## 9. Source Control Strategy

### 9.1 Strategy-Specific Guidance

**Big Bang Strategy Source Control Approach**:

Since all project updates occur in a single atomic operation, the recommended source control strategy is:

**Single Commit Approach** (Recommended):
- Perform all framework and package updates
- Build and fix all compilation errors
- Execute and pass all tests
- Commit everything as one atomic change
- Commit message: "Upgrade solution to .NET 10.0"

**Rationale**:
- Reflects the atomic nature of Big Bang strategy
- Creates a clean, reviewable single diff
- Easy to rollback if issues discovered
- Simplifies code review process
- Matches the "all at once" philosophy

**Alternative: Two-Commit Approach** (If preferred):
1. First commit: Framework and package updates only
2. Second commit: Compilation error fixes and test adjustments

### 9.2 Branching Strategy

**Upgrade Branch**: `upgrade-to-NET10` (already created)
- Source: `main` branch
- Purpose: Isolate .NET 10 upgrade work
- Lifespan: Until upgrade complete and validated

**Integration Approach**:
- Complete all upgrade work on `upgrade-to-NET10` branch
- Validate all tests pass and applications run
- Create Pull Request from `upgrade-to-NET10` → `main`
- Conduct code review
- Merge to `main` after approval

### 9.3 Commit Strategy

**Default: Single Commit After Complete Atomic Upgrade**

**Recommended Commit Structure**:

```
Commit 1 (after all work complete):
  Title: "Upgrade solution to .NET 10.0"
  
  Body:
  - Updated all 6 projects from net9.0 to net10.0
  - Updated 10 NuGet packages to version 10.0.0
  - Fixed [N] compilation errors related to breaking changes
  - All tests pass (TriviaSpark.Core.Tests)
  - Applications validated: Web, Console, JeopardyData.Console
  
  Breaking changes addressed:
  - [List specific breaking changes fixed]
  
  Files changed: [~15 .csproj files and code files]
```

**Alternative: Multiple Checkpoint Commits** (if needed):

Only create multiple commits if:
- Upgrade work spans multiple work sessions
- Team prefers more granular history
- Intermediate validation checkpoints desired

**Checkpoint Commit Pattern**:
1. "Upgrade: Update all project files to net10.0"
2. "Upgrade: Update all NuGet packages to 10.0.0"
3. "Upgrade: Fix compilation errors in [ProjectName]"
4. "Upgrade: Validate tests and applications"

**Commit Best Practices**:
- Include specific .NET version (10.0) in commit message
- Reference any breaking changes addressed
- Note test execution results
- Keep commits on `upgrade-to-NET10` branch until fully validated

### 9.4 Review and Merge Process

**Pull Request Requirements**:
- Title: "Upgrade solution from .NET 9 to .NET 10"
- Description: Include summary of changes, packages updated, tests passed
- Reviewers: Senior developers familiar with the codebase
- Checklist:
  - [ ] All projects build without errors
  - [ ] All projects build without warnings
  - [ ] All unit tests pass (TriviaSpark.Core.Tests)
  - [ ] Web application starts and runs
  - [ ] Console applications execute successfully
  - [ ] No new security warnings
  - [ ] Performance acceptable

**Review Checklist Items**:
- [ ] Project files updated correctly (TargetFramework)
- [ ] Package versions appropriate (10.0.0)
- [ ] Breaking changes handled properly
- [ ] No debugging code or temporary fixes left
- [ ] Comments explain non-obvious changes
- [ ] Documentation updated if needed

**Merge Criteria**:
- All PR checks pass (build, tests)
- At least one approval from reviewer
- No unresolved comments
- All validation criteria met

**Integration Validation**:
After merge to `main`:
- [ ] CI/CD pipeline builds successfully
- [ ] Automated tests pass in CI
- [ ] Deployment to staging environment successful (if applicable)
- [ ] Smoke tests pass in staging
- [ ] Team notified of upgrade completion

**Post-Merge Cleanup**:
```bash
# After successful merge
git checkout main
git pull origin main
git branch -d upgrade-to-NET10  # Delete local branch
```

---

## 10. Success Criteria

### 10.1 Strategy-Specific Success Criteria

**Big Bang Strategy Success Indicators**:
- [ ] All 6 projects upgraded to net10.0 in single atomic operation
- [ ] All 10 package updates applied simultaneously
- [ ] Entire solution builds successfully after atomic upgrade
- [ ] All compilation errors resolved in one pass
- [ ] No multi-targeting or intermediate states required
- [ ] Total upgrade time minimized (target: < 4 hours)

### 10.2 Technical Success Criteria

**Project Framework Updates**:
- [ ] HttpClientDecorator: net9.0 → net10.0 ✅
- [ ] TriviaSpark.Core: net9.0 → net10.0 ✅
- [ ] JeopardyData.Console: net9.0 → net10.0 ✅
- [ ] TriviaSpark.Console: net9.0 → net10.0 ✅
- [ ] TriviaSpark.Core.Tests: net9.0 → net10.0 ✅
- [ ] TriviaSpark.Web: net9.0 → net10.0 ✅

**Package Updates Applied**:
- [ ] Microsoft.Extensions.Http: 9.0.2 → 10.0.0 ✅
- [ ] System.Drawing.Common: 9.0.2 → 10.0.0 ✅
- [ ] System.Runtime.Caching: 9.0.2 → 10.0.0 ✅
- [ ] Microsoft.AspNetCore.Identity.EntityFrameworkCore: 9.0.2 → 10.0.0 ✅
- [ ] Microsoft.AspNetCore.Identity.UI: 9.0.2 → 10.0.0 ✅
- [ ] Microsoft.EntityFrameworkCore.Sqlite: 9.0.2 → 10.0.0 ✅
- [ ] Microsoft.EntityFrameworkCore.Tools: 9.0.2 → 10.0.0 ✅
- [ ] System.Text.Json: 9.0.2 → 10.0.0 ✅
- [ ] Microsoft.Extensions.Diagnostics.HealthChecks: 9.0.2 → 10.0.0 ✅
- [ ] Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore: 9.0.2 → 10.0.0 ✅

**Build Success**:
- [ ] Zero compilation errors across all projects
- [ ] Zero warnings (or all warnings documented and acceptable)
- [ ] All package dependencies resolve correctly
- [ ] Solution builds in both Debug and Release configurations

**Testing Success**:
- [ ] TriviaSpark.Core.Tests: All tests pass (908 LOC)
- [ ] Test execution time acceptable (< 15 minutes)
- [ ] Code coverage maintained or improved
- [ ] No new test failures introduced

**Application Functionality**:
- [ ] TriviaSpark.Web starts without errors
- [ ] Web application home page loads
- [ ] User authentication/authorization works
- [ ] Database operations function correctly
- [ ] Health check endpoints respond healthy
- [ ] TriviaSpark.Console executes successfully
- [ ] JeopardyData.Console executes successfully

**Security & Performance**:
- [ ] No new security vulnerabilities in dependencies
- [ ] No security warnings from package analysis
- [ ] Web application response times within acceptable range (< 20% degradation)
- [ ] Database query performance acceptable
- [ ] Memory usage stable

### 10.3 Quality Criteria

**Code Quality**:
- [ ] No compiler warnings introduced by upgrade
- [ ] Obsolete API usage warnings addressed
- [ ] Code follows existing project conventions
- [ ] No temporary workarounds left in code
- [ ] Breaking changes properly documented in code comments

**Test Coverage**:
- [ ] Unit test coverage maintained (same % as pre-upgrade)
- [ ] All critical paths covered by tests
- [ ] No tests disabled or skipped without justification
- [ ] Test code updated for any Core library API changes

**Documentation**:
- [ ] README.md updated with .NET 10 requirement (if applicable)
- [ ] Developer setup instructions updated
- [ ] Known issues documented (if any .NET 10 preview issues)
- [ ] Breaking changes log created/updated
- [ ] Upgrade process documented for future reference

### 10.4 Process Criteria

**Big Bang Strategy Principles Followed**:
- [ ] All projects upgraded simultaneously (no incremental phases)
- [ ] Single atomic operation for framework + package updates
- [ ] No intermediate multi-targeting states
- [ ] Entire solution validated together
- [ ] Migration completed efficiently (target: < 4 hours)

**Source Control Strategy**:
- [ ] All work performed on `upgrade-to-NET10` branch
- [ ] Single commit for atomic upgrade (or minimal checkpoint commits)
- [ ] Commit message(s) clearly describe changes
- [ ] Pull Request created with comprehensive description
- [ ] Code review completed successfully
- [ ] Changes merged to `main` branch
- [ ] No merge conflicts

**Migration Scenario Compliance**:
- [ ] .NET 10 SDK verified installed before starting
- [ ] Assessment data used to guide all decisions
- [ ] All 10 recommended package updates applied
- [ ] Dependency order respected during validation
- [ ] Rollback strategy documented and tested
- [ ] No steps skipped from the plan

### 10.5 Sign-Off Criteria

**The migration is considered complete and successful when**:

✅ **All Technical Criteria Met**: Every project builds, all tests pass, all applications run  
✅ **All Quality Criteria Met**: Code quality maintained, documentation updated  
✅ **All Process Criteria Met**: Source control strategy followed, Big Bang principles applied  
✅ **Stakeholder Approval**: Team lead or project owner signs off  
✅ **Production Readiness**: Solution ready for deployment (if applicable)  

**Final Validation Checklist**:
- [ ] Developer can clone repo, checkout main, and build successfully
- [ ] CI/CD pipeline passes all checks
- [ ] Staging environment (if exists) runs the upgraded application
- [ ] No blocking issues identified
- [ ] Team trained on any new .NET 10 features or changes
- [ ] Upgrade retrospective conducted (lessons learned documented)

---

## 11. Timeline and Effort Estimates

### 11.1 Atomic Upgrade Effort (Big Bang Strategy)

**Phase 0: Preparation** (15-30 minutes)
- Verify .NET 10 SDK installed and active
- Confirm all tools compatible with .NET 10
- Review this migration plan
- Ensure `upgrade-to-NET10` branch is active
- **Complexity**: Low

**Phase 1: Atomic Upgrade Operation** (1.5-2.5 hours)
- Update all 6 project files (TargetFramework: net9.0 → net10.0)
- Update all 10 NuGet package references (version 9.0.2 → 10.0.0)
- Restore dependencies (`dotnet restore`)
- Build entire solution (`dotnet build`)
- Fix all compilation errors discovered
- Rebuild to verify all errors resolved
- **Complexity**: Medium
- **Risk Level**: Medium (all changes applied simultaneously)

**Phase 2: Testing and Validation** (1-2 hours)
- Execute TriviaSpark.Core.Tests (908 LOC of tests)
- Fix any test failures related to framework changes
- Start and validate TriviaSpark.Web application
- Execute TriviaSpark.Console and JeopardyData.Console
- Perform smoke testing of core functionality
- Validate health check endpoints
- **Complexity**: Medium
- **Risk Level**: Low

**Phase 3: Code Review and Merge** (30-60 minutes)
- Create Pull Request with changes
- Code review process
- Address reviewer feedback (if any)
- Merge to main branch
- Post-merge validation
- **Complexity**: Low
- **Risk Level**: Low

### 11.2 Per-Project Breakdown

| Project | Complexity | Estimated Time | Dependencies | Risk Level |
|---------|------------|---------------|--------------|------------|
| HttpClientDecorator | Low | 15 min | None (leaf node) | Low |
| TriviaSpark.Core | Medium | 45 min | HttpClientDecorator | Medium |
| JeopardyData.Console | Low | 10 min | None (independent) | Low |
| TriviaSpark.Console | Low | 5 min | TriviaSpark.Core | Low |
| TriviaSpark.Core.Tests | Low | 30 min | TriviaSpark.Core | Low |
| TriviaSpark.Web | High | 60 min | Core + HttpClientDecorator | Medium |

**Note**: Under Big Bang Strategy, these times represent validation effort per project, not sequential execution. All projects are updated simultaneously in Phase 1.

### 11.3 Total Timeline

**Optimistic Estimate**: 3 hours  
**Realistic Estimate**: 4-5 hours  
**Pessimistic Estimate**: 6-8 hours (if significant breaking changes discovered)

**Timeline Breakdown**:
- **Preparation**: 30 minutes
- **Atomic Upgrade**: 2.5 hours (includes error fixing)
- **Testing**: 1.5 hours
- **Review & Merge**: 1 hour
- **Buffer**: 30 minutes (contingency)

**Total Elapsed Time**: 4-5 hours for complete migration from start to merge

### 11.4 Resource Requirements

**Developer Skills Needed**:
- **Primary Developer**: 
  - Strong C# and .NET experience
  - Familiarity with ASP.NET Core and Entity Framework
  - Experience with package management and dependency resolution
  - Understanding of breaking changes in .NET versions
  - 4-5 hours dedicated time (minimize interruptions)

**Reviewer**: 
  - Senior developer familiar with codebase
  - 30-60 minutes for code review
  - Understanding of .NET 10 changes

**Optional**:
- QA resource for additional manual testing: 1-2 hours
- DevOps resource for CI/CD validation: 30 minutes

**Parallel Work Capacity**:
- Primary work: 1 developer (atomic upgrade operation)
- Testing can potentially involve multiple people after Phase 1 completes
- Code review: 1-2 reviewers

**Timing Recommendations**:
- Schedule during low-traffic period (if production system)
- Avoid Friday afternoon or before holidays
- Ensure rollback window available
- Plan for .NET 10 preview issues (check GitHub issues first)

---

## 12. Implementation Execution Guidance

### Overview

This section provides the structured execution plan for implementing the .NET 10 upgrade using Big Bang Strategy. The plan is organized into logical phases that describe WHAT needs to be done and in what ORDER, while leaving the specific HOW to the executor agent.

### Phase 0: Preparation

**Objective**: Ensure environment is ready for .NET 10 upgrade

**Operations**:
1. Verify .NET 10 SDK is installed on the development machine
   - Expected version: 10.0.x (preview)
   - Command: `dotnet --list-sdks`
   - Success criteria: .NET 10 SDK appears in list

2. Confirm upgrade branch is active
   - Branch: `upgrade-to-NET10`
   - Verify no pending changes (or acceptable pending changes)
   - Success criteria: On correct branch, clean working directory

3. Environment validation
   - Visual Studio or VS Code compatible with .NET 10 preview
   - Build tools support .NET 10
   - Package restore can access NuGet.org

**Deliverables**: 
- Environment ready for upgrade
- No blockers to proceeding

**Estimated Duration**: 15-30 minutes

---

### Phase 1: Atomic Upgrade

**Objective**: Perform simultaneous framework and package updates across all projects, build the solution, and resolve all compilation errors in a single coordinated operation.

**Operations** (performed as single atomic batch):

#### Step 1: Update All Project Files
Update `<TargetFramework>` from `net9.0` to `net10.0` in all project files:

1. `HttpClientDecorator\HttpClientDecorator.csproj`
2. `TriviaSpark.Core\TriviaSpark.Core.csproj`
3. `JeopardyData.Console\JeopardyData.Console.csproj`
4. `TriviaSpark.Console\TriviaSpark.Console.csproj`
5. `TriviaSpark.Core.Tests\TriviaSpark.Core.Tests.csproj`
6. `TriviaSpark.Web\TriviaSpark.Web.csproj`

#### Step 2: Update All Package References

Update package versions from 9.0.2 to 10.0.0 (see complete package matrix in §5 Package Update Reference):

**HttpClientDecorator.csproj**:
- Microsoft.Extensions.Http: 9.0.2 → 10.0.0

**TriviaSpark.Core.csproj** (7 packages):
- System.Drawing.Common: 9.0.2 → 10.0.0
- System.Runtime.Caching: 9.0.2 → 10.0.0
- Microsoft.AspNetCore.Identity.EntityFrameworkCore: 9.0.2 → 10.0.0
- Microsoft.AspNetCore.Identity.UI: 9.0.2 → 10.0.0
- Microsoft.EntityFrameworkCore.Sqlite: 9.0.2 → 10.0.0
- Microsoft.EntityFrameworkCore.Tools: 9.0.2 → 10.0.0
- System.Text.Json: 9.0.2 → 10.0.0

**TriviaSpark.Web.csproj** (2 packages):
- Microsoft.Extensions.Diagnostics.HealthChecks: 9.0.2 → 10.0.0
- Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore: 9.0.2 → 10.0.0

**Note**: Projects with no explicit package updates (JeopardyData.Console, TriviaSpark.Console, TriviaSpark.Core.Tests) only need TargetFramework change.

#### Step 3: Restore Dependencies
- Execute: `dotnet restore` at solution level
- Success criteria: All packages restored successfully, no errors

#### Step 4: Build Solution and Fix Compilation Errors
- Execute: `dotnet build` at solution level
- Analyze all compilation errors discovered
- Apply fixes for breaking changes (reference §6 Breaking Changes Catalog):
  - API changes in EF Core 10.0
  - ASP.NET Core 10.0 middleware/configuration changes
  - Identity system changes
  - System.Text.Json serialization changes
  - Obsolete API replacements
- Rebuild after each category of fixes
- Iterate until zero compilation errors

#### Step 5: Final Build Verification
- Execute: `dotnet build` in Release configuration
- Validate: Zero errors, zero warnings (or acceptable warnings documented)
- Success criteria: Solution builds completely

**Deliverables**: 
- All projects targeting net10.0
- All packages updated to 10.0.0
- Solution builds with 0 errors

**Estimated Duration**: 1.5-2.5 hours

---

### Phase 2: Test Validation

**Objective**: Execute all automated tests and validate application functionality.

**Operations**:

#### Step 1: Execute Unit Tests
- Project: `TriviaSpark.Core.Tests`
- Command: `dotnet test TriviaSpark.Core.Tests\TriviaSpark.Core.Tests.csproj`
- Expected: All 908 LOC of tests execute
- Success criteria: 0 test failures

#### Step 2: Address Test Failures (if any)
- Analyze failure reasons (likely related to Core library changes)
- Fix issues in TriviaSpark.Core or test code as appropriate
- Re-run tests until 100% pass
- Document any behavioral changes discovered

#### Step 3: Application Smoke Testing

**TriviaSpark.Web**:
- Start application: `dotnet run --project TriviaSpark.Web`
- Verify: Application starts without errors
- Validate: Health check endpoint returns healthy
- Test: Load home page in browser
- Test: Authenticate with test user
- Test: Basic trivia game functionality works

**TriviaSpark.Console**:
- Execute: `dotnet run --project TriviaSpark.Console`
- Verify: Application runs and exits successfully
- Check: Console output indicates success

**JeopardyData.Console**:
- Execute: `dotnet run --project JeopardyData.Console`
- Verify: Data processing completes successfully

#### Step 4: Integration Validation
- Database connectivity (SQLite)
- Entity Framework migrations compatibility
- Identity system operations
- HTTP client operations (via HttpClientDecorator)
- Health checks functionality

**Deliverables**: 
- All tests pass
- All applications validated

**Estimated Duration**: 1-2 hours

---

### Phase 3: Code Review and Merge

**Objective**: Review changes, obtain approval, and integrate to main branch.

**Operations**:

#### Step 1: Source Control Commit
- Review all changes made during Phase 1 and Phase 2
- Commit with message: "Upgrade solution to .NET 10.0"
- Include comprehensive commit body (see §9.3 for format)
- Push `upgrade-to-NET10` branch to origin

#### Step 2: Create Pull Request
- Create PR: `upgrade-to-NET10` → `main`
- Title: "Upgrade solution from .NET 9 to .NET 10"
- Description: 
  - List all projects upgraded
  - List all packages updated
  - Note test results
  - Highlight any breaking changes addressed
  - Include validation checklist results

#### Step 3: Code Review
- Assign reviewers
- Address feedback
- Update PR if changes needed
- Obtain approval

#### Step 4: Merge to Main
- Ensure all PR checks pass
- Merge to main branch
- Validate post-merge CI/CD pipeline (if exists)

#### Step 5: Cleanup
- Delete `upgrade-to-NET10` branch locally
- Notify team of upgrade completion
- Update documentation if needed

**Deliverables**: 
- Changes merged to main
- Upgrade complete and validated

**Estimated Duration**: 30-60 minutes

---

### Success Criteria Summary

The migration is complete when:
- ✅ All 6 projects target net10.0
- ✅ All 10 packages updated to 10.0.0
- ✅ Solution builds with 0 errors
- ✅ All tests pass (TriviaSpark.Core.Tests)
- ✅ All applications run successfully
- ✅ Changes merged to main branch
- ✅ No security warnings or vulnerabilities

---

## Appendix A: Quick Reference

### Project Dependency Order
1. **Leaf Projects** (no dependencies): HttpClientDecorator, JeopardyData.Console
2. **Mid-Tier**: TriviaSpark.Core (depends on HttpClientDecorator)
3. **Top-Tier**: TriviaSpark.Web, TriviaSpark.Console, TriviaSpark.Core.Tests (depend on Core)

### Package Update Cheat Sheet
```
All packages: 9.0.2 → 10.0.0

HttpClientDecorator:
  - Microsoft.Extensions.Http

TriviaSpark.Core (7 packages):
  - System.Drawing.Common
  - System.Runtime.Caching
  - Microsoft.AspNetCore.Identity.EntityFrameworkCore
  - Microsoft.AspNetCore.Identity.UI
  - Microsoft.EntityFrameworkCore.Sqlite
  - Microsoft.EntityFrameworkCore.Tools
  - System.Text.Json

TriviaSpark.Web (2 packages):
  - Microsoft.Extensions.Diagnostics.HealthChecks
  - Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore
```

### Key Commands
```bash
# Verify SDK
dotnet --list-sdks

# Restore packages
dotnet restore

# Build solution
dotnet build

# Run tests
dotnet test TriviaSpark.Core.Tests\TriviaSpark.Core.Tests.csproj

# Run applications
dotnet run --project TriviaSpark.Web
dotnet run --project TriviaSpark.Console
dotnet run --project JeopardyData.Console
```

### Rollback Command
```bash
git checkout main
git branch -D upgrade-to-NET10
```

---

**End of Migration Plan**