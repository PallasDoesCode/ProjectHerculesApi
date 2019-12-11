using Core.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class EfRepository : IRepository
    {
        private readonly DbContext _dbContext;
        protected EfRepository( DbContext dbContext )
        {
            _dbContext = dbContext;
        }

        #region Public Methods

        public IQueryable<TEntity> Any<TEntity>( Expression<Func<TEntity, bool>> expression ) where TEntity : class
        {
            return _dbContext.Set<TEntity>().Where( expression ).AsNoTracking();
        }

        public Task<int> CountAsync<TEntity>( Expression<Func<TEntity, bool>> expression ) where TEntity : class
        {
            return Any( expression ).CountAsync();
        }

        public void Create<TEntity>( TEntity entity, bool autoSaveChanges = false ) where TEntity : class
        {
            _dbContext.Set<TEntity>().Add( entity );

            if ( autoSaveChanges )
                SaveChanges();
        }

        public void Delete<TEntity>( TEntity entity, bool autoSaveChanges = false ) where TEntity : class
        {
            _dbContext.Set<TEntity>().Remove( entity );

            if ( autoSaveChanges )
                SaveChanges();
        }

        public void DeleteById<TEntity>( long id, bool autoSaveChanges = false ) where TEntity : class
        {
            TEntity entity = FindById<TEntity>( id );
            _dbContext.Set<TEntity>().Remove( entity ); ;

            if ( autoSaveChanges )
                SaveChanges();
        }

        public TEntity FindById<TEntity>( long id ) where TEntity : class
        {
            Expression<Func<TEntity, bool>> predicate = BuildLambdaForFindById<TEntity>( id );
            return Any( predicate ).FirstOrDefault();
        }

        public void Update<TEntity>( TEntity entity, bool autoSaveChanges = false ) where TEntity : class
        {
            _dbContext.Set<TEntity>().Update( entity );

            if ( autoSaveChanges )
                SaveChanges();
        }

        #endregion

        #region Private Methods

        public int SaveChanges()
        {
            int changes;
            try
            {
                changes = _dbContext.SaveChanges();
            }
            catch ( Exception ex )
            {
                throw ex;
            }
            return changes;
        }

        protected static Expression<Func<TEntity, bool>> BuildLambdaForFindById<TEntity>( long id )
        {
            var item = Expression.Parameter( typeof( TEntity ), "entity" );
            var prop = Expression.Property( item, "Id" );
            var value = Expression.Constant( id );
            var equal = Expression.Equal( prop, value );
            var lambda = Expression.Lambda<Func<TEntity, bool>>( equal, item );
            return lambda;
        }

        #endregion
    }
}