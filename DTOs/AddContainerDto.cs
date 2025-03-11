using System.ComponentModel.DataAnnotations;

namespace Warehouse.DTOs;

public record AddContainerDto
{
    [Required] public int ContainerId { get; set; }
    [Required] public int WarehouseId { get; set; }
}