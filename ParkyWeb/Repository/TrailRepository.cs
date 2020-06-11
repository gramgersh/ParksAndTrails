using System.Net.Http;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRespository;

namespace ParkyWeb.Repository
{
    public class TrailRepository : ParkyWeb.Repository.Respository<Trail>, ITrailRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public TrailRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}