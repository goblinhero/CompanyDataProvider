using NHibernate.Search;

namespace SuperSchnell.CompanyDataProvider.Queries.Abstract
{
    public interface IDeleteCommand
    {
        bool TryExecute(IFullTextSession session);
    }
}