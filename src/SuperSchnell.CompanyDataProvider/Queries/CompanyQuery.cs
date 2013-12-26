using NHibernate;
using SuperSchnell.CompanyDataProvider.Contracts;
using SuperSchnell.CompanyDataProvider.Contracts.Dto;
using SuperSchnell.CompanyDataProvider.Helpers;
using SuperSchnell.CompanyDataProvider.Queries.Abstract;

namespace SuperSchnell.CompanyDataProvider.Queries
{
    public class CompanyQuery : IPagedSimpleQuery<DanishCompanyDto>
    {
        private readonly ListOptions _listOptions;

        public CompanyQuery(ListOptions listOptions)
        {
            _listOptions = listOptions;
        }

        public void Execute(ISession session)
        {
            Result = session.QueryOver<DanishCompanyDto>().ToPagedSet(_listOptions);
        }

        public PagedResultSet<DanishCompanyDto> Result { get; private set; }
    }
}