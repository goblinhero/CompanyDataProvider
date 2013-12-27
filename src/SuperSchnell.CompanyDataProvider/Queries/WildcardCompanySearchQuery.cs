using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Search;
using NHibernate.Criterion;
using NHibernate.Search;
using SuperSchnell.CompanyDataProvider.Contracts;
using SuperSchnell.CompanyDataProvider.Contracts.Dto;
using SuperSchnell.CompanyDataProvider.Domain;
using SuperSchnell.CompanyDataProvider.Queries.Abstract;

namespace SuperSchnell.CompanyDataProvider.Queries
{
    public class WildcardCompanySearchQuery : CompositeBaseQuery<DanishCompany, DanishCompanyDto>
    {
        public WildcardCompanySearchQuery(string searchTerm, ListOptions listOptions)
            : base(searchTerm, listOptions)
        {
        }

        protected override IEnumerable<Query> CreateQueries()
        {
            return new[]
            {
                CreateGeneralWildcardSearch("CVRNumber"),
                CreateGeneralWildcardSearch("CompanyName"),
                CreateGeneralWildcardSearch("CVRNumber","CompanyName", "Address.Street","Address.Zip","Address.City","Address.PlaceName","Address.CoName")
            };
        }

        protected override IList<long> FindInitialResults(IFullTextSession session)
        {
            if(_searchTerms.Any() && _searchTerms.Count()==1)
            {
                var crit = Restrictions.Or(Restrictions.Where<DanishCompanyDto>(dc => dc.CompanyName == _searchTerms.First()),
                    Restrictions.Where<DanishCompanyDto>(dc => dc.CVRNumber == _searchTerms.First()));
                return session.QueryOver<DanishCompanyDto>()
                    .Where(crit)
                    .Select(dc => dc.Id)
                    .List<long>();
            }
            return base.FindInitialResults(session);
        }
    }
}