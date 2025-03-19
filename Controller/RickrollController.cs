using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Controller;

[Route("v1/rickroll")]
public class RickrollController : BaseApiController
{
    [HttpGet, AllowAnonymous]
    public IActionResult GetRickRoll()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/videos/rickroll.mp4");

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("Видео не найдено.");
        }

        var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return File(stream, "video/mp4", enableRangeProcessing: true);
    }
}