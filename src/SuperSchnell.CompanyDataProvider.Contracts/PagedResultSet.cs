using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SuperSchnell.CompanyDataProvider.Contracts
{
  [DataContractAttribute]
    public class PagedResultSet<T>
    {
        public PagedResultSet(int totalCount, IList<T> results)
        {
            TotalCount = totalCount;
            Results = results;
        }

        [DataMember]
        public int TotalCount { get; private set; }
        [DataMember ]
        public IList<T> Results { get; private set; }
    }
}