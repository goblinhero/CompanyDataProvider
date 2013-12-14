using System.Collections.Generic;

namespace SuperSchnell.CompanyDataProvider.Domain
{
    public interface IEntity
    {
        bool IsValid(out IEnumerable<string> validationErrors);
        int Version { get; }
    }
}