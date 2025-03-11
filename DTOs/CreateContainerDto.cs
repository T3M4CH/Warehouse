using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Warehouse.Enums;

namespace Warehouse.DTOs;

public record CreateContainerDto()
{
    [Required, Range(5.0, 10000.0)] public double MaxWeight { get; set; }
    [Required, JsonConverter(typeof(JsonStringEnumConverter))] public EContainerType Type { get; set; }
    [Required, JsonConverter(typeof(JsonStringEnumConverter))] public EProductType Category { get; set; }
    public int? WarehouseId { get; set; }
}