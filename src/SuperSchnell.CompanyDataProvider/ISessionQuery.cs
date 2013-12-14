using NHibernate;

namespace SuperSchnell.CompanyDataProvider
{
    public interface ISessionQuery
    {
        void Execute(ISession session);
    }
}