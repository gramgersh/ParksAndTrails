using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParksController : Controller
    {


        // This contains a reference to all of the db routines defined
        // in the Repository.
        private INationalParkRepository _npRepository;

        // This gives us access to the mapping methods.
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepository = npRepo;
            _mapper = mapper;
        }


    }
}