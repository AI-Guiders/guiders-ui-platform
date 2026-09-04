# GUIDERS-UI-0002: v1 extraction slice

**Status:** accepted (2026-08-25)  
**Related:** FORGE-ADR-0059 · GUIDERS-ADR-0005

## v1 scope (shipped in 0.1.0)

| Extracted | Package |
|-----------|---------|
| PageChrome models + Razor partials | Core + Web.HTMX |
| EmptyStates (Message, HomeCatalog, CreateRepoHint) | Core + Web.HTMX |
| HumanUiBreadcrumb | Core |
| Design token CSS subset | Tokens |
| Razor render bridge | Web.HTMX |

## Stays in Forge (for now)

- Catalog tree builder, grouping toolbar implementation
- IOP, diff, code browse composites
- IssueList / MergeRequestList empty states (domain copy)
- Full `forge-view-layout.css`

## DoD

- [x] Monorepo builds and tests
- [x] Forge references sibling packages
- [x] Journey tests pass (home chrome not escaped)
- [x] `release.yml` + [docs/nuget-publishing.md](../nuget-publishing.md) (TP parameters)
- [x] NuGet Trusted Publishing policy saved on nuget.org + tag `v0.1.0` pushed (`release` workflow green, 2026-08-25)
