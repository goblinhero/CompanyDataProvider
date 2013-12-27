using NHibernate.Search.Attributes;

namespace SuperSchnell.CompanyDataProvider.Domain
{
    public class Address
    {
        protected Address()
        {
        }

        public Address(string street, string zip, string city, string placeName, string coName)
        {
            CoName = coName;
            Street = street ?? string.Empty;
            Zip = zip??string.Empty;
            City = city??string.Empty;
            PlaceName = placeName ?? string.Empty;
        }

        [Field]
        public string CoName { get; private set; }
        [Field]
        public string Street { get; private set; }
        [Field]
        public string Zip { get; private set; }
        [Field]
        public string City { get; private set; }
        [Field]
        public string PlaceName { get; private set; }
    }
}