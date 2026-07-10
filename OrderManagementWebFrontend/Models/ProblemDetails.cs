using System.Text.Json.Serialization;

namespace OrderManagementWebFrontend.Models;

public record ProblemDetails(
    [property: JsonPropertyName("type")] string? Type,
    [property: JsonPropertyName("title")] string? Title,
    [property: JsonPropertyName("status")] int? Status,
    [property: JsonPropertyName("detail")] string? Detail,
    [property: JsonPropertyName("instance")] string? Instance
);