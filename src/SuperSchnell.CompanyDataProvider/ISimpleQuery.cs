using NHibernate;
using SuperSchnell.CompanyDataProvider.Contracts;

namespace SuperSchnell.CompanyDataProvider
{
    public interface ISimpleQuery
    {
        void Execute(ISession session);
    }

    public interface IPagedSimpleQuery<T> : ISimpleQuery
    {
        PagedResultSet<T> Result { get; }
    }

    public interface IDeleteCommand
    {
        bool TryExecute(ISession session);
    }
}