using System.Runtime.Serialization;

namespace SuperSchnell.CompanyDataProvider.Contracts.Dto
{
    [DataContract]
    public class DanishCompanyDto : EntityDto
    {
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public string CVRNumber { get; set; }
        [DataMember]
        public AddressDto Address { get; set; }
    }
}