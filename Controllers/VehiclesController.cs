using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using clase_3.Models;

namespace clase_3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public VehiclesController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: api/Vehicles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles()
        {
            return await _context.Vehicles.ToListAsync();
        }

        // GET: api/Vehicles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle>> GetVehicle(long id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }

            return vehicle;
        }

        // PUT: api/Vehicles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicle(long id, Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return BadRequest();
            }

            _context.Entry(vehicle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Vehicles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Vehicle>> PostVehicle(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVehicle", new { id = vehicle.Id }, vehicle);
        }

        // DELETE: api/Vehicles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Vehicle>> DeleteVehicle(long id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            return vehicle;
        }



        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<Vehicle> vehicle)
        {
            if (vehicle == null)
            {
                return BadRequest();
            }
            var vehicleFromDB = await _context.Vehicles.FirstOrDefaultAsync(x => x.Id == id);

            if (vehicleFromDB == null)
            {
                return NotFound();
            }

            vehicle.ApplyTo(vehicleFromDB, ModelState);

            var isValid = TryValidateModel(vehicleFromDB);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            await _context.SaveChangesAsync();

            return NoContent();
            
        }

        private bool VehicleExists(long id)
        {
            return _context.Vehicles.Any(e => e.Id == id);
        }
    }
}
