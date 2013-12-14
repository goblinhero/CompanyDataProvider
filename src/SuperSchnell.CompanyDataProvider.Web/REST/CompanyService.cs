using ServiceStack;
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
    }
}