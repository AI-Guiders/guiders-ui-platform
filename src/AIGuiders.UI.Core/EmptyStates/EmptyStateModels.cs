namespace AIGuiders.UI.Core.EmptyStates;

public sealed record EmptyStateMessageModel(string Message);

public sealed record HomeCatalogEmptyModel(
    string Title = "No repositories yet",
    string TestId = "human-ui-home-empty");

public sealed record CreateRepoHintModel(
    string? Message = null);
