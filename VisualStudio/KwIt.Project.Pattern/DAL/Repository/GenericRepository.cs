using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Context;
using KwIt.Project.Pattern.DAL.Library;
using KwIt.Project.Pattern.DAL.Models;

namespace KwIt.Project.Pattern.DAL.Repository
{
    public class GenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected virtual BaseContext Context { get; set; }
        protected DbSet<TEntity> DbSet;

        public GenericRepository(BaseContext context)
        {
            this.Context = context;
            this.DbSet = this.Context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                            string includeProperties = "")
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query.Include(includeProperty);
            }

            return (orderBy != null) ? orderBy(query).ToList() : query.ToList();
        }

        public virtual TEntity GetById(int id)
        {
            // do not use find
            //return DbSet.Find(id);
            return GetByExpression(e => e.Id == id);
        }
        public virtual TEntity GetById(int id, string includeProperties)
        {
            TEntity entity = this.GetById(id);

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (!includeProperty.Contains("."))
                {
                    DbMemberEntry member = Context.Entry(entity).Member(includeProperty);

                    if (member is DbCollectionEntry)
                    {
                        this.Context.Entry<TEntity>(entity).Collection(includeProperty).Load();
                    }

                    if (member is DbReferenceEntry)
                    {
                        this.Context.Entry<TEntity>(entity).Reference(includeProperty).Load();
                    }
                }
            }
            return entity;
        }
        public virtual TEntity GetById(int id, List<IncludeProperty> includeProperties)
        {
            TEntity entity = this.GetById(id);
            loadIncludeProperties(entity, includeProperties);
            return entity;
        }


        private void loadIncludeProperties(object entity, IEnumerable<IIncludeProperty> includeProperties)
        {
            foreach (var includeProperty in includeProperties)
            {
                DbMemberEntry member = Context.Entry(entity).Member(includeProperty.Name);

                if (member is DbCollectionEntry)
                {
                    this.Context.Entry(entity).Collection(includeProperty.Name).Load();

                    if (includeProperty.Children != null &&
                        this.Context.Entry(entity).Collection(includeProperty.Name).CurrentValue.GetType().GetInterface(typeof(IEnumerable<>).FullName) != null)
                    {
                        var subCollection =
                            ((IEnumerable<object>)this.Context.Entry(entity).Collection(includeProperty.Name).CurrentValue);

                        foreach (var subElement in subCollection)
                        {
                            loadIncludeProperties(subElement, includeProperty.Children);
                        }
                    }
                }

                if (member is DbReferenceEntry)
                {
                    this.Context.Entry(entity).Reference(includeProperty.Name).Load();
                }
            }
        }

        public virtual IQueryable<TEntity> AsQueryable()
        {
            return DbSet;
        }

        public virtual TEntity GetByExpression(Expression<Func<TEntity, bool>> filter)
        {
            // do not use find
            //return DbSet.Find(id);
            var entity = DbSet.Local.SingleOrDefault(filter.Compile());
            return entity ?? DbSet.SingleOrDefault(filter);
        }



        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Delete(int id)
        {
            Delete(GetById(id));
        }

        public virtual void Delete(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            DbSet.Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Commit(TEntity entity)
        {
            if (entity.Id != default(int))
                Update(entity);
            else
                Insert(entity);
        }

        public virtual TEntity Attach(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                var existingEntity = DbSet.Local.Where(e => e.Id == entity.Id).FirstOrDefault();

                if (existingEntity == null)
                {
                    DbSet.Attach(entity);
                    Context.Entry(entity).State = EntityState.Unchanged;
                }
                else
                {
                    entity = existingEntity;
                }
            }

            return entity;
        }

        public virtual TEntity Detach(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public TEntity GetAttachedModel(TEntity entity)
        {
            TEntity attachedModel = GetById(entity.Id);
            Context.Entry(attachedModel).CurrentValues.SetValues(entity);
            return attachedModel;
        }
    }
}
