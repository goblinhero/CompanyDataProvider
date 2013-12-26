using NHibernate;

namespace SuperSchnell.CompanyDataProvider.Queries.Abstract
{
    public interface ISimpleQuery
    {
        void Execute(ISession session);
    }
}