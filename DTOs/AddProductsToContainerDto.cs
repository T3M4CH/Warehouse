using System.ComponentModel.DataAnnotations;

namespace Warehouse.DTOs;

public class AddProductsToContainerDto
{
    [Required] public int ContainerId { get; set; }
    [Required, MinLength(1)] public ICollection<int> ProductIds { get; set; } = new List<int>();
}