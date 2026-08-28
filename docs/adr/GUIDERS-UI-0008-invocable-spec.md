# GUIDERS-UI-0008: InvocableSpec — action contract behind Physical UI

**Status:** accepted (2026-08-28)  
**Tags:** #guiders #ui #invocable #codegen #rest #event #federation  
**Related:** GUIDERS-UI-0007 · GUIDERS-UI-0003 · GUIDERS-UI-0005 · GUIDERS-ADR-0006

---

## Context

GUIDERS-UI-0007 fixes the **render** axis:

```text
FunctionalSpec  →  IR  →  PhysicalImplementation
```

XAML-family IR already covers trees, bindings, and styles. Physical profiles describe **how a node renders** (Angular template, WPF control, Razor partial).

What IR does **not** normalize is **what happens on interact**: the same «Submit» on web is `POST /api/...`, on desktop it may be `ICommand` → local handler → optional HTTP later. Today each planet hand-wires this; codegen stops at markup.

**InvocableSpec** names the **domain operation** behind a physical wire (`onClick`, `(click)`, `hx-post`, `Command=`) so one functional handler can drive generated clients and commands across planets.

### Out of scope for InvocableSpec

These are **not** InvocableSpec `route_type` values:

| Concern | Where it lives | Why not here |
|---------|----------------|--------------|
| **MCP tool calls** | Agent AX / planet MCP host | Agent ingress, not human widget wiring |
| **Federation intent / slash-DOI** | `guiders-platform` routing, CDP organs | Runtime command plane; not UI codegen input |
| **AG-UI event stream** | Runtime wire over rendered surface | Orthogonal to SSOT graph |

A physical button may **eventually** trigger something that also surfaces as an MCP tool or platform intent — but InvocableSpec describes the **handler the UI binds to**, not every consumer of that handler. Agent `commandHints[]` (GUIDERS-UI-0003) may **reference the same handler id**; that is a projection, not a second SSOT.

---

## Decision

### 1. Extended pipeline

```text
FunctionalSpec  →  IR  →  PhysicalImplementation  →  InvocableSpec
     │              │              │                        │
  meaning        graph         render profile          action contract
```

- **PhysicalImplementation** chooses widget + local wire (`onClick`, `Command`, `hx-post`).
- **InvocableSpec** declares **handler id** + **how to reach the operation** for the active planet/runtime class.

### 2. InvocableSpec shape (conceptual)

Each interactable IR node may reference one or more named handlers (e.g. `ClickHandler`, `SubmitHandler`):

| Field | Role |
|-------|------|
| `handler_id` | Stable id within functional component (links AX `commandHints` by reference) |
| `route_type` | See §3 |
| `route` | Target: URL template, event name, or navigation path |
| `method` | HTTP verb when `route_type=rest` |
| `params` | Bound names / schema refs (from IR bindings) |
| `authz` | Optional policy ref (required before public codegen) |

Authoring may be **attributes**, **IR YAML/JSON**, or **C# records** — no mandatory mini-DSL.

### 3. `route_type` (v1 — three values only)

| `route_type` | Runtime class | Physical lowering examples |
|--------------|---------------|----------------------------|
| **`rest`** | Web / HTTP-capable | `hx-post`, `fetch()`, Angular `HttpClient`, generated API client |
| **`event`** | Desktop / in-process | WPF `ICommand`, Avalonia `ReactiveCommand`, local event bus, view-model method |
| **`navigate`** | Any | Route change without mutation — `href`, `Router.navigate`, `NavigationService` |

**Profile rule:** a planet picks a **default** `route_type` for codegen (Forge → `rest`; Glass → `event`) but the **same `handler_id`** may declare **profile overrides** when one functional screen ships on multiple runtime classes.

Example (attribute sketch):

```csharp
[Invocable("ClickHandler",
    RouteType = InvocableRoute.Rest,
    Method = "POST",
    Route = "/forge/catalog/refresh")]
[Invocable("ClickHandler",
    RouteType = InvocableRoute.Event,
    Route = "CatalogRefresh",
    Profile = "wpf-glass")]
public partial class HomeCatalogEmptyState { }
```

### 4. Codegen consequence

With FunctionalSpec + IR + Physical profile + InvocableSpec, the platform can generate:

| Artifact | Source |
|----------|--------|
| Markup / components | IR + Physical profile |
| HTTP client / HTMX attrs | InvocableSpec `rest` |
| Commands / event subscriptions | InvocableSpec `event` |
| Journey / contract test stubs | `handler_id` + `route` |
| Agent AX hints (optional) | **Reference** `handler_id` — not duplicate route tables |

**Kill test (extends 0007):** one `handler_id` → generated REST binding **and** generated local event binding without copy-pasting route strings in product code.

### 5. Compiler gate

Same discipline as 0007:

- Physical wire without `InvocableSpec` on a **catalog interactable** → **warning** (v1) → **error** (v2).
- `route_type` not allowed for active profile → compile error.
- `rest` route without `method` → compile error.

### 6. Relationship to GUIDERS-UI-0007

| Layer | Question |
|-------|----------|
| FunctionalSpec | What is this node? |
| IR | Tree + data bindings |
| PhysicalImplementation | How does it look? |
| **InvocableSpec** | What runs when the user acts? |

InvocableSpec does **not** replace REST API design or domain services — it **links** UI nodes to already-owned endpoints/handlers.

---

## Non-goals

- Generating MCP servers or tool manifests from InvocableSpec.
- Encoding CDP / federation intent grammar in UI SSOT.
- 100% UI codegen for rich islands (editors, diff, charts) — escape hatch remains.
- Replacing OpenAPI as API SSOT — OpenAPI may **inform** `rest` routes; InvocableSpec **attaches** them to UI nodes.

---

## Consequences

- Forge HTMX and Glass WPF can share **handler ids** while codegen emits different `route_type` lowerings.
- Product repos stop hand-writing duplicate `hx-post` + C# command strings for catalog components.
- Agent AX work references `handler_id`; no parallel MCP route table in UI Core.

---

## vNext (ordered)

1. `InvocableRoute` enum + attribute prototype in `AIGuiders.UI.Core`
2. One catalog component (`EmptyState.HomeCatalog` or `HumanUiPanel` action) with `rest` + `event` overrides
3. Codegen spike: HTMX attrs from InvocableSpec (Forge)
4. Codegen spike: `ICommand` stub from same spec (Glass or Avalonia)
5. CI: handler id stable across generated artifacts

---

## Review gate (PR)

- [ ] `handler_id` named and stable
- [ ] `route_type` matches planet profile or explicit override documented
- [ ] `rest` routes have `method` + authz or `#TODO-authz` with issue link
- [ ] No MCP / intent routes in InvocableSpec — use platform layers instead
- [ ] Agent hints, if any, reference `handler_id` only
