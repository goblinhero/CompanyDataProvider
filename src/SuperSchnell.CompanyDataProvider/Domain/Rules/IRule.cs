namespace SuperSchnell.CompanyDataProvider.Domain.Rules
{
    public interface IRule<T>
    {
        bool IsBroken(T obj);
        string Message { get; }
    }
}