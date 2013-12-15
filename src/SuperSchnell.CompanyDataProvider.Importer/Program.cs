using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SuperSchnell.CompanyDataProvider.Importer.Commands;

namespace SuperSchnell.CompanyDataProvider.Importer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Count() < 2 || args.Any(a => !a.Contains(":")))
            {
                Console.WriteLine("Syntax is: SuperSchnell.CompanyProvider.Importer.exe filename:<filename> encoding:<encoding>");
                Console.ReadKey();
                return;
            }
            var splits = args.Select(a => a.Split(':')).ToDictionary(a => a[0], a => a[1]);
            const string fileNameKey = "filename";
            const string encodingKey = "encoding";
            if (!splits.ContainsKey(fileNameKey) || !File.Exists(splits[fileNameKey]))
            {
                Console.WriteLine("You must supply an existing filename as the following syntax: filename:<filename>");
                Console.ReadKey();
                return;
            }
            if (!splits.ContainsKey(encodingKey))
            {
                Console.WriteLine("You must supply an existing filename as the following syntax: filename:<filename>");
                Console.ReadKey();
                return;
            }
            var filename = splits[fileNameKey];
            var encoding = splits[encodingKey];
            var sessionHelper = new SessionHelper();
            sessionHelper.WrapDelete(new TruncateDanishCompaniesCommand());
            new DanishCompanyImporter(filename, encoding,sessionHelper).Execute();
        }
    }
}
