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
            return new Address(string.Empty,string.Empty,string.Empty);
        }
        private Address CreateAddress(string street, string zip, string city)
        {
            return new Address(street,zip,city);
        }
    }
}