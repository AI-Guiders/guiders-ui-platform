# GUIDERS-UI-0003: Agent AX and human a11y — dual accessibility layer

**Status:** accepted (2026-08-25)  
**Tags:** #guiders #ui #a11y #ax #agent #human #open  
**Related:** GUIDERS-UI-0001 · GUIDERS-UI-0002 · GUIDERS-ADR-0005 · FORGE-ADR-0049

---

## Context

AIGuiders UI serves **two audiences** with equal standing:

- **Humans** — browser, keyboard, assistive tech, visual perception diversity
- **Agents** — MCP, automation, command surfaces, programmatic navigation

Traditional a11y (WCAG, ARIA, contrast, screen readers) targets humans. Agents today scrape HTML, guess from CSS classes, and break on refactors. That is not agent parity — it is second-class access.

The UI platform (GUIDERS-UI-0001/0002) separates **Core contracts** from **adapters** (.NET Razor first; JS, Python, Qt, Rust later). That split enables a second insight: accessibility is not one layer. It is **two exposure surfaces** over the same semantics.

**Agent AX** (accessibility exposure for agents) may be machine-oriented, structural, even «inhuman». That is acceptable — agents do not need warm copy in a JSON manifest; they need reliable semantics.

---

## Decision

### 1. Dual accessibility layer (same principles, different exposure)

| Layer | Audience | Package home | Delivers |
|-------|----------|--------------|----------|
| **Human a11y** | people | `AIGuiders.UI.Tokens` + adapters | contrast-safe tokens, focus, ARIA, reduced motion, screen-reader text |
| **Agent AX** | agents, tests, MCP | `AIGuiders.UI.Core` (+ future `UI.AX`) | stable component IDs, semantic roles, state, affordances, action hints |

Both are **first-class**. Neither is a binding around the other.

### 2. SSOT = semantics in Core

View models in `AIGuiders.UI.Core` are the semantic source:

- component kind (`PageChrome.Title`, `EmptyState.HomeCatalog`, …)
- stable `TestId` / exposure id (e.g. `forge-home-empty`)
- optional machine fields: `role`, `empty`, `actions[]`, `commandHints[]`

Adapters render human markup **and** expose AX — not instead of each other.

Example (illustrative; not shipped schema yet):

```json
{
  "component": "EmptyState.HomeCatalog",
  "testId": "forge-home-empty",
  "semantics": { "role": "status", "empty": true },
  "actions": [{ "surface": "command", "hint": "Ctrl+K /repo create" }]
}
```

Humans see the card. Agents consume the contract. Screen readers hear the title. No single format must serve all three identically.

### 3. Native per ecosystem — not language bindings

Human a11y and Agent AX propagate as **native implementations** per stack:

| Stack | Human a11y | Agent AX |
|-------|------------|----------|
| .NET / Razor (`UI.Web.HTMX`) | `aria-*`, tokens CSS | `data-testid`, optional `data-ui-ax` JSON |
| npm / React | `@aiguiders/ui-tokens` + a11y props | `@aiguiders/ui-ax` types + test selectors |
| Python / PyPI | Jinja + tokens | pydantic models from Core schema |
| Qt / C++ | palettes from tokens | semantic object names / properties |
| Rust / Cargo | theme from tokens | typed component manifest |

**Forbidden pattern:** «call .NET to get HTML string» as the only agent path.  
**Required pattern:** shared Core schema; each ecosystem ships its own adapter.

### 4. Conformance, not charity

Open Core + Tokens means basic comfort (human and agent) is not paywalled. Conformance is verified by:

- human: contrast/token lint, axe or equivalent in CI (adapters)
- agent: contract tests against Core snapshots / journey tests (stable `testId`, semantics)
- cross-language: same JSON Schema generated from Core; adapter tests in each repo

### 5. Relationship to Forge Human View

Forge remains a **reference consumer** (GUIDERS-ADR-0005). FORGE-ADR-0049 human-primary + command palette are AX affordances. Forge-specific domain (IOP, diff) stays in Forge; **chrome, empty states, tokens, AX ids** migrate to `AIGuiders.UI.*`.

---

## Non-goals (v1)

- Full WCAG certification program
- Single universal DOM format for humans and agents
- Agent AX that mimics natural language where structure suffices
- Blocking v1 NuGet ship on AX schema finalization

---

## Consequences

- `AIGuiders.UI.Core` models gain optional AX fields (`TestId`, semantics) — already started in v0.1.0 (`HomeCatalogEmptyModel.TestId`).
- Future package `AIGuiders.UI.AX` (or Core sub-namespace) may publish JSON Schema + codegen for TS/Python/Rust.
- MCP tools may describe surfaces from AX contracts (`human_view.describe_surface`) without scraping opaque HTML.
- Human a11y and Agent AX can evolve independently as long as Core semantics stay stable (semver).

---

## vNext (ordered)

1. Document Core component catalog with required `testId` + semantic role per component
2. JSON Schema export from `AIGuiders.UI.Core` (CI artifact)
3. Token lint: contrast pairs in `UI.Tokens`
4. First non-.NET adapter proves native AX (e.g. npm types-only package)
5. Forge journey tests assert AX ids alongside human-visible copy
