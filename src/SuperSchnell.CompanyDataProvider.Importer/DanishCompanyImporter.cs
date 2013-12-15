using System.Collections.Generic;
using System.IO;
using System.Text;
using LumenWorks.Framework.IO.Csv;

namespace SuperSchnell.CompanyDataProvider.Importer
{
    public class DanishCompanyImporter
    {
        private readonly string _filename;
        private readonly string _encoding;
        private readonly SessionHelper _sessionHelper;

        public DanishCompanyImporter(string filename, string encoding, SessionHelper sessionHelper)
        {
            _filename = filename;
            _encoding = encoding;
            _sessionHelper = sessionHelper;
        }

        public void Execute()
        {
            using (var reader = new StreamReader(_filename, Encoding.GetEncoding(_encoding)))
            {
                var csvReader = new CsvReader(reader, true, ',');
                IEnumerable<string> errors;
                while (csvReader.ReadNextRecord())
                {
                    _sessionHelper.WrapCreate(new DanishCompanyCreator(csvReader),out errors);
                }
            }
        }
    }
}