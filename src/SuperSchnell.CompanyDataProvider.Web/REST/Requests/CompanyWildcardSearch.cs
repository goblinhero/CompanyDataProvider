using ServiceStack;
using SuperSchnell.CompanyDataProvider.Web.REST.Requests.Abstract;

namespace SuperSchnell.CompanyDataProvider.Web.REST.Requests
{
    [Route("/Company/WildcardSearch")]
    public class CompanyWildcardSearch : BasePagedRequest
    {
        public string QueryString { get; set; }
    }
}