using System;
using System.Collections.Generic;
using System.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Event;
using NHibernate.Linq;
using NHibernate.Search;
using NHibernate.Search.Event;
using SuperSchnell.CompanyDataProvider.Domain;
using SuperSchnell.CompanyDataProvider.Domain.Abstract;
using SuperSchnell.CompanyDataProvider.EntityUpdaters.Abstract;
using SuperSchnell.CompanyDataProvider.Mappings;
using SuperSchnell.CompanyDataProvider.Queries.Abstract;

namespace SuperSchnell.CompanyDataProvider
{
    public class SessionHelper
    {
        private static readonly ISessionFactory _sessionFactory;

        static SessionHelper()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["CompanyDatabase"];
            var configuration = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                    .ConnectionString(connectionString.ConnectionString)
                    .UseOuterJoin())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<DanishCompanyMap>()
                    .Conventions.Add(ForeignKey.EndsWith("Id")))
                .BuildConfiguration();
            configuration.SetListener(ListenerType.PostUpdate, new FullTextIndexEventListener());
            configuration.SetListener(ListenerType.PostInsert, new FullTextIndexEventListener());
            configuration.SetProperty("adonet.batch_size", "100");
            _sessionFactory = configuration.BuildSessionFactory();
        }

        public void WrapQuery(ISimpleQuery query)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                query.Execute(session);
                tx.Commit();
            }
        }

        public void WrapQuery(IFullQuery query)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var fullTextSession = Search.CreateFullTextSession(session);
                using (var tx = fullTextSession.BeginTransaction())
                {
                    query.Execute(fullTextSession);
                    tx.Commit();
                }
            }
        }

        public void BulkCreate<TEntity>(IEnumerable<TEntity> entities)
        {
            using (var session = _sessionFactory.OpenStatelessSession())
            using (var tx = session.BeginTransaction())
            {
                entities.ForEach(e => session.Insert(e));
                tx.Commit();
            }
        }

        public bool WrapCreate<TEntity>(IEntityCreator<TEntity> updater, out IEnumerable<string> errors)
            where TEntity : IEntity
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                TEntity entity;
                if (!updater.TryCreateNew(session, out entity, out errors) || !entity.IsValid(out errors))
                {
                    tx.Rollback();
                    return false;
                }
                session.Save(entity);
                tx.Commit();
                return true;
            }
        }

        public bool WrapUpdate<TEntity>(IEntityUpdater<TEntity> updater, out IEnumerable<string> errors)
            where TEntity : IEntity
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var entity = session.Get<TEntity>(updater.Id);
                if (entity == null)
                {
                    errors = new[] {string.Format(Errors.Domain_Save_Entity_Not_Found, updater.Id)};
                    tx.Rollback();
                    return false;
                }
                if (entity.Version != updater.Version)
                {
                    errors = new[] {string.Format("{0}{1}{2}", Errors.Domain_Save_Wrong_Version, entity.Version, updater.Version)};
                    tx.Rollback();
                    return false;
                }
                if (!updater.TryUpdate(session, ref entity, out errors) || !entity.IsValid(out errors))
                {
                    tx.Rollback();
                    return false;
                }
                tx.Commit();
                return true;
            }
        }

        public void WrapDelete(IDeleteCommand deleteCommand)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var fullTextSession = Search.CreateFullTextSession(session);
                using (var tx = fullTextSession.BeginTransaction())
                {
                    if (deleteCommand.TryExecute(fullTextSession))
                    {
                        tx.Commit();
                    }
                    else
                    {
                        tx.Rollback();
                    }
                }
            }
        }

        public void ReIndex<TEntity>() where TEntity : class
        {
            var purgeQuery = new PurgeAndCountCompanyQuery<DanishCompany>();
            WrapQuery(purgeQuery);
            int count = purgeQuery.Count;
            var currentPage = 0;
            const int pageSize = 1000;
            while (currentPage*pageSize < count)
            {
                WrapQuery(new PagedReIndexQuery<DanishCompany>(currentPage, pageSize));
                currentPage++;
                Console.WriteLine("Reindexed {0} companies", currentPage*pageSize);
            }
        }
    }

    public class PagedReIndexQuery<TEntity> : IFullQuery
        where TEntity : class
    {
        private readonly int _currentPage;
        private readonly int _pageSize;

        public PagedReIndexQuery(int currentPage, int pageSize)
        {
            _currentPage = currentPage;
            _pageSize = pageSize;
        }

        public void Execute(IFullTextSession session)
        {
            var entities = session.QueryOver<TEntity>()
                .Skip(_currentPage*_pageSize)
                .Take(_pageSize)
                .List();
            foreach (var entity in entities)
            {
                session.Index(entity);
            }
        }
    }

    public class PurgeAndCountCompanyQuery<TEntity> : IFullQuery
        where TEntity : class
    {
        public int Count { get; private set; }

        public void Execute(IFullTextSession session)
        {
            session.PurgeAll(typeof (TEntity));
            Count = session.QueryOver<TEntity>()
                .ToRowCountQuery()
                .SingleOrDefault<int>();
        }
    }
}