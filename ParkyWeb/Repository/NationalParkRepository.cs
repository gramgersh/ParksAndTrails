using System.Net.Http;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRespository;

namespace ParkyWeb.Repository
{
    public class NationalParkRepository : ParkyWeb.Repository.Respository<NationalPark>, INationalParkRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public NationalParkRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}