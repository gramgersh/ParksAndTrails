using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using ParkyAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Models;

namespace ParkyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParksController : Controller
    {


        // This contains a reference to all of the db routines defined
        // in the Repository.
        private INationalParkRepository _npRepo;

        // This gives us access to the mapping methods.
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }

        // The HTTP methods
        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var objList = _npRepo.GetNationalParks();
            var objDto = new List<NationalParkDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<NationalParkDto>(obj));
            }

            return Ok(objDto);
        }

        // Get single park.  Tell HttpGet that we expect a parameter
        // so that we don't clash with the above HttpGet which has
        // no parameters defined.
        // Set the name so that we can call this using that name from other
        // verbs.
        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int nationalParkID)
        {
            var obj = _npRepo.GetNationalPark(nationalParkID);
            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<NationalParkDto>(obj);
            return Ok(objDto);
        }

        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_npRepo.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Exists!");
                return StatusCode(404, ModelState);
            }
            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);

            // If the Create() method succeeds, the object passed in will be
            // filled with all of the created properties.  In this case, the
            // nationalParkObj will have an Id set.
            if (!_npRepo.CreateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            // Rather than return just OK, run the "GetNationalPark" method (matches
            // the Name= and not the method name).  This method takes the int nationalParkId
            // option, so create a new object with the values that were filled in from
            // the Create() method above, and send in the object that was filled in.
            return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalParkObj.Id }, nationalParkObj);
        }

        [HttpPut("{nationalParkId:int}", Name ="UpdateNationalPark")]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody]NationalParkDto nationalParkDto)
        {

            if (nationalParkDto == null || nationalParkId != nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }
            if (! _npRepo.NationalParkExists(nationalParkId))
            {
                ModelState.AddModelError("", "National Park does not exist!");
                return StatusCode(404, ModelState);
            }

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);
            if ( ! _npRepo.UpdateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}