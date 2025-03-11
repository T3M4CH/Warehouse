using System.ComponentModel.DataAnnotations;

namespace Warehouse.DTOs;

public class CreateWarehouseDto
{
    [Required] public string Location { get; set; } 
}