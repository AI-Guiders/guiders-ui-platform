# Публикация на nuget.org (Trusted Publishing)

Долгоживущий API key **не нужен**: [Trusted Publishing](https://learn.microsoft.com/nuget/nuget-org/trusted-publishers) + GitHub OIDC (`NuGet/login@v1`).

## 1. Политика на nuget.org (один раз, вручную)

1. Войти на [nuget.org](https://www.nuget.org/) (**LonelySoul**).
2. **Account settings** → **Trusted publishers** → **Add**.
3. **Provider:** GitHub
4. Заполнить **ровно так**:

| Поле | Значение |
|------|----------|
| **Repository owner** | `AI-Guiders` |
| **Repository name** | `guiders-ui-platform` |
| **Workflow filename** | `release.yml` |
| **Environment name** | *(пусто)* |

5. **Save**.

Одна политика покрывает **все** пакеты из [`.github/workflows/release.yml`](../.github/workflows/release.yml):

| PackageId | Версия (v0.1.0) |
|-----------|-----------------|
| `AIGuiders.UI.Core` | `0.1.0` |
| `AIGuiders.UI.Tokens` | `0.1.0` |
| `AIGuiders.UI.Web.HTMX` | `0.1.0` |

**Package scope (glob):** `AIGuiders.UI.*` — отдельный glob на nuget.org **не** задаётся; фильтр в workflow: `artifacts/AIGuiders.UI.*.nupkg`.

### Соседние политики (не удалять)

| Owner | Repository | Workflow |
|-------|------------|----------|
| `AI-Guiders` | `guiders-core` | `release.yml` |
| `AI-Guiders` | `guiders-platform` | `release.yml` |

Три sibling monorepo = **три** Trusted Publisher записи. Не конфликтуют.

## 2. Версии пакетов

Версия из `<Version>` в [`Directory.Build.props`](../Directory.Build.props) (общая для всех ship-пакетов) — **не** из имени тега.

Перед релизом bump `Version` в `Directory.Build.props`. Тег `v*` — только триггер CI; `--skip-duplicate` пропускает уже опубликованные версии.

## 3. Запуск первого релиза (0.1.0)

**Порядок:**

1. Убедиться, что TP-политика сохранена (§1).
2. `main`/`master` зелёный (`ci.yml`).
3. Merge workflow `release.yml` (если ещё не в default branch).
4. Тег и push:

```bash
git tag v0.1.0
git push origin v0.1.0
```

5. **Actions** → `release` → дождаться green.
6. Проверить страницы пакетов на nuget.org.

Альтернатива: **Actions → release → Run workflow** (после добавления `workflow_dispatch` — опционально).

## 4. Локальная проверка (до тега)

```bash
dotnet test -c Release
dotnet pack -c Release --output ./artifacts
ls artifacts/AIGuiders.UI.*.nupkg
```

Ожидается **3** nupkg.

## 5. Проверка после CI

```bash
dotnet add package AIGuiders.UI.Core -v 0.1.0
dotnet add package AIGuiders.UI.Tokens -v 0.1.0
dotnet add package AIGuiders.UI.Web.HTMX -v 0.1.0
```

Forge fallback NuGet pin (`0.1.0`) начнёт резолвиться после успешного push.

## 6. Troubleshooting

| Симптом | Что проверить |
|---------|----------------|
| `403` / trusted publisher | Owner/repo/workflow **байт-в-байт** как в §1; workflow file в default branch |
| `409` duplicate | Версия уже на nuget — bump `Version` или ожидаемо `--skip-duplicate` |
| Пустой push loop | Нет nupkg под glob — `dotnet pack` локально, смотреть `artifacts/` |
| OIDC failed | `permissions: id-token: write` в workflow |
