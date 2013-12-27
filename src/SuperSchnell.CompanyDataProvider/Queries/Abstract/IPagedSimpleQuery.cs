using SuperSchnell.CompanyDataProvider.Contracts;

namespace SuperSchnell.CompanyDataProvider.Queries.Abstract
{
    public interface IPagedSimpleQuery<T> : ISimpleQuery
    {
        PagedResultSet<T> Result { get; }
    }
    public interface IPagedFullQuery<T> : IFullQuery
    {
        PagedResultSet<T> Result { get; }
    }
}