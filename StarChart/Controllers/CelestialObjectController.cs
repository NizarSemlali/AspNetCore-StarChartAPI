using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [ApiController]
    [Route("")]
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
            var celestialObject = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);

            if (celestialObject == null)
            {
                return NotFound();
            } 

            foreach( var orbited in _context.CelestialObjects)
            {
                if (orbited.OrbitedObjectId == id)
                {
                    celestialObject.Satellites.Add(orbited);
                }
            }

            return Ok(celestialObject);

        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(c => c.Name == name).ToList();

            if (!celestialObjects.Any())
            {
                return NotFound();
            }

            foreach (var celestialObject in celestialObjects)
            {
                foreach ( var orbited in _context.CelestialObjects)
                {
                    if (orbited.OrbitedObjectId == celestialObject.Id)
                    {
                        celestialObject.Satellites.Add(orbited);
                    }

                }

            }

            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {

            foreach (var celestialObject in _context.CelestialObjects)
            {
                foreach (var orbited in _context.CelestialObjects)
                {
                    if (orbited.OrbitedObjectId == celestialObject.Id)
                    {
                        celestialObject.Satellites.Add(orbited);
                    }

                }

            }

            return Ok(_context.CelestialObjects);

        }

    }
}
