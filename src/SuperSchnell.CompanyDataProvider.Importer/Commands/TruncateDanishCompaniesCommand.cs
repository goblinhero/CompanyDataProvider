using NHibernate;
using NHibernate.Search;
using SuperSchnell.CompanyDataProvider.Domain;
using SuperSchnell.CompanyDataProvider.Queries.Abstract;

namespace SuperSchnell.CompanyDataProvider.Importer.Commands
{
    public class TruncateDanishCompaniesCommand : IDeleteCommand
    {
        public bool TryExecute(IFullTextSession session)
        {
            session.CreateSQLQuery("TRUNCATE TABLE DanishCompany").ExecuteUpdate();
            session.PurgeAll(typeof(DanishCompany));
            return true;
        }

    }
}