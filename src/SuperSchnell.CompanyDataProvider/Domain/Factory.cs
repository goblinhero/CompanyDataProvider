namespace SuperSchnell.CompanyDataProvider.Domain
{
    public class Factory
    {
        public DanishCompany CreateDanishCompany()
        {
            return new DanishCompany
                {
                    Address = CreateBlankAddress(),
                };
        }

        private Address CreateBlankAddress()
        {
            return new Address(string.Empty,string.Empty,string.Empty,string.Empty,string.Empty);
        }

        public Address CreateAddress(string street, string zip, string city, string placeName, string coName)
        {
            return new Address(street,zip,city,placeName,coName);
        }
    }
}