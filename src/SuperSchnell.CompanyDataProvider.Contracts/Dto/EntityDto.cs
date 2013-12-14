using System.Runtime.Serialization;

namespace SuperSchnell.CompanyDataProvider.Contracts.Dto
{
    [DataContract]
    public abstract class EntityDto
    {
        [DataMember]
        public long? Id { get; set; }
        [DataMember]
        public int Version { get; set; }
    }
}