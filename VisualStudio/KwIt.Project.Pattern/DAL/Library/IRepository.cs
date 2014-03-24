using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace KwIt.Project.Pattern.DAL.Library
{
    /// <summary>
    /// Interface for all Repositories
    /// </summary>
    /// <typeparam name="TEntity">IBaseEntity</typeparam>
    public interface IRepository<TEntity>
        where TEntity : BaseEntity
    {
        IQueryable<TEntity> AsQueryable();

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                            string includeProperties = "");

        TEntity GetById(int id);
        TEntity GetById(int id, string includeProperties);


        TEntity GetByExpression(Expression<Func<TEntity, bool>> filter);

        void Insert(TEntity entity);
        void Delete(int id);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        void Commit(TEntity entity);

        TEntity Attach(TEntity entity);
        TEntity Detach(TEntity entity);
        TEntity GetAttachedModel(TEntity entity);
    }
}
