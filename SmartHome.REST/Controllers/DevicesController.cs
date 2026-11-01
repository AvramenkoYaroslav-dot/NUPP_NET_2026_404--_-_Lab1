using Microsoft.AspNetCore.Mvc;
using SmartHome.Common;
using SmartHome.Infrastructure.Models;
using SmartHome.Infrastructure;

[Route("api/[controller]")]
[ApiController]
public class DevicesController : ControllerBase
{
    private readonly ICrudServiceAsync<DeviceModel> _deviceService;

    public DevicesController(ICrudServiceAsync<DeviceModel> deviceService)
    {
        _deviceService = deviceService;
    }

    // GET: api/devices
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DeviceModel>>> GetDevices()
    {
        var devices = await _deviceService.ReadAllAsync();
        return Ok(devices);
    }

    // GET: api/devices/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DeviceModel>> GetDevice(Guid id)
    {
        var device = await _deviceService.ReadAsync(id);
        if (device == null)
        {
            return NotFound(new { message = $"Пристрій з ID {id} не знайдено." }); // 404 Not Found
        }
        return Ok(device);
    }

    // GET: api/devices/paginated?page=1&amount=5
    [HttpGet("paginated")]
    public async Task<ActionResult<IEnumerable<DeviceModel>>> GetPaginatedDevices(int page = 1, int amount = 10)
    {
        var devices = await _deviceService.ReadAllAsync(page, amount);
        return Ok(devices);
    }

    // POST: api/devices
    [HttpPost]
    public async Task<ActionResult<DeviceModel>> PostDevice([FromBody] SmartLightModel device)
    {
        if (device == null) return BadRequest("Некоректні дані.");

        var success = await _deviceService.CreateAsync(device);
        if (!success) return BadRequest("Не вдалося створити пристрій.");

        // 201 Created з посиланням на новий ресурс
        return CreatedAtAction(nameof(GetDevice), new { id = device.Id }, device);
    }

    // PUT: api/devices
    [HttpPut]
    public async Task<IActionResult> PutDevice([FromBody] SmartLightModel device)
    {
        var existing = await _deviceService.ReadAsync(device.Id);
        if (existing == null) return NotFound($"Пристрій з ID {device.Id} не знайдено.");

        var success = await _deviceService.UpdateAsync(device);
        if (!success) return BadRequest("Помилка оновлення.");

        return NoContent(); // 204 No Content
    }

    // DELETE: api/devices/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteDevice(Guid id)
    {
        var device = await _deviceService.ReadAsync(id);
        if (device == null) return NotFound($"Пристрій з ID {id} не знайдено.");

        await _deviceService.RemoveAsync(device);
        return NoContent(); // 204 No Content
    }
}