using Microsoft.AspNetCore.Mvc;
using OccupationApi.Models;

namespace OccupationApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OccupationsController : ControllerBase
{
    // Hardcoded mock data representing your table rows
    private static readonly List<Occupation> _occupations = new()
    {
        new Occupation { Id = 1, Name = "Cleaner", Rating = "Light Manual" },
        new Occupation { Id = 2, Name = "Doctor", Rating = "Professional" },
        new Occupation { Id = 3, Name = "Author", Rating = "White Collar" },
        new Occupation { Id = 4, Name = "Farmer", Rating = "Heavy Manual" },
        new Occupation { Id = 5, Name = "Mechanic", Rating = "Heavy Manual" },
        new Occupation { Id = 6, Name = "Florist", Rating = "Light Manual" },
        new Occupation { Id = 7, Name = "Other", Rating = "Heavy Manual" }
    };

    [HttpGet]
    public ActionResult<IEnumerable<Occupation>> GetAll()
    {
        return Ok(_occupations);
    }
}
