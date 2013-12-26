using NHibernate.Search;

namespace SuperSchnell.CompanyDataProvider.Queries.Abstract
{
    public interface IFullQuery
    {
        void Execute(IFullTextSession session);
    }
}