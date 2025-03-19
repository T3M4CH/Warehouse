using System.ComponentModel.DataAnnotations;

namespace Warehouse.DTOs;

public class AddProductsToContainerDto
{
    [Required, MinLength(1)] public ICollection<int> ProductIds { get; set; } = new List<int>();
}