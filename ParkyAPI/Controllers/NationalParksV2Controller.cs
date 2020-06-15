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
    // [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/nationalparks")]
    [ApiVersion("2.0")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    // All methods return a 400 Bad Request
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksV2Controller : ControllerBase
    {


        // This contains a reference to all of the db routines defined
        // in the Repository.
        private readonly INationalParkRepository _npRepo;

        // This gives us access to the mapping methods.
        private readonly IMapper _mapper;

        public NationalParksV2Controller(INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }

        // The HTTP methods
        /// <summary>
        /// Get a list of National Parks.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        // Add a couple of ProducesResponseType() attributes to help with
        // the Swagger documentation
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(List<NationalParkDto>))]
        public IActionResult GetNationalParks()
        {
            // To differentiate between v1 and V2, just send back the first one.
            var obj = _npRepo.GetNationalParks().FirstOrDefault();
            return Ok(_mapper.Map<NationalParkDto>(obj));
            //   var objList = _npRepo.GetNationalParks();
            //   var objDto = new List<NationalParkDto>();

            //foreach (var obj in objList)
            //{
            //    objDto.Add(_mapper.Map<NationalParkDto>(obj));
            // }

            // return Ok(objDto);
        }
    }
}