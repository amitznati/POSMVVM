using POS.Data.Repository.Core.Repository;
using POSEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repository.Repositories
{
    public class UnDeleteableRepository<TEntity> : IUnDeleteableRepository<TEntity> where TEntity : class,IUnDeleteableEntity
    {
        protected readonly DbContext Context;
        private DbSet<TEntity> _entities { get; set; }

        public UnDeleteableRepository(DbContext context)
        {
            Context = context;
            _entities = Context.Set<TEntity>();
        }

        public TEntity Get(int id)
        {
            // Here we are working with a DbContext, not PlutoContext. So we don't have DbSets 
            // such as Courses or Authors, and we need to use the generic Set() method to access them.
            var ent = _entities.Find(id);
            if (ent != null && ent.Mode) return ent;
            return null;
        }

        public IEnumerable<TEntity> GetAll()
        {
            // Note that here I've repeated Context.Set<TEntity>() in every method and this is causing
            // too much noise. I could get a reference to the DbSet returned from this method in the 
            // constructor and store it in a private field like _entities. This way, the implementation
            // of our methods would be cleaner:
            // 
            // _entities.ToList();
            // _entities.Where();
            // _entities.SingleOrDefault();
            // 
            // I didn't change it because I wanted the code to look like the videos. But feel free to change
            // this on your own.
            var list = _entities.ToList();
            if (list.Count == 0) return list;
            return list.Where(e=>e.Mode == true).ToList();
        }
        public List<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            var query = _entities.Where(predicate);
            if (query.Count() == 0) return query.ToList();
            return query.Where(e=>e.Mode).ToList();
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            var query = _entities.Where(predicate);
            if (query.Count() == 0) return null;
            return query.Where(e => e.Mode).SingleOrDefault();
        }

        public void Add(TEntity entity)
        {
            _entities.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _entities.AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            _entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _entities.RemoveRange(entities);
        }

        
    }
}
