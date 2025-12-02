# Pull Request Creation Commands

## Option 1: Using GitHub CLI (Recommended)

If you have GitHub CLI installed, run:

```bash
gh pr create --base main --head upgrade-to-NET10 --title ".NET 10 Upgrade: 99.3% Warning Reduction (273→2)" --body-file PULL_REQUEST.md
```

## Option 2: Using GitHub Web Interface

1. Push the branch to origin:
```bash
git push origin upgrade-to-NET10
```

2. Go to: https://github.com/controlorigins/TriviaSpark/compare/main...upgrade-to-NET10

3. Click "Create Pull Request"

4. Copy and paste the content from `PULL_REQUEST.md` into the PR description

## Option 3: Using Git Command Line with Opening Browser

```bash
# Push the branch
git push origin upgrade-to-NET10

# Open the PR creation page in your default browser
start https://github.com/controlorigins/TriviaSpark/compare/main...upgrade-to-NET10
```

Then paste the content from `PULL_REQUEST.md` into the description field.

---

## PR Details

**Title**: `.NET 10 Upgrade: 99.3% Warning Reduction (273→2)`

**Base Branch**: `main`

**Compare Branch**: `upgrade-to-NET10`

**Labels** (suggested):
- `enhancement`
- `quality`
- `.net10`
- `breaking-change-free`
- `ready-for-review`

**Reviewers** (suggested):
- Team leads
- Code quality reviewers

**Description**: Use content from `PULL_REQUEST.md` (full version) or `PULL_REQUEST_SHORT.md` (concise version)

---

## Files Available

1. **PULL_REQUEST.md** - Full detailed PR description (recommended for major PRs)
2. **PULL_REQUEST_SHORT.md** - Concise version (for quick review)
3. **WARNINGS_REPORT.md** - Complete technical documentation (already committed)

Choose the version that best fits your team's PR review process!
