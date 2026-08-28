# GUIDERS-UI-0007: FunctionalSpec → IR (XAML family) → PhysicalImplementation

**Status:** accepted (2026-08-28)  
**Tags:** #guiders #ui #ir #xaml #functional #physical #cfihos #federation #compiler  
**Related:** GUIDERS-UI-0005 · GUIDERS-UI-0006 · GUIDERS-ADR-0006 · [ui-ir-platform research](https://gitlab.wissance.com/wissance/ai/agent-notes/-/blob/main/knowledge/work/projects/door-to-singularity/guiders-ui-platform/research/note-ui-ir-platform-research-v1.md)

---

## Context

GUIDERS-UI-0005 names **adapter siblings** (HTMX, Blazor, SPA, native) over one **semantic center** (`AIGuiders.UI.Core`). GUIDERS-UI-0006 extracts L1 kit primitives into Core. Both stop short of a **federation-wide lowering model**: how the same product meaning ships on Glass (WPF XAML), CIDE (Avalonia AXAML), Forge (Razor MPA), ANUI (Skia), and future JS stacks **without** copy-pasting semantics per planet.

Industry precedent already exists:

| Precedent | What it proves |
|-----------|----------------|
| **XAML / AXAML** | Mature **IR**: element tree, templates, resources, `{Binding}`, commands, styles |
| **.NET MAUI Handlers** | Explicit **VirtualView → PlatformView** split inside one runtime |
| **WPF + Avalonia in our stack** | Two planets already speak the **same markup family** |

The gap is **not** «invent a new UI graph language». The gap is an explicit **PhysicalImplementation** axis: compile-time **`realizes`** registry per `profile:*`, enforced by our toolchain — not convention in markdown.

CFIHOS Functional / Physical is **metaphor for provenance** (functional tag vs installed equipment), not a copy of the standard. Primary goal: escape **framework zoo** coupling. Agent AX / human a11y (GUIDERS-UI-0003/0004) are **optional projections** of the same IR — not the driver for this ADR.

Federation charter (GUIDERS-ADR-0006): shared functional catalog; sovereign physical planets; no annexation.

---

## Decision

### 1. Canonical pipeline

```text
FunctionalSpec  →  IR  →  PhysicalImplementation
     │              │              │
 authoring      normalized      profile:* lowering
 (intent)         graph         per planet
```

| Layer | Role | Examples |
|-------|------|----------|
| **FunctionalSpec** | What should be here — meaning, role, density, token refs, design intent | `EmptyState.HomeCatalog`, `PageChrome`, `HumanUiPanel` kind, ASP/AX node refs |
| **IR** | Platform-agnostic SSOT graph — **transport may be XAML-family subset or export-compatible graph** | normalized element tree + bindings + resource keys; no planet-specific control types |
| **PhysicalImplementation** | Lowering IR into a concrete stack | `profile:web-htmx`, `profile:wpf-glass`, `profile:avalonia-cide`, `profile:anui-skia`, `profile:maui-handler` |

**Edge name:** `realizes` — `FunctionalKind + props` → physical component + props for a given profile.

### 2. XAML family as IR — do not reinvent the graph

1. **IR reuse:** Treat XAML / AXAML (and C# kit builders that lower to the same graph) as the **primary IR transport** where planets already use them (Glass, CIDE).
2. **Profile = compiler contract:** A **profiled subset** of markup + Roslyn analyzers / compile hooks define what is valid FunctionalSpec vs forbidden physical leakage (`Button` vs `HumanUiPanel`, platform `xmlns`, attached properties outside allow-list).
3. **Physical gate:** Build fails if IR references a physical type not declared in the active profile's `realizes` table.
4. **Authoring surfaces (siblings):**
   - C# `HumanUiKit` / Core types (GUIDERS-UI-0006) → lower to IR
   - Subset XAML / AXAML for design-time tools
   - Future JSON graph export for npm consumers — **derived**, not a second SSOT

### 3. What belongs where

| In FunctionalSpec / IR | In PhysicalImplementation only |
|------------------------|----------------------------------|
| Component **kind** (`Panel`, `EmptyState`, `Table`) | Concrete control type (`System.Windows.Controls.Grid`, Avalonia `Button`) |
| Binding paths, commands, token keys | Framework-specific attached properties, visual states |
| `TestId`, role, ASP/AX node ids | Razor partial name, HTMX swap target, Skia draw op |
| Layout intent (regions, density) | Pixel-perfect platform chrome |

**Rule:** physical leakage into FunctionalSpec = **compile error**, not review nit.

### 4. Relationship to GUIDERS-UI-0005 adapters

0005 adapters remain **sibling consumers** of Physical profiles:

```text
AIGuiders.UI.Core (FunctionalSpec types + catalog)
        │
        ▼
   UI IR (normalized graph; XAML export where applicable)
        │
        ├── profile:web-htmx     → UI.Web.HTMX
        ├── profile:wpf-glass    → Glass / WPF
        ├── profile:avalonia-cide → CIDE / Avalonia
        └── profile:anui-skia    → ANUI
```

Adapters implement **one primary profile** per human surface. They do not each own a duplicate functional catalog.

### 5. Exposure profiles (not a fourth axis)

| Projection | Source | Notes |
|------------|--------|-------|
| Human a11y / ASP | Same IR graph | GUIDERS-UI-0004 |
| Agent AX | Same IR graph | GUIDERS-UI-0003 |
| AG-UI runtime wire | Orthogonal | Events over rendered surface; not IR |

### 6. Engineering kill test

Adopt this model only while:

> **Second physical adapter is cheaper than copy-pasting functional semantics into another stack.**

Ordered proof targets:

1. **Core + HTMX** — shipped baseline (`profile:web-htmx`)
2. **IR schema v0.1** — minimal node graph + `realizes` for one component
3. **Second profile** — `profile:wpf-glass` **or** `profile:avalonia-cide` (XAML-family proof) **or** types-only second web profile
4. Only then expand catalog breadth

### 7. Anti-patterns

| Anti-pattern | Why |
|--------------|-----|
| New JSON IR as **only** SSOT while XAML planets duplicate the graph by hand | Two truths; drift |
| «Profile» as README convention without compiler gate | Physical leaks back in |
| Big-bang GUIDERS-UI-0007 implementation before kill-test #3 | ADR without evidence |
| Replacing MAUI / Avalonia / WPF | They are **physical runtimes**, not competitors to FunctionalSpec |
| a11y-first scope creep | Footnote to 0003/0004; same graph, different projection |

---

## Non-goals

- Shipping all profiles before HTMX + Core conformance is stable.
- Mandating XAML authoring for Forge Razor (C# kit → IR → Razor is fine).
- UI «RDL» as a product name — canon term is **IR**.
- Federation-wide codegen monolith (`AIGuiders.Platform.Web.UI` owns all routes).

---

## Consequences

- **ui-ir-platform-v1** sub-project has a charter ADR; research note becomes supporting material, not shadow SSOT.
- Glass ↔ CIDE is the **cheapest** second-adapter experiment (shared markup mental model).
- New planet UI work: name **profile**, link `realizes` row, prove compile gate before adding functional kinds.
- Roslyn / XAML compiler investment is **in scope** for guiders-ui-platform; not optional polish.

---

## vNext (ordered)

1. `realizes` table format (YAML or embedded resource) + one row: `EmptyState.HomeCatalog` × `profile:web-htmx`
2. Roslyn analyzer: block direct physical control types in `AIGuiders.UI.Core` / kit authoring layer
3. IR export: Core kit build artifact (JSON or AXAML subset) — CI diff on change
4. Spike: same functional node → HTMX partial **and** minimal WPF or Avalonia surface (pick one)
5. Cross-link in `cascade-ide/docs/adr/` and agent-notes `note-ui-ir-platform-research-v1.md`

---

## Review gate (PR)

- [ ] Functional kind named in catalog (or explicit extension with planet scope)
- [ ] Physical type appears only behind `profile:*` / adapter project
- [ ] `TestId` + role on new interactive functional nodes
- [ ] No second dialect of the same screen without IR lowering path
- [ ] If XAML used: profile allow-list cited in PR or build target
