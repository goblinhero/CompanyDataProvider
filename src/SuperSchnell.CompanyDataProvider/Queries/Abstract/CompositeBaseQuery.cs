using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Util;
using NHibernate;
using NHibernate.Search;
using SuperSchnell.CompanyDataProvider.Contracts;
using SuperSchnell.CompanyDataProvider.Contracts.Dto;
using SuperSchnell.CompanyDataProvider.Domain.Abstract;
using SuperSchnell.CompanyDataProvider.Helpers;

namespace SuperSchnell.CompanyDataProvider.Queries.Abstract
{
    public abstract class CompositeBaseQuery<TEntity, TReturnType> : IPagedFullQuery<TReturnType>
        where TEntity : IHasId
        where TReturnType : EntityDto
    {
        private readonly ListOptions _listOptions;
        protected readonly IEnumerable<string> _searchTerms;

        protected CompositeBaseQuery(string searchTerm, ListOptions listOptions)
        {
            _listOptions = listOptions;
            _searchTerms = searchTerm.TrimFreeTextQueryString();
        }

        public void Execute(IFullTextSession session)
        {
            Result = GetPagedResults(session, _listOptions);
        }

        public PagedResultSet<TReturnType> Result { get; set; }

        protected abstract IEnumerable<Query> CreateQueries();

        protected Query CreateBooleanQuery(Query query, IEnumerable<Query> usedQueries)
        {
            var boolQuery = new BooleanQuery();
            boolQuery.Add(query, BooleanClause.Occur.MUST);
            foreach (var usedQuery in usedQueries)
            {
                boolQuery.Add(usedQuery, BooleanClause.Occur.MUST_NOT);
            }
            return boolQuery;
        }

        protected Query CreateGeneralWildcardSearch(params string[] properties)
        {
            var wildCardSearch = new MultiFieldQueryParser(Version.LUCENE_29,
                properties,
                new StandardAnalyzer(Version.LUCENE_29));
            wildCardSearch.SetDefaultOperator(QueryParser.Operator.AND);
            string wildcardSearch = string.Join("* ", _searchTerms) + '*';
            return wildCardSearch.Parse(wildcardSearch);
        }

        private PagedResultSet<TReturnType> GetPagedResults(IFullTextSession session, ListOptions options)
        {
            var fullResultIds = GetFullResultIds(session);
            var idsToFind = fullResultIds.Skip(options.Page*options.PageSize).Take(options.PageSize).ToArray();
            var entities = session.QueryOver<TReturnType>()
                .WhereRestrictionOn(tr => tr.Id).IsIn(idsToFind).List().ToDictionary(p => p.Id.Value, p => p);
            var idsFound = idsToFind.Intersect(entities.Keys);
            return new PagedResultSet<TReturnType>(fullResultIds.Length, idsFound.Select(id => entities[id]).ToList());
        }

        public IQueryOver<TReturnType, TReturnType> Query(IFullTextSession session)
        {
            var idsToFind = GetFullResultIds(session).ToArray();
            return session.QueryOver<TReturnType>()
                .WhereRestrictionOn(t => t.Id).IsIn(idsToFind);
        }

        protected virtual IList<long> FindInitialResults(IFullTextSession session)
        {
            return new List<long>();
        }

        private long[] GetFullResultIds(IFullTextSession session)
        {
            var result = FindInitialResults(session).ToList();
            var queries = CreateQueries().ToList();
            var usedQueries = new List<Query>();
            foreach (var query in queries)
            {
                var executingQuery = CreateBooleanQuery(query, usedQueries);
                var luceneQuery = session.CreateFullTextQuery(executingQuery, typeof (TEntity));
                var entitiesToAdd =
                    luceneQuery.SetProjection(ProjectionConstants.ID).List<object[]>();
                var newUniques = entitiesToAdd.SelectMany(r => r).OfType<long>().Except(result);
                result.AddRange(newUniques);
                usedQueries.Add(query);
            }
            return result.ToArray();
        }
    }
}