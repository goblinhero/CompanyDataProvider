using System;
using System.Collections.Generic;
using LumenWorks.Framework.IO.Csv;
using NHibernate;
using SuperSchnell.CompanyDataProvider.Domain;
using SuperSchnell.CompanyDataProvider.EntityUpdaters;

namespace SuperSchnell.CompanyDataProvider.Importer
{
    public class DanishCompanyCreator:IEntityCreator<DanishCompany>
    {
        private readonly CsvReader _csvReader;
        private Factory _factory = new Factory();
        public DanishCompanyCreator(CsvReader csvReader)
        {
            _csvReader = csvReader;
        }

        public bool TryCreateNew(ISession session, out DanishCompany entity, out IEnumerable<string> errors)
        {
            entity = _factory.CreateDanishCompany();
            entity.CVRNumber = _csvReader["cvrnr"];
            entity.CompanyName = _csvReader["navn_tekst"];
            entity.Address = CreateAddress();
            errors = new String[0];
            return true;
        }

        private Address CreateAddress()
        {
            return _factory.CreateAddress(CreateStreet(), _csvReader["beliggenhedsadresse_postnr"], _csvReader["beliggenhedsadresse_postdistrikt"]);
        }

        private string CreateStreet()
        {
            if (string.IsNullOrEmpty(_csvReader["beliggenhedsadresse_husnummerTil"]))
                return string.Format("{0} {1}{2}", _csvReader["beliggenhedsadresse_vejnavn"], _csvReader["beliggenhedsadresse_husnummerFra"], _csvReader["beliggenhedsadresse_bogstavFra"]);
            return string.Format("{0} {1}-{2}{3}", _csvReader["beliggenhedsadresse_vejnavn"], _csvReader["beliggenhedsadresse_husnummerFra"], _csvReader["beliggenhedsadresse_husnummerTil"], _csvReader["beliggenhedsadresse_bogstavFra"]);
        }
    }
}