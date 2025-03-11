using System.ComponentModel.DataAnnotations;

namespace Warehouse.DTOs;

public class SendContainerDto
{
    [Required] public int ContainerId { get; set; }
    [Required] public int SenderWarehouseId { get; set; }
    [Required] public int TargetWarehouseId { get; set; }
}