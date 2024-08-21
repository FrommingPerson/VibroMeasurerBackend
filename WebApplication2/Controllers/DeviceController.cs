using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Controllers;

[ApiController]
[Route("[controller]")]
public class DeviceController: ControllerBase
{
    private readonly MineDbContext _context;

    public DeviceController(MineDbContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<List<Device>> GetAll()
    {
        var devices = await _context.Devices.ToListAsync();
        return devices;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<Device> Get(int id)
    {
        var device = await _context.Devices.FirstOrDefaultAsync(x => x.DeviceId == id);
        if (device == null) throw new Exception($"There is no this device with this id: {id}");
        return device;
    }
}