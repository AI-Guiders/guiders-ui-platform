using AIGuiders.UI.Core.Html;

namespace AIGuiders.UI.Core.Kit;

/// <summary>Generic table primitive — layout/chrome only; cell HTML from rows.</summary>
public readonly record struct HumanUiTableColumn(
    string Header,
    string? HeaderClass = null,
    string? CellClass = null);

public readonly record struct HumanUiTableRow(
    string? RowClass,
    IReadOnlyList<string> CellsHtml,
    IReadOnlyList<string>? CellClasses = null);

public static class HumanUiTable
{
    public const string DefaultPanelClass = HumanUiPanel.CatalogPanelClass;

    public static string Render(
        IReadOnlyList<HumanUiTableColumn> columns,
        IReadOnlyList<HumanUiTableRow> rows,
        string tableClass = "human-table",
        IReadOnlyList<string>? prependBodyRows = null,
        string? dataTestId = null)
    {
        if (columns.Count == 0)
            throw new ArgumentException("Table requires at least one column.", nameof(columns));

        var headerCells = columns
            .Select(c => string.IsNullOrEmpty(c.HeaderClass)
                ? HumanUiHtml.Th(c.Header)
                : HumanUiHtml.Th(c.HeaderClass, c.Header))
            .ToArray();

        var body = string.Concat((prependBodyRows ?? []).Select(r => r));
        body += string.Concat(rows.Select(r => RenderRow(columns, r)));

        var tableInner = HumanUiHtml.Fragment(
            HumanUiHtml.Thead(HumanUiHtml.HeaderRow(headerCells)),
            HumanUiHtml.Tbody(body));

        return string.IsNullOrWhiteSpace(dataTestId)
            ? HumanUiHtml.Table(tableClass, tableInner)
            : HumanUiHtml.TableWithTestId(tableClass, dataTestId, tableInner);
    }

    public static string RenderPanel(
        IReadOnlyList<HumanUiTableColumn> columns,
        IReadOnlyList<HumanUiTableRow> rows,
        string tableClass = "human-table",
        string? panelClass = DefaultPanelClass,
        string? islandId = null,
        string? emptyMessage = null,
        IReadOnlyList<string>? prependBodyRows = null,
        string? dataTestId = null)
    {
        var hasBody = rows.Count > 0 || (prependBodyRows is { Count: > 0 });
        if (!hasBody)
        {
            var empty = RenderPanel(
                columns,
                [EmptyRow(columns, emptyMessage ?? "Nothing here yet.")],
                tableClass,
                panelClass,
                islandId: null,
                emptyMessage: null,
                prependBodyRows: null,
                dataTestId);
            return WrapIsland(islandId, empty);
        }

        var table = Render(columns, rows, tableClass, prependBodyRows, dataTestId);
        var panel = HumanUiPanel.Render(table, panelClass);
        return WrapIsland(islandId, panel);
    }

    public static HumanUiTableRow EmptyRow(IReadOnlyList<HumanUiTableColumn> columns, string message)
    {
        var cells = new List<string>(columns.Count);
        for (var i = 0; i < columns.Count; i++)
        {
            cells.Add(i == 0
                ? HumanUiHtml.Span("muted", HumanUiHtml.Text(message))
                : HumanUiHtml.Text("—"));
        }

        return new HumanUiTableRow("rule-row rule-empty", cells);
    }

    public static string WrapIsland(string? islandId, string inner) =>
        HumanUiPanel.WithIsland(islandId, inner);

    private static string RenderRow(IReadOnlyList<HumanUiTableColumn> columns, HumanUiTableRow row)
    {
        if (row.CellsHtml.Count != columns.Count)
            throw new ArgumentException(
                $"Row has {row.CellsHtml.Count} cells but table defines {columns.Count} columns.");

        var cells = new string[columns.Count];
        for (var i = 0; i < columns.Count; i++)
        {
            var cellClass = row.CellClasses is not null && i < row.CellClasses.Count && !string.IsNullOrEmpty(row.CellClasses[i])
                ? row.CellClasses[i]
                : columns[i].CellClass;
            cells[i] = string.IsNullOrEmpty(cellClass)
                ? HumanUiHtml.Td(HumanUiHtml.Raw(row.CellsHtml[i]))
                : HumanUiHtml.Td(cellClass, HumanUiHtml.Raw(row.CellsHtml[i]));
        }

        return HumanUiHtml.Tr(row.RowClass, null, cells);
    }
}
