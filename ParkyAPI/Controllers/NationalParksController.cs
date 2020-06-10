using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using ParkyAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models.Dtos;

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
            _npRepo= npRepo;
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
    }
}