using System.Runtime.Serialization;

namespace SuperSchnell.CompanyDataProvider.Contracts
{
    [DataContract]
    public class ListOptions    
    {
        [DataMember]
        public int Page { get; set; }
        [DataMember]
        public int PageSize { get; set; }
    }
}