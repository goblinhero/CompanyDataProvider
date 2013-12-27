using System;
using System.Collections.Generic;
using System.Text;
using LumenWorks.Framework.IO.Csv;
using NHibernate;
using SuperSchnell.CompanyDataProvider.Domain;
using SuperSchnell.CompanyDataProvider.EntityUpdaters;

namespace SuperSchnell.CompanyDataProvider.Importer
{
    public class DanishCompanyCreator
    {
        private readonly CsvReader _csvReader;
        private readonly Factory _factory = new Factory();
        public DanishCompanyCreator(CsvReader csvReader)
        {
            _csvReader = csvReader;
        }

        public DanishCompany CreateNew()
        {
            var entity =_factory.CreateDanishCompany();
            entity.CVRNumber = _csvReader["cvrnr"];
            entity.CompanyName = _csvReader["navn_tekst"];
            entity.Phone = _csvReader["telefonnummer_kontaktoplysning"];
            entity.Email = _csvReader["email_kontaktoplysning"];
            entity.Address = CreateAddress();
            return entity;
        }
        private Address CreateAddress()
        {
            return _factory.CreateAddress(CreateStreet(), _csvReader["beliggenhedsadresse_postnr"], _csvReader["beliggenhedsadresse_postdistrikt"], _csvReader["beliggenhedsadresse_bynavn"], _csvReader["beliggenhedsadresse_coNavn"]);
        }

        private string CreateStreet()
        {
            return string.Format("{0} {1}", _csvReader["beliggenhedsadresse_vejnavn"], GetRoadNumber());
        }

        private object GetRoadNumber()
        {
            var builder = new StringBuilder(string.Format("{0}{1}",_csvReader["beliggenhedsadresse_husnummerFra"], _csvReader["beliggenhedsadresse_bogstavFra"]));
            if (!string.IsNullOrEmpty(_csvReader["beliggenhedsadresse_husnummerTil"]) || !string.IsNullOrEmpty(_csvReader["beliggenhedsadresse_bogstavFra"]))
                builder.AppendFormat("-{0}{1}", _csvReader["beliggenhedsadresse_husnummerTil"], _csvReader["beliggenhedsadresse_bogstavTil"]);
            return builder.ToString();
        }
    }
}