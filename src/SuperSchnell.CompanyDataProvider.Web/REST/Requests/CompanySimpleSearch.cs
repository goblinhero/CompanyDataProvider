using ServiceStack;
using SuperSchnell.CompanyDataProvider.Contracts;
using SuperSchnell.CompanyDataProvider.Helpers;

namespace SuperSchnell.CompanyDataProvider.Web.REST.Requests
{
    [Route("/Company/SimpleSearch")]
    public class CompanySimpleSearch
    {
        public CompanySimpleSearch()
        {
            PageSize = 20;
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public ListOptions ListOptions
        {
            get { return new ListOptions { Page = Page, PageSize = PageSize }; }
        }
    }
}