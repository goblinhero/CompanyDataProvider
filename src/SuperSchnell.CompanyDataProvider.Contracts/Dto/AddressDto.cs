using System.Runtime.Serialization;

namespace SuperSchnell.CompanyDataProvider.Contracts.Dto
{
    [DataContract]
    public class AddressDto
    {
        [DataMember]
        public string CoName { get; set; }
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string Zip { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string PlaceName { get; set; }

    }
}