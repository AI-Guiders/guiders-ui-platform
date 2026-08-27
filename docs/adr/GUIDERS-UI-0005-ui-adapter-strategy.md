# GUIDERS-UI-0005: UI adapter strategy — MPA, Blazor, SPA, native

**Status:** accepted (2026-08-27)  
**Tags:** #guiders #ui #adapter #mpa #blazor #spa #native #federation #agent #human  
**Related:** GUIDERS-UI-0001 · GUIDERS-UI-0003 · GUIDERS-UI-0004 · GUIDERS-ADR-0005 · GUIDERS-ADR-0006 · FORGE-ADR-0049 · FORGE-ADR-0059

---

## Context

AI Guiders ships **agent + human** surfaces across carriers:

| Planet | Typical surface | Ingress |
|--------|---------------|---------|
| `agent-forge` | Human View MPA (`/view/*`) | browser + MCP |
| `agent-nuget-pm` | `Anpm.View` (standalone or Forge mount) | browser + MCP |
| `dash-spec` | Blazor Server dashboards | browser |
| `cascade-ide` | native IDE (Avalonia) | human + CDP agents |
| JS ecosystems (partners, future) | React / Vue / Angular apps | browser + their agent layer |

GUIDERS-UI-0001/0003 established:

- **Core** = semantic SSOT (view models, Agent AX fields, future ASP nodes).
- **Adapters** = ecosystem-native render paths (Razor first).
- **Forbidden:** .NET HTML string as the only agent path; language bindings as gateway.

Operators still need a **single decision frame**: when MPA, when Blazor, when SPA, when native — without implying one winner or re-building a monolith UI capital.

Federation charter (GUIDERS-ADR-0006): sovereign planets, shared hyperlanes, no annexation.

---

## Decision

### 1. One semantic center, many adapters

```text
AIGuiders.UI.Core          — component semantics + AX fields (+ future ASP graph)
AIGuiders.UI.Tokens        — design tokens (CSS variables; npm mirror later)
        │
        ├── UI.Web.HTMX     — Razor MPA / SSR (shipped v0.1)
        ├── UI.Web.Blazor   — Blazor Server / WASM adapter (planned)
        ├── @aiguiders/ui-* — React / Vue adapters (planned; types + components)
        └── (future)        — Qt / Avalonia profile, terminal+TTS, …
```

**Products consume Core (+ Tokens).** They pick **one primary adapter** per human surface. Domain composites (Forge diff, dash-spec charts) stay on the planet.

### 2. Adapter catalog

| Adapter | Runtime model | Human interaction | Agent ingress | First consumers |
|---------|---------------|-------------------|---------------|-----------------|
| **Web.HTMX** (MPA) | HTTP + SSR; full page or partial refresh | forms, links, keyboard chrome | MCP + journey tests + AX ids / future `describe_surface` | Forge Human View, ANPM.View |
| **Web.Blazor** | server circuit or WASM; component tree on wire | rich in-page state, filters, drag | same AX contract; Blazor component refs map to ASP nodes | dash-spec Host |
| **SPA** (React/Vue/…) | client bundle; API/SSR optional | highly interactive, third-party component libs | `@aiguiders/ui-ax` + native selectors; no .NET scrape | partner JS stacks |
| **Native IDE** | OS windowing (Avalonia, Qt, …) | cockpit density, low latency | CDP / platform MCP; UIA/Qt accessibility profile | cascade-ide |

Adapters are **siblings**, not layers. React does not wrap Razor. Blazor does not require HTMX.

### 3. When to choose (product heuristic)

| Choose **MPA (Web.HTMX)** | Choose **Blazor (Web.Blazor)** | Choose **SPA** | Choose **native** |
|---------------------------|--------------------------------|----------------|-------------------|
| CRUD admin, settings, catalog tables | Dense dashboards, cross-filter state, live tiles | Ecosystem is already JS-first; team owns npm graph | IDE, cockpit, offline desktop |
| Agent-primary + human-primary **same routes** | Many widgets on one board without full reload | Need mature JS component market (charts, editors) | Latency / windowing beats browser |
| Journey tests via `WebApplicationFactory` | Hot-reload spec files, server-owned filter state | Partner cannot adopt .NET UI stack | Multi-monitor, OS integration |
| Forge-class repo journey | dash-spec-class operational boards | Greenfield in Vue/React org | CASCADE-class agent habitat |

**Default for AI Guiders .NET tools:** **MPA** unless the surface is **dashboard-interactive** (→ Blazor) or **IDE-native** (→ Avalonia).

**Default for foreign ecosystems:** **SPA adapter + shared Core schema** — not a .NET binding.

### 4. Shared contract (what every adapter must implement)

| Layer | Package | Adapter obligation |
|-------|---------|-------------------|
| Semantics | `UI.Core` | Map product view models to Core types; stable `TestId`; optional semantics / `commandHints` |
| Visual | `UI.Tokens` | Use token variables; do not fork ad-hoc color/spacing SSOT |
| Human a11y | Tokens + adapter markup | ARIA / platform AT profile (GUIDERS-UI-0003, ASP human projection) |
| Agent AX | Core (+ future `Accessibility.Core`) | Expose ids + action graph; MCP may read contract without HTML scrape |
| Conformance | CI per planet | Journey/contract tests on ids; token lint where applicable |

**Versioning:** Core semver is the federation contract. Adapters may ship independently but must declare compatible Core range.

### 5. Ecosystem-native — required patterns

| Pattern | OK | Not OK |
|---------|-----|--------|
| Semantics | JSON Schema / Core types generated once, implemented natively per stack | Single .NET process renders HTML for all consumers |
| Tokens | CSS variables · npm `@aiguiders/ui-tokens` · Qt palette from same source | Copy-paste hex values per repo |
| Agent path | `describe_surface` / AX manifest / stable test ids | Agent scrapes opaque class names |
| Reuse open UI | MUI/Radix/shadcn **inside** React adapter with our Tokens + AX | «Bridge» that shells out to .NET for markup |
| Cross-planet chrome | `PageChrome`, `EmptyState` from Core | Forge `ForgeHtml` string tables in product C# |

### 6. Anti-patterns (reject in review)

| Anti-pattern | Why | Fix |
|--------------|-----|-----|
| **HTML string SSOT** (`ForgeHtml.Tr`, C# `Render()` builders) | No agent contract; refactor breaks scrape; blocks Razor/a11y | Kit partial or adapter component |
| **Binding gateway** («only agent path = call .NET get HTML») | Annexes foreign ecosystems to our runtime | Core schema + native adapter |
| **Monolith UI capital** (`AIGuiders.Platform.Web.UI` owns all routes) | Violates GUIDERS-ADR-0006; couples planets | Extract to `UI.*`; products wire domain |
| **SPA rewrite for admin CRUD** | Loses MCP/human route parity; ops tax | MPA + optional islands (FORGE-ADR-0049) |
| **Second dialect per product** | `@Html.Raw` + string builders + Razor + React micro-front without Core | One primary adapter + documented escapes |
| **Adapter without AX ids** | Agent second-class; journey tests brittle | Core `TestId` required per component catalog |

Legacy note: Forge migrated off `ForgeHtml` page builders (FORGE-ADR-0059). New work must not reintroduce the pattern under another name.

### 7. Relationship to products (non-annexation)

| Stays in **guiders-ui-platform** | Stays on **planet** |
|-----------------------------------|---------------------|
| PageChrome, generic EmptyStates, breadcrumbs | Issue/MR lists, diff, IOP, wiki dialects |
| Tokens, ASP reference profiles | Domain routes, plugin contributors |
| Adapter packages | `WebApplicationFactory` journey for product routes |
| JSON Schema export (vNext) | Business mutations, storage, MCP tool bodies |

Forge = **reference consumer** (GUIDERS-ADR-0005), not owner of React or Blazor adapters.

### 8. Islands (all web adapters)

Progressive enhancement is allowed and bounded (FORGE-ADR-0049):

- command palette, markdown editor, diff viewer, chart tiles
- **not** a second app shell or client-side router replacing MPA navigation

Islands must still emit ASP/AX nodes for agent parity.

---

## Non-goals

- Picking one adapter for the entire federation.
- Shipping all adapters before Core + HTMX conformance is stable.
- Replacing dash-spec DSL or CASCADE presentation ADRs — only alignment on Core/Tokens/ASP.
- npm React package as v1 gate (proof adapter is vNext).

---

## Consequences

- New .NET human surface: default checklist → MPA unless Blazor/native row in §3 matches.
- New extraction from Forge: horizontal chrome → `UI.*`; domain → Forge plugin.
- Integration reviews add: **adapter named? Core types? AX ids? anti-pattern scan?**
- dash-spec may adopt Tokens + ASP profile without adopting HTMX partials.
- cascade-ide may adopt Tokens + native accessibility profile without web adapters.

---

## vNext (ordered)

1. Core component catalog: required `TestId` + role per shipped component (GUIDERS-UI-0003 item 1)
2. `UI.Web.Blazor` spike: one Core component (e.g. `EmptyState.HomeCatalog`) in dash-spec Host
3. JSON Schema export from Core (CI artifact) — input for npm types-only package
4. ADR cross-link in `dash-spec/design/` and `cascade-ide/docs/adr/` (pointer only)
5. Forge journey tests: assert adapter + AX ids on every new chrome partial
6. `human_view.describe_surface` MCP tool sketch (Forge or platform) consuming AX manifest

---

## Review gate (PR checklist)

- [ ] Primary adapter explicit in product README or planet ADR
- [ ] View models use `AIGuiders.UI.Core` types or extend with planet-specific subclasses
- [ ] No new C# HTML string composition for UI
- [ ] `TestId` present on new interactive chrome
- [ ] Tokens referenced; no duplicate design SSOT
- [ ] If island JS added: scope documented; not a SPA shell
