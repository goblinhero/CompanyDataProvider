using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SuperSchnell.CompanyDataProvider.Contracts
{
    [DataContractAttribute]
    public class PagedResultSet<T>
    {
        public PagedResultSet()
        {
            Results = new List<T>();
        }
        public PagedResultSet(int totalCount, IList<T> results)
        {
            TotalCount = totalCount;
            Results = results.ToList();
        }

        [DataMember]
        public int TotalCount { get; set; }
        [DataMember]
        public List<T> Results { get; set; }
    }
}