# GUIDERS-UI-0004: Accessibility Surface Protocol (ASP) — platform-neutral a11y charter

**Status:** accepted (draft charter — 2026-08-25)  
**Tags:** #guiders #ui #a11y #ax #standard #federation #open  
**Related:** GUIDERS-UI-0001 · GUIDERS-UI-0003 · GUIDERS-ADR-0006 · ANUI-ADR-0004 (evolution)

---

## Context

True accessibility means **using software regardless of constraints** — sensory, motor, cognitive, technological. That promise must hold across SPA, MPA, server-rendered web, desktop, mobile, and agent/automation surfaces.

Today:

- **WCAG / ARIA** are web-DOM centric; each JS framework bridges differently.
- **Desktop/mobile** use platform APIs (UIA, UIAccessibility, Qt accessibility, …) — separate islands.
- **Agent AX** (GUIDERS-UI-0003) adds a second exposure layer but shares no industry-wide neutral schema yet.
- **ANUI** (`ai-native-ui`) pioneered evidence-first semantic trees with stable ids — but centered on a .NET runtime.

Operator goal: not another UI kit, but a **standard platforms can adopt** — React, Vue, Angular, Razor, Qt, native mobile — with conformance, not goodwill README claims.

Federation charter (GUIDERS-ADR-0006): sovereign planets, shared protocols, no annexation.

---

## Decision

### 1. Accessibility Surface Protocol (ASP)

Introduce **ASP** as the federation's platform-neutral accessibility model:

| ASP concept | Meaning |
|-------------|---------|
| **Surface** | Any rendered UI: web, desktop, mobile, terminal+TTS |
| **Semantic node** | Role, name, state, value, relations, actions — **not** DOM tag |
| **Exposure profile** | Mapping ASP → target platform (ARIA, UIA, Qt, UIAccessibility, agent JSON) |
| **Conformance artifact** | Machine-checkable snapshot/manifest (Evidence lineage from ANUI) |

ASP is **limitation-independent semantics**. Rendering is an adapter concern.

### 2. Relationship to GUIDERS-UI-0003

GUIDERS-UI-0003 dual layer collapses into **two projections of one ASP graph**:

| Projection | Audience | Typical profile |
|------------|----------|-----------------|
| **Human a11y** | people + AT | `profile:web-aria`, `profile:uia`, … |
| **Agent AX** | agents, tests, MCP | `profile:agent-json`, `profile:test-selectors` |

Same node graph; different serializers. Not two hand-maintained layers.

### 3. Package trajectory (guiders-ui-platform)

| Package | Role |
|---------|------|
| `AIGuiders.UI.Core` | Product component semantics (PageChrome, EmptyState, …) |
| `AIGuiders.UI.Tokens` | Visual tokens; contrast pairs for human profile lint |
| `AIGuiders.Accessibility.Core` | **ASP** schema: nodes, roles, states, actions, relations |
| `AIGuiders.Accessibility.Profiles.*` | Per-ecosystem mappers (future npm scope `@aiguiders/a11y-profile-*`) |
| `AIGuiders.UI.Web.HTMX` | First adapter: Razor + ARIA profile + AX ids |

`AIGuiders.UI.AX` (GUIDERS-UI-0003 vNext) **merges into** `AIGuiders.Accessibility.Core` — not parallel SSOT.

### 4. Platform adoption model

Frameworks do **not** embed our runtime. They adopt ASP by:

1. Implementing or consuming an **ASP profile** for their stack.
2. Publishing **conformance** in CI (snapshot diff, invariant packs).
3. Mapping component libraries to ASP roles — optionally via community registry.

**Native per ecosystem** (GUIDERS-UI-0003): React package reads same JSON Schema as .NET; no .NET binding as gateway.

### 5. Conformance (serious a11y)

| Check | Human | Agent |
|-------|-------|-------|
| Semantic completeness | required roles/names present | stable ids, action graph |
| Token/contrast | profile lint | N/A |
| State honesty | aria-live / platform equiv | manifest matches DOM/runtime |
| Journey stability | axe / platform AT smoke | contract tests on ids |

Open Core: baseline packs are not paywalled.

### 6. ANUI evolution (see ANUI-ADR-0004)

`ai-native-ui` pivots from **runtime-first** to **protocol + evidence-first**:

- Scene/runtime becomes **one adapter planet**, not the federation capital.
- `Anui.Evidence`, Invariants, Ingest, Agent channel **align with ASP**.
- Foreign ingest (ADR-0002) feeds ASP trees from legacy islands.

---

## Non-goals (v1 charter)

- Replacing WCAG — ASP **feeds** WCAG conformance, does not supersede W3C overnight.
- Certifying all 8B humans' apps — federation scope first.
- Single renderer for all platforms.
- Blocking UI platform v0.1 ship on ASP schema finalization.

---

## Consequences

- GUIDERS-UI-0003 remains valid; implementation path refines toward single ASP graph.
- New work prioritizes JSON Schema + .NET types in `AIGuiders.Accessibility.Core` before React profile.
- Integration reviews: **ASP node present? profile named? conformance artifact?**
- ANUI roadmap explicitly subordinate to ASP unless runtime proves unique value (e.g. headless audit host).

---

## vNext (ordered)

1. ASP JSON Schema v0.1 (minimal node: id, role, name, state, actions[])
2. Map `PageChrome` / `EmptyState` Core models → ASP nodes (Forge journey asserts)
3. `profile:web-aria` reference mapper in `UI.Web.HTMX`
4. ANUI Evidence export compatibility shim
5. npm `profile:react` types-only package (proves federation hyperlane)
6. Second consumer conformance (CDP/cockpit or Forge extended journeys)

---

## Impact note (honest)

ASP does not need universal overnight adoption to matter. **Working hyperlane + open schema + reference conformance** changes what integrators and regulators can demand. Scale follows copies of the protocol, not one hero repo.
