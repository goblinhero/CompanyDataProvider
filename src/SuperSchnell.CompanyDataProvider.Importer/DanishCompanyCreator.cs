using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        private TextInfo _textInfo;
        private readonly Dictionary<string,string> _commonPhrases = new Dictionary<string, string>();

        public DanishCompanyCreator(CsvReader csvReader)
        {
            _csvReader = csvReader;
            _textInfo = new CultureInfo("da-DK").TextInfo;
            SetupCommonPhrases();
        }

        private void SetupCommonPhrases()
        {
            _commonPhrases.Add("aps","ApS");
            _commonPhrases.Add("af","af");
            _commonPhrases.Add("og","og");
            _commonPhrases.Add("en","en");
        }

        public DanishCompany CreateNew()
        {
            var entity =_factory.CreateDanishCompany();
            entity.CVRNumber = _csvReader["cvrnr"];
            entity.CompanyName = CleanString(_csvReader["navn_tekst"]);
            entity.Phone = _csvReader["telefonnummer_kontaktoplysning"];
            entity.Email = _csvReader["email_kontaktoplysning"];
            entity.Address = CreateAddress();
            return entity;
        }
        private Address CreateAddress()
        {
            return _factory.CreateAddress(CreateStreet(), _csvReader["beliggenhedsadresse_postnr"],CleanString(_csvReader["beliggenhedsadresse_postdistrikt"]),CleanString(_csvReader["beliggenhedsadresse_bynavn"]),CleanString(_csvReader["beliggenhedsadresse_coNavn"]));
        }

        private string CreateStreet()
        {
            return string.Format("{0} {1}", CleanString(_csvReader["beliggenhedsadresse_vejnavn"]), GetRoadNumber());
        }

        private object GetRoadNumber()
        {
            var builder = new StringBuilder(string.Format("{0}{1}",_csvReader["beliggenhedsadresse_husnummerFra"], _csvReader["beliggenhedsadresse_bogstavFra"]));
            if (!string.IsNullOrEmpty(_csvReader["beliggenhedsadresse_husnummerTil"]) || !string.IsNullOrEmpty(_csvReader["beliggenhedsadresse_bogstavFra"]))
                builder.AppendFormat("-{0}{1}", _csvReader["beliggenhedsadresse_husnummerTil"], _csvReader["beliggenhedsadresse_bogstavTil"]);
            return CleanString(builder.ToString());
        }

        private string CleanString(string convert)
        {
            return string.Join(" ", convert.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries).Select(s => GetProperCase(s.ToLower())));
        }

        private string GetProperCase(string convert)
        {
            return _commonPhrases.ContainsKey(convert) ? _commonPhrases[convert] : _textInfo.ToTitleCase(convert);
        }
    }
}