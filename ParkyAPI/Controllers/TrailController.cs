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
    // All methods return a 400 Bad Request
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : Controller
    {


        // This contains a reference to all of the db routines defined
        // in the Repository.
        private readonly ITrailRepository _trailRepo;

        // This gives us access to the mapping methods.
        private readonly IMapper _mapper;

        public TrailsController(ITrailRepository trailRepo, IMapper mapper)
        {
            _trailRepo = trailRepo;
            _mapper = mapper;
        }

        // The HTTP methods
        /// <summary>
        /// Get a list of Trail.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        // Add a couple of ProducesResponseType() attributes to help with
        // the Swagger documentation
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(List<TrailDto>))]
        public IActionResult GetTrails()
        {
            var objList = _trailRepo.GetTrails();
            var objDto = new List<TrailDto>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }

            return Ok(objDto);
        }

        // Get single park.  Tell HttpGet that we expect a parameter
        // so that we don't clash with the above HttpGet which has
        // no parameters defined.
        // Set the name so that we can call this using that name from other
        // verbs.
        /// <summary>
        /// Retrieve a single Trail.
        /// </summary>
        /// <param name="trailID">The Id of the Trail.</param>
        /// <returns></returns>
        [HttpGet("{trailId:int}", Name = "GetTrail")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int trailID)
        {
            var obj = _trailRepo.GetTrail(trailID);
            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<TrailDto>(obj);
            return Ok(objDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created,Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
        {
            if (trailDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_trailRepo.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail Exists!");
                return StatusCode(StatusCodes.Status409Conflict, ModelState);
            }
            var trailObj = _mapper.Map<Trail>(trailDto);

            // If the Create() method succeeds, the object passed in will be
            // filled with all of the created properties.  In this case, the
            // trailObj will have an Id set.
            if (!_trailRepo.CreateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            // Rather than return just OK, run the "GetTrail" method (matches
            // the Name= and not the method name).  This method takes the int trailId
            // option, so create a new object with the values that were filled in from
            // the Create() method above, and send in the object that was filled in.
            return CreatedAtRoute("GetTrail", new { trailId = trailObj.Id }, trailObj);
        }

        [HttpPatch("{trailId:int}", Name ="UpdateTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int trailId, [FromBody]TrailUpdateDto trailDto)
        {

            if (trailDto == null || trailId != trailDto.Id)
            {
                return BadRequest(ModelState);
            }
            if (! _trailRepo.TrailExists(trailId))
            {
                ModelState.AddModelError("", "Trail does not exist!");
                return StatusCode(StatusCodes.Status404NotFound, ModelState);
            }

            var trailObj = _mapper.Map<Trail>(trailDto);
            if ( ! _trailRepo.UpdateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{trailId:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int trailId)
        {

            if (!_trailRepo.TrailExists(trailId))
            {
                ModelState.AddModelError("", "Trail does not exist!");
                return StatusCode(404, ModelState);
            }

            var trailObj = _trailRepo.GetTrail(trailId);
            if (!_trailRepo.DeleteTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}