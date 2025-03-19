namespace Warehouse.DTOs;

public record SendContainerDto
{
    public required int ContainerId { get; set; }
    public required int SenderWarehouseId { get; set; }
    public required int TargetWarehouseId { get; set; }
}