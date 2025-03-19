using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Warehouse.Enums;

namespace Warehouse.DTOs;

public record AddProductDto
{
    public required string Name { get; set; }
    [Range(0, 100.0)] public required double Weight { get; set; }
    public EProductType Category { get; set; }

    public string? PassId { get; set; }
    public string? Size { get; set; }
    public DateTime ExpiryDate { get; set; }
}