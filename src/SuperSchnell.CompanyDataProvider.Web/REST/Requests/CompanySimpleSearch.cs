using ServiceStack;
using SuperSchnell.CompanyDataProvider.Helpers;
using SuperSchnell.CompanyDataProvider.Web.REST.Requests.Abstract;

namespace SuperSchnell.CompanyDataProvider.Web.REST.Requests
{
    [Route("/Company/SimpleSearch")]
    public class CompanySimpleSearch : BasePagedRequest
    {
    }
}