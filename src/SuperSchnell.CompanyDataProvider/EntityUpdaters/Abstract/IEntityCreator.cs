using System.Collections.Generic;
using NHibernate;

namespace SuperSchnell.CompanyDataProvider.EntityUpdaters.Abstract
{
    public interface IEntityCreator<T>
    {
        bool TryCreateNew(ISession session, out T entity, out IEnumerable<string> errors);
    }
}