using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LumenWorks.Framework.IO.Csv;
using SuperSchnell.CompanyDataProvider.Domain;

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
                int count = 0;
                var sessionHelper = new SessionHelper();

                bool moreRecords = true;
                while (moreRecords)
                {
                    var companyList = new List<DanishCompany>(100);
                    for (int i = 0; i < 100; i++)
                    {
                        if (!csvReader.ReadNextRecord() || csvReader.EndOfStream)
                        {
                            moreRecords = false;
                            break;
                        }
                        companyList.Add(new DanishCompanyCreator(csvReader).CreateNew());
                    }
                    sessionHelper.BulkCreate(companyList);
                    count += companyList.Count;
                    Console.WriteLine("Inserted {0} Danish companies", count);
                }
            }
        }
    }
}