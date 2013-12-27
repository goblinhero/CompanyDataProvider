using SuperSchnell.CompanyDataProvider.Contracts;

namespace SuperSchnell.CompanyDataProvider.Web.REST.Requests.Abstract
{
    public abstract class BasePagedRequest
    {
        protected BasePagedRequest()
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