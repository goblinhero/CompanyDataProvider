using NHibernate;

namespace SuperSchnell.CompanyDataProvider.Importer.Commands
{
    public class TruncateDanishCompaniesCommand : IDeleteCommand
    {
        public bool TryExecute(ISession session)
        {
            session.CreateSQLQuery("TRUNCATE TABLE DanishCompany").ExecuteUpdate();
            return true;
        }
    }
}