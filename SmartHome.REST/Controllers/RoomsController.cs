using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHome.Infrastructure;
using SmartHome.Infrastructure.Models;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly SmartHomeContext _context;

    public RoomsController(SmartHomeContext context)
    {
        _context = context;
    }

    // GET: api/rooms
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomModel>>> GetRooms()
    {
        var rooms = await _context.Rooms
            .Include(r => r.Devices)
            .Include(r => r.ClimateSensor)
            .ToListAsync();
        return Ok(rooms);
    }

    // GET: api/rooms/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RoomModel>> GetRoom(int id)
    {
        var room = await _context.Rooms
            .Include(r => r.Devices)
            .Include(r => r.ClimateSensor)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (room == null)
        {
            return NotFound(new { message = $"Кімнату з ID {id} не знайдено." });
        }

        return Ok(room);
    }

    // POST: api/rooms
    [HttpPost]
    public async Task<ActionResult<RoomModel>> PostRoom([FromBody] RoomModel room)
    {
        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
    }

    // DELETE: api/rooms/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteRoom(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null) return NotFound($"Кімнату з ID {id} не знайдено.");

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}