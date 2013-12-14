using NHibernate.Search;

namespace SuperSchnell.CompanyDataProvider
{
    public interface IFullQuery
    {
        void Execute(IFullTextSession session);
    }
}