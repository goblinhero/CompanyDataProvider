using ServiceStack;
using SuperSchnell.CompanyDataProvider.Contracts.Dto;
using SuperSchnell.CompanyDataProvider.Queries;
using SuperSchnell.CompanyDataProvider.Web.REST.Requests;

namespace SuperSchnell.CompanyDataProvider.Web.REST
{
    public class CompanyService : Service
    {
        private readonly SessionHelper _sessionHelper = new SessionHelper();

        public object Any(CompanySimpleSearch search)
        {
            var query = new CompanyQuery(search.ListOptions);
            _sessionHelper.WrapQuery(query);
            return query.Result;
        }

        public object Any(CompanyWildcardSearch search)
        {
            if (string.IsNullOrEmpty(search.QueryString))
            {
                return Any(new CompanySimpleSearch {Page = search.Page, PageSize = search.PageSize});
            }
            var query = new WildcardCompanySearchQuery(search.QueryString.ToLower(), search.ListOptions);
            _sessionHelper.WrapQuery(query);
            return query.Result;
        }
        public object Any(CompanyProxySearch search)
        {
            if (string.IsNullOrWhiteSpace(string.Format("{0}{1}{2}{3}{4}", search.CVRNumber, search.CompanyName, search.Street, search.Zip, search.City)))
            {
                return Any(new CompanySimpleSearch {Page = search.Page, PageSize = search.PageSize});
            }
            var query = new ProxyCompanySearchQuery(new DanishCompanyDto
            {
                CompanyName = search.CompanyName.ToLower(),
                CVRNumber = search.CVRNumber.ToLower(),
                Address = new AddressDto
                {
                    City = search.City.ToLower(),
                    Street = search.Street.ToLower(),
                    Zip = search.Zip.ToLower(),
                }
            }, search.ListOptions);
            _sessionHelper.WrapQuery(query);
            return query.Result;
        }
    }
}