# GUIDERS-UI-0009: REST leg defers to OpenAPI `operationId`

**Status:** accepted (2026-08-28)  
**Tags:** #guiders #ui #openapi #invocable #rest #codegen  
**Related:** GUIDERS-UI-0008 · GUIDERS-UI-0007 · GUIDERS-UI-0005

---

## Context

GUIDERS-UI-0008 defines **InvocableSpec** for UI interact (`rest` | `event` | `navigate`). For `route_type=rest`, duplicating path, HTTP method, and parameter schemas in XAML markup would recreate a second, drift-prone API description.

**OpenAPI** already solves HTTP operation contracts: stable **`operationId`**, paths, methods, parameters, request bodies, security. Forge and other .NET planets can emit OpenAPI from ASP.NET (`Microsoft.AspNetCore.OpenApi` / Swashbuckle / NSwag).

This ADR normatively defines how the **rest leg** of InvocableSpec **defers** to planet OpenAPI.

---

## Decision

### 1. SSOT split

| Concern | SSOT | Owner |
|---------|------|--------|
| HTTP surface (path, method, schemas, security) | **Planet OpenAPI** document | Product repo (`agent-forge`, …) |
| UI node → handler wiring | **InvocableSpec** (XAML `inv:` / IR) | `guiders-ui-platform` + planet views |
| Render tree | **IR** (GUIDERS-UI-0007) | `guiders-ui-platform` |

InvocableSpec **must not** copy `paths`, `method`, or JSON Schema for `rest` when `OperationId` is set.

### 2. Link model

```text
inv:Handler (RouteType=rest, OperationId="CatalogRefresh")
        │
        ▼ resolve at compile / CI
planet openapi.json → paths.*.post.operationId == "CatalogRefresh"
        │
        ▼ codegen
hx-post / HttpClient / fetch template + param bindings
```

| Invocable field | OpenAPI source |
|-----------------|----------------|
| `OperationId` | `operation.operationId` (required, unique in document) |
| `route` + `method` | **derived** from matched path item |
| `params` | IR `{Binding}` → OpenAPI `parameters[].name` (path/query/header) |
| body fields | IR binding → `requestBody` schema property names |
| `authz` | `operation.security` / `components.securitySchemes` ref |

**`handler_id` vs `operationId`:** UI may use a local id (`ClickHandler`) that **references** `OperationId` on the `inv:Handler` row. For simple screens they may match; federation codegen keys on **`operationId`** for REST resolution.

### 3. XAML (canonical)

```xml
<inv:Handler Id="ClickHandler"
             RouteType="rest"
             OperationId="CatalogRefresh" />
```

Forbidden on `rest` handlers (compile error):

```xml
<!-- ✗ duplicates OpenAPI -->
<inv:Handler RouteType="rest" Method="POST" Route="/forge/catalog/refresh" />
```

Allowed escape (non-OpenAPI planet, **deprecated**): explicit `Method` + `Route` only when planet documents `openapi: none` in profile manifest — requires ADR waiver per product.

### 4. OpenAPI extension (optional, desktop override)

When the **same logical operation** has a desktop `event` leg, prefer declaring the REST operation in OpenAPI and attach desktop metadata:

```yaml
/forge/catalog/refresh:
  post:
    operationId: CatalogRefresh
    x-aiguiders-invocable:
      event:
        route: CatalogRefresh
        profile: wpf-glass
```

XAML may still declare profile override in `inv:HandlerRegistry`; OpenAPI extension is for **API-first** teams and cross-repo discovery. **One** override wins — CI fails on conflict.

### 5. Planet OpenAPI artifact

Each planet with `profile:web-htmx` (or any `rest` default) publishes:

| Artifact | Location (convention) |
|----------|------------------------|
| `openapi.json` or `openapi.yaml` | CI artifact and/or `docs/api/openapi.json` in planet repo |

UI platform build step receives **path or URL** to this artifact (MSBuild property `GuidersPlanetOpenApi`).

### 6. Validation gates

| Gate | When | Rule |
|------|------|------|
| **Resolve** | compile / `dotnet build` on view project | every `OperationId` in lowered IR exists in planet OpenAPI |
| **Bind** | compile | every IR binding target maps to a parameter or requestBody property on that operation |
| **Security** | codegen | operation with `security` cannot emit client without `authz` policy hook |
| **Drift** | CI | OpenAPI hash pinned or diff-reviewed when `OperationId` set changes |

Recommended tooling: **NSwag** / **Kiota** / **OpenAPI Generator** for typed clients; custom emitter for **HTMX attrs** from same resolved operation model.

### 7. Binding example

OpenAPI fragment:

```yaml
paths:
  /forge/catalog/refresh:
    post:
      operationId: CatalogRefresh
      parameters:
        - name: tenantId
          in: query
          schema: { type: string }
```

IR / XAML:

```xml
<Button inv:Handler.Click="ClickHandler"
        CommandParameter="{Binding TenantId}" />
<!-- codegen maps TenantId → query param tenantId per OpenAPI -->
```

### 8. Relationship to GUIDERS-UI-0008

| `route_type` | OpenAPI |
|--------------|---------|
| `rest` | **defers** (this ADR) |
| `event` | not in OpenAPI; `inv:Handler` or `x-aiguiders-invocable.event` |
| `navigate` | optional `operationId` link if navigation is API-driven; else route string only |

---

## Non-goals

- Generating planet OpenAPI from UI markup (UI-first API design).
- Replacing OpenAPI 3.1 / JSON Schema — we consume, not fork.
- Federation-wide merged mega-spec — each planet keeps its own document; UI resolves against **active planet** profile.

---

## Consequences

- Forge Human View: add `openapi.json` to CI; HumanUi components reference `operationId` only.
- Glass desktop: `event` leg unchanged; may share `operationId` name as event route convention.
- Agent AX `commandHints` may cite `operationId` for REST-discoverable actions.

---

## vNext (ordered)

1. Forge: export `openapi.json` in CI (ASP.NET OpenAPI)
2. MSBuild target: `ValidateInvocableOperations` against `GuidersPlanetOpenApi`
3. HTMX emitter prototype: `CatalogRefresh` → `hx-post` + `hx-vals` from resolved operation
4. Document `x-aiguiders-invocable` in planet OpenAPI template

---

## Review gate (PR)

- [ ] `rest` handler uses `OperationId`, not inline path/method
- [ ] `OperationId` exists in planet OpenAPI artifact attached to build
- [ ] Bindings match OpenAPI parameter / body names
- [ ] Security on operation has matching authz hook or documented waiver
