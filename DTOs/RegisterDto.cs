using System.ComponentModel.DataAnnotations;

namespace Warehouse.DTOs;

public record RegisterDto
{
    [MaxLength(20), MinLength(2)] public required string UserName { get; set; }

    public required string Password { get; set; }
}