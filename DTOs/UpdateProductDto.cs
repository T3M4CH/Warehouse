using System.ComponentModel.DataAnnotations;
using Warehouse.Enums;

namespace Warehouse.DTOs;

public record UpdateProductDto
{
    public string? Name { get; set; }
    [Range(0, 100.0)] public double? Weight { get; set; }

    public string? PassId { get; set; }
    public string? Size { get; set; }
    public DateTime? ExpiryDate { get; set; }
}