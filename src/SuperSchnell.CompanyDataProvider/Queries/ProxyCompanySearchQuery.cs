using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Util;
using NHibernate.Search;
using SuperSchnell.CompanyDataProvider.Contracts;
using SuperSchnell.CompanyDataProvider.Contracts.Dto;
using SuperSchnell.CompanyDataProvider.Domain;
using SuperSchnell.CompanyDataProvider.Helpers;
using SuperSchnell.CompanyDataProvider.Queries.Abstract;

namespace SuperSchnell.CompanyDataProvider.Queries
{
    public class ProxyCompanySearchQuery : IPagedFullQuery<DanishCompanyDto>
    {
        private readonly ListOptions _listOptions;
        private readonly DanishCompanyDto _proxy;

        public ProxyCompanySearchQuery(DanishCompanyDto proxy, ListOptions listOptions)
        {
            _proxy = proxy;
            _listOptions = listOptions;
        }

        public void Execute(IFullTextSession session)
        {
            var luceneQuery = session.CreateFullTextQuery(CreateQuery(), typeof(DanishCompany));
            var totalResultSet = luceneQuery.SetProjection(ProjectionConstants.ID).List<object[]>().SelectMany(r => r).OfType<long>().ToArray();
            var idsToFind = totalResultSet.Skip(_listOptions.Page * _listOptions.PageSize).Take(_listOptions.PageSize).ToArray();
            Result = new PagedResultSet<DanishCompanyDto>(totalResultSet.Length, session.QueryOver<DanishCompanyDto>().WhereRestrictionOn(dc => dc.Id).IsIn(idsToFind).List());
        }

        public PagedResultSet<DanishCompanyDto> Result { get; private set; }

        private Query CreateQuery()
        {
            var queryList = new List<Query>();
            if (!string.IsNullOrWhiteSpace(_proxy.CVRNumber))
                queryList.Add(CreateWildcardSearch(_proxy.CVRNumber.TrimFreeTextQueryString(),"CVRNumber"));
            if (!string.IsNullOrWhiteSpace(_proxy.CompanyName))
                queryList.Add(CreateWildcardSearch(_proxy.CompanyName.TrimFreeTextQueryString(), "CompanyName"));
            if (!string.IsNullOrWhiteSpace(_proxy.Email))
                queryList.Add(CreateWildcardSearch(_proxy.Email.TrimFreeTextQueryString(), "Email"));
            if (!string.IsNullOrWhiteSpace(_proxy.Phone))
                queryList.Add(CreateWildcardSearch(_proxy.Phone.TrimFreeTextQueryString(), "Phone"));
            if (!string.IsNullOrWhiteSpace(_proxy.Address.Street))
                queryList.Add(CreateWildcardSearch(_proxy.Address.Street.TrimFreeTextQueryString(), "Address.Street"));
            if (!string.IsNullOrWhiteSpace(_proxy.Address.Zip))
                queryList.Add(CreateWildcardSearch(_proxy.Address.Zip.TrimFreeTextQueryString(), "Address.Zip"));
            if (!string.IsNullOrWhiteSpace(_proxy.Address.City))
                queryList.Add(CreateWildcardSearch(_proxy.Address.City.TrimFreeTextQueryString(), "Address.City"));
            if (!string.IsNullOrWhiteSpace(_proxy.Address.CoName))
                queryList.Add(CreateWildcardSearch(_proxy.Address.CoName.TrimFreeTextQueryString(), "Address.CoName"));
            if (!string.IsNullOrWhiteSpace(_proxy.Address.PlaceName))
                queryList.Add(CreateWildcardSearch(_proxy.Address.PlaceName.TrimFreeTextQueryString(), "Address.PlaceName"));
            return CreateBooleanQuery(queryList);
        }
        private Query CreateWildcardSearch(IEnumerable<string> searchTerms, params string[] properties)
        {
            var wildCardSearch = new MultiFieldQueryParser(Version.LUCENE_29, properties, new StandardAnalyzer(Version.LUCENE_29));
            wildCardSearch.SetDefaultOperator(QueryParser.Operator.AND);
            string wildcardSearch = string.Join("* ", searchTerms) + '*';
            return wildCardSearch.Parse(wildcardSearch);
        }

        private Query CreateBooleanQuery(IEnumerable<Query> queries)
        {
            var boolQuery = new BooleanQuery();
            foreach (var query in queries)
            {
                boolQuery.Add(query, BooleanClause.Occur.MUST);
            }
            return boolQuery;
        }
    }
}