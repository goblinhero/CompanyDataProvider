using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Index;
using Lucene.Net.Search;
using NHibernate.Search;
using SuperSchnell.CompanyDataProvider.Contracts;
using SuperSchnell.CompanyDataProvider.Contracts.Dto;
using SuperSchnell.CompanyDataProvider.Domain;
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
            var luceneQuery = session.CreateFullTextQuery(CreateQuery(), typeof (DanishCompany));
            var totalResultSet = luceneQuery.SetProjection(ProjectionConstants.ID).List<object[]>().SelectMany(r => r).OfType<long>().ToArray();
            var idsToFind = totalResultSet.Skip(_listOptions.Page*_listOptions.PageSize).Take(_listOptions.PageSize).ToArray();
            Result = new PagedResultSet<DanishCompanyDto>(totalResultSet.Length, session.QueryOver<DanishCompanyDto>().WhereRestrictionOn(dc => dc.Id).IsIn(idsToFind).List());
        }

        public PagedResultSet<DanishCompanyDto> Result { get; private set; }

        private Query CreateQuery()
        {
            var queryList = new List<Query>();
            if (!string.IsNullOrEmpty(_proxy.CVRNumber))
                queryList.Add(new PrefixQuery(new Term("CVRNumber", _proxy.CVRNumber)));
            if (!string.IsNullOrEmpty(_proxy.CompanyName))
                queryList.Add(new PrefixQuery(new Term("CompanyName", _proxy.CompanyName)));
            if (!string.IsNullOrEmpty(_proxy.Address.Street))
                queryList.Add(new PrefixQuery(new Term("Address.Street", _proxy.Address.Street)));
            if (!string.IsNullOrEmpty(_proxy.Address.Zip))
                queryList.Add(new PrefixQuery(new Term("Address.Zip", _proxy.Address.Zip)));
            if (!string.IsNullOrEmpty(_proxy.Address.City))
                queryList.Add(new PrefixQuery(new Term("Address.City", _proxy.Address.City)));
            return CreateBooleanQuery(queryList);
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