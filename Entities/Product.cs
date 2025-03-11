using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Warehouse.Enums;

namespace Warehouse.Entities;

public class Product
{
    public int Id { get; set; }
    [Required] public string Name { get; set; }
    [Required] public double Weight { get; set; }

    public EProductType Category { get; set; }

    public int? ContainerId { get; set; }
}