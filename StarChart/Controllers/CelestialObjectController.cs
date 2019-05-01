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
            var currentObj = _context.CelestialObjects.FirstOrDefault(co => co.Id == id);
            if (currentObj == null)
            {
                return NotFound();
            }

            var listOrbitals = _context.CelestialObjects.Where(o => o.OrbitedObjectId == currentObj.Id).ToList();
            if (listOrbitals.Any())
            {
                currentObj.Satellites = listOrbitals;
            }

            return Ok(currentObj);
        }

        [HttpGet("{name}", Name = "GetByName")]
        public IActionResult GetByName(string name)
        {
            var resultList = _context.CelestialObjects.Where(co => co.Name == name).ToList();
            if (!resultList.Any())
            {
                return NotFound();
            }

            foreach (var currentObject in resultList)
            {
                var listOrbitals = _context.CelestialObjects.Where(o => o.OrbitedObjectId == currentObject.Id).ToList();
                if (listOrbitals.Any())
                {
                    currentObject.Satellites = listOrbitals;
                }
            }

            return Ok(resultList);
        }

        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll()
        {
            var resultList = _context.CelestialObjects.ToList();
            if (!resultList.Any())
            {
                return NotFound();
            }

            foreach (var currentObject in resultList)
            {
                var listOrbitals = _context.CelestialObjects.Where(o => o.OrbitedObjectId == currentObject.Id).ToList();
                if (listOrbitals.Any())
                {
                    currentObject.Satellites = listOrbitals;
                }
            }

            return Ok(resultList);
        }
    }
}
