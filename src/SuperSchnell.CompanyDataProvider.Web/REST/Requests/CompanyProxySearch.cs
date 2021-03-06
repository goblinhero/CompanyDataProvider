using ServiceStack;
using SuperSchnell.CompanyDataProvider.Web.REST.Requests.Abstract;

namespace SuperSchnell.CompanyDataProvider.Web.REST.Requests
{
    [Route("/Company/SearchByProxy")]
    public class CompanyProxySearch : BasePagedRequest
    {
        public string CVRNumber { get; set; }
        public string CompanyName { get; set; }
        public string PlaceName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string CoName { get; set; }
    }
}