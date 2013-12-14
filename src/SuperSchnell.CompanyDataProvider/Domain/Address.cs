namespace SuperSchnell.CompanyDataProvider.Domain
{
    public class Address
    {
        protected Address()
        {
        }

        public Address(string street, string zip, string city)
        {
            Street = street ?? string.Empty;
            Zip = zip??string.Empty;
            City = city??string.Empty;
        }

        public string Street { get; private set; }
        public string Zip { get; private set; }
        public string City { get; private set; }
    }
}