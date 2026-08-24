# Guiders UI Platform

Open human+agent UI kit for AI Guiders products — **Core contracts**, **design tokens**, and **web adapters** (MPA / HTMX first).

Sibling to [guiders-platform](../guiders-platform) (headless mechanics) and [guiders-core](../guiders-core) (backend libraries). See [GUIDERS-ADR-0005](../guiders-platform/docs/adr/GUIDERS-ADR-0005-ui-platform-monorepo.md).

## Packages

| Package | Role |
|---------|------|
| `AIGuiders.UI.Core` | View contracts (page chrome, empty states, breadcrumbs) |
| `AIGuiders.UI.Tokens` | CSS variables + chrome/empty-state hooks |
| `AIGuiders.UI.Web.HTMX` | Razor partials + SSR render bridge (first adapter) |

Future adapters (`Web.Blazor`, `React`, …) share **Core** + **Tokens**; markup stays in adapters.

## Layout

```
src/          # ship-ready NuGet packages
tests/        # unit tests
docs/adr/     # UI platform ADRs
```

## Build

```bash
dotnet build
dotnet test
```

## Local dev (agent-forge)

Clone `guiders-ui-platform` as sibling of `agent-forge` under `open/`. `AgentForge.Plugin.View` resolves `$(GuidersUiPlatformRoot)` when `AIGuiders.UI.slnx` exists (same pattern as `GuidersPlatformRoot`).

## ADR

- [GUIDERS-UI-0001 — monorepo bootstrap](docs/adr/GUIDERS-UI-0001-monorepo-bootstrap.md)
- [GUIDERS-UI-0002 — v1 extraction slice](docs/adr/GUIDERS-UI-0002-v1-extraction-slice.md)
- [GUIDERS-UI-0003 — Agent AX and human a11y](docs/adr/GUIDERS-UI-0003-agent-ax-and-human-a11y.md)
- [GUIDERS-UI-0004 — Accessibility Surface Protocol (ASP)](docs/adr/GUIDERS-UI-0004-accessibility-surface-protocol.md)
