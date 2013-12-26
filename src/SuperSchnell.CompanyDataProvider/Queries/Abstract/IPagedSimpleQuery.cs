using SuperSchnell.CompanyDataProvider.Contracts;

namespace SuperSchnell.CompanyDataProvider.Queries.Abstract
{
    public interface IPagedSimpleQuery<T> : ISimpleQuery
    {
        PagedResultSet<T> Result { get; }
    }
}