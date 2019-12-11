using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Persistence.Interfaces
{
    public interface IRepository
    {
        IQueryable<TEntity> Any<TEntity>( Expression<Func<TEntity, bool>> expresion ) where TEntity : class;
        Task<int> CountAsync<TEntity>( Expression<Func<TEntity, bool>> expression ) where TEntity : class;
        void Create<TEntity>( TEntity entity, bool autoSaveChanges = false ) where TEntity : class;
        void Delete<TEntity>( TEntity entity, bool autoSaveChanges = false ) where TEntity : class;
        void DeleteById<TEntity>( long id, bool autoSaveChanges = false ) where TEntity : class;
        TEntity FindById<TEntity>( long id ) where TEntity : class;
        void Update<TEntity>( TEntity entity, bool autoSaveChanges ) where TEntity : class;
    }
}