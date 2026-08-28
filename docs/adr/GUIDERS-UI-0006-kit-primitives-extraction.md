# GUIDERS-UI-0006: Kit primitives extraction (Table, List, Badge, Flash, Panel)

**Status:** accepted (2026-08-28)  
**Related:** GUIDERS-UI-0002 · GUIDERS-UI-0005 · GUIDERS-UI-0007 · FORGE-ADR-0068 · FORGE-ADR-0069

## Context

L1 kit primitives (`Table`, `List`, `Badge`, `Flash`, `Panel`, `Select`, `KitControls`) shipped in Forge as `ForgeHuman*` — double prefix and wrong planet boundary. Platform already owns PageChrome, EmptyStates, breadcrumbs (`HumanUi*` in `AIGuiders.UI.Core` / `Web.HTMX`).

## Decision

### Extract to `AIGuiders.UI.Core`

| Package path | Types |
|------------|-------|
| `Html/HumanUiHtml` | L0 escaped atoms (subset; grows with kit) |
| `Kit/HumanUiTable`, `HumanUiList`, … | L1 primitives |
| `Kit/HumanUiKit`, `HumanUiKitLayers` | manifest (L0–L1 only) |

Naming: **`HumanUi*`** — human+agent SSR kit, not Forge-specific. Matches `HumanUiPageChrome`, `HumanUiBreadcrumb`.

### Forge boundary

- **Dialects / composites** stay on Forge: `ForgeCatalogTable`, `ForgeAccessTokensTable`, `ForgeHumanIssueList`, …
- **ForgeHumanKit** manifest references `HumanUiKit` for primitives + Forge composite names
- **No** `ForgeHumanTable` forwarders — callers use `HumanUiTable` via `@using AIGuiders.UI.Core.Kit`

### Island DOM contract

`HumanUiPanel.WithIsland` keeps `data-forge-island` / `forge-island-{id}` for existing View JS until a cross-product island protocol ships.

## DoD

- [x] Core builds; primitives tests in `AIGuiders.UI.Tests`
- [x] Forge references sibling/local package; journey tests green
- [ ] NuGet publish (next platform release)
