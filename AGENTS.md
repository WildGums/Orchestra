# Orchestra

Orchestra is a mature, composable WPF shell and framework built on top of [Catel](http://www.catelproject.com). It provides a robust yet flexible Line of Business (LoB) shell, designed with best practices in mind, to jump-start the development of desktop applications.

Orchestra consists of the following projects:

- **Orchestra.Core** — Core shell library with services, commands, and shared WPF functionality.
- **Orchestra.Shell.Ribbon.Fluent** — Fluent Ribbon shell implementation.
- **Orchestra.Shell.TaskRunner** — Task Runner shell implementation.

---

## Critical Rules (Read First)

These rules are **non-negotiable**. Violating them causes broken builds, crashes, or downstream breakage.

### 1. Never Edit Generated Files

Files matching `*.generated.cs`, `*.generated.xaml` are auto-generated.

- **NEVER** manually edit these files

### 2. ABI / API Stability

This project maintains stable ABI / API. Breaking changes break downstream apps.

| Allowed | Never |
|---------|-------|
| Add new overloads | Modify existing signatures |
| Add new methods | Remove public APIs |
| Add new classes | Change return types |

### 3. Tests Are Mandatory

**Building alone is NOT sufficient.** Run tests before claiming completion (see [Commands](#commands)).

### 4. Branch Protection (COMPLIANCE REQUIRED)

**Direct commits to protected branches are a policy violation.**

| Repository | Protected Branches |
|------------|-------------------|
| Orchestra | `master` |
| Orchestra | `develop` |

**Required workflow:**

1. **Create a feature branch FIRST** — Use naming convention: `feature/issue-NNNN-description`
2. **Make all commits on the feature branch** — Never commit directly to protected branches
3. **Submit a Pull Request** — Changes must be reviewed by a human before merging

```bash
# CORRECT — Always create a feature branch first
git checkout -b feature/issue-1234-fix-description

# NEVER DO THIS — Policy violation
git checkout develop && git commit  # FORBIDDEN

# NEVER DO THIS — Policy violation
git checkout master && git commit  # FORBIDDEN
```

The repository has protected branches that must be respected.

---

## Commands

Single source of truth for all commands:

| Task | Command |
|------|---------|
| **Build** | `dotnet cake --target=build` |
| **Test** | `dotnet cake --target=test` |
| **Build and test** | `dotnet cake --target=buildandtest` |

---

## Architecture & Directories

### Project Overview

```
Orchestra.Core                  => Core shell services and WPF utilities (cross-shell)
Orchestra.Shell.Ribbon.Fluent   => Fluent Ribbon shell
Orchestra.Shell.TaskRunner      => Task Runner shell
Orchestra.Shell.Shared          => Shared shell code
Orchestra.Tests                 => Automated test suite
```

### Directory Guide

| Directory / Pattern | Editable? | Notes |
|---------------------|-----------|-------|
| `*.generated.cs` | No | Leave as-is — auto-generated |
| `*.generated.xaml` | No | Leave as-is — auto-generated |
| `deployment/` | No | Deployment / build scripts |
| `doc/` | Yes | Documentation and images |
| `src/Orchestra.Core/` | Yes | Core shell source |
| `src/Orchestra.Shell.Ribbon.Fluent/` | Yes | Fluent Ribbon shell source |
| `src/Orchestra.Shell.TaskRunner/` | Yes | Task Runner shell source |
| `src/Orchestra.Tests/` | Yes | Automated tests |

---

## Writing Code

### Anti-Patterns (Never Do This)

| Anti-Pattern | Why |
|-------------|-----|
| Modifying method signatures | ABI breaking |
| Manual edits to `*.generated.cs`, `*.generated.xaml` | Overwritten on regenerate |
| Using default parameters in public APIs | ABI breaking |
| Reformatting unrelated code | Makes diffs harder to review |
| **Skipping failing tests** | **Unacceptable — tests must pass** |

---

## Testing & Debugging

### Running Tests

```bash
dotnet cake --target=test
```

### Tests MUST Pass

> **NON-NEGOTIABLE:** Tests must PASS before claiming completion.
>
> - Do NOT skip failing tests
> - Do NOT claim completion if tests fail
> - Do NOT use `SkipException` to work around failures

### Writing Tests

1. Use NUnit to write tests
2. Create a Facts class for a feature
3. Combine Pascal / Snake case for test methods (e.g. `Feature_Does_Work`)

```csharp
[Test]
public void Feature_Does_Work()
{
    var result = 47 - 5;

    Assert.That(result, Is.EqualTo(42));
}
```

**Philosophy:** Tests FAIL when wrong, never skip (except missing hardware).

### Debugging Methodology

1. **Establish baseline** — What's the known-good state?
2. **One change at a time** — Verify each change before proceeding
3. **Track changes in a table** — Log what you changed and the result
4. **Platform differences are signals** — If X works and Y fails, the difference IS the answer
5. **Revert if worse** — Don't pile fixes on top of failures

---

## Further Reading

| Topic | Document |
|-------|----------|
| Contributing guidelines | [CONTRIBUTING.md](CONTRIBUTING.md) |
| Documentation portal | https://opensource.wildgums.com |
| Catel (underlying framework) | https://github.com/Catel/Catel |
