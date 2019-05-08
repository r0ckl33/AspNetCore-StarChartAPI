using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var item = _context.CelestialObjects.FirstOrDefault(e => e.Id == id);

            if (item == null)
            {
                return NotFound();
            }
            else
            {
                item.Satellites = _context.CelestialObjects.Where(s => s.OrbitedObjectId == id).ToList();

                return Ok(item);
            }
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(e => e.Name == name).ToList();

            if (celestialObjects == null || celestialObjects.Count == 0)
            {
                return NotFound();
            }
            else
            {
                foreach (var item in celestialObjects)
                {
                    item.Satellites = _context.CelestialObjects.Where(s => s.OrbitedObjectId == item.Id).ToList();
                }

                return Ok(celestialObjects);
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();

            foreach (var item in celestialObjects)
            {
                item.Satellites = _context.CelestialObjects.Where(s => s.OrbitedObjectId == item.Id).ToList();
            }

            return Ok(celestialObjects);
        }
    }
}
