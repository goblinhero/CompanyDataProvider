using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SuperSchnell.CompanyDataProvider.Domain;
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
            var monitor = new PerfMon();
            var sessionHelper = new SessionHelper();
            monitor.AddMark("SessionHelper initialized");
            sessionHelper.WrapDelete(new TruncateDanishCompaniesCommand());
            monitor.AddMark("DB truncated");
            new DanishCompanyImporter(filename, encoding, sessionHelper).Execute();
            monitor.AddMark("Companies import");
            sessionHelper.ReIndex<DanishCompany>();
            monitor.AddMark("Companies indexing");
            monitor.PrintSummary();
            Console.ReadKey();
        }
    }

    public class PerfMon
    {
        private readonly IList<Tuple<string,DateTime>> _marks = new List<Tuple<string, DateTime>>();

        public PerfMon()
        {
            Start = DateTime.Now;
        }

        public DateTime Start { get; set; }

        public void AddMark(string mark)
        {
            _marks.Add(new Tuple<string, DateTime>(mark, DateTime.Now));
        }

        public void PrintSummary()
        {
            var firstMark = _marks.First();
            foreach (var mark in _marks)
            {
                Console.WriteLine("{0} started at {1:T} and took {2:####} seconds", mark.Item1,mark.Item2,mark.Item2.Subtract(firstMark.Item2).TotalSeconds);
            }
        }
    }
}
