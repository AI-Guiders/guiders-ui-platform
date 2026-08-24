# GUIDERS-UI-0001: Monorepo bootstrap

**Status:** accepted (2026-08-25)

## Context

Human+agent UI patterns (page chrome, empty states, catalog semantics) were embedded in product repos (Forge `Plugin.View`). GUIDERS-ADR-0001 keeps guiders-platform headless; UI extraction needs its own monorepo.

## Decision

1. **`guiders-ui-platform` monorepo** — publishes `AIGuiders.UI.*` on nuget.org.
2. **Sibling** to guiders-platform and guiders-core — not nested.
3. **Layering:** Core (contracts) · Tokens (CSS) · Adapters (`Web.HTMX`, later SPA).
4. **Products** consume via `GuidersUiPlatformRoot` sibling `ProjectReference` or NuGet.

## Consequences

- Forge Human View becomes a reference consumer, not cross-product UI SSOT.
- First adapter is Razor MPA; React/Vue are future packages sharing Core.
