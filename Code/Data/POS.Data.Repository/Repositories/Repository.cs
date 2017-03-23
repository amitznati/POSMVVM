
using POS.Data.Repository.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace POS.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        public Repository(DbContext context)
        {
            Context = context;
        }

        public TEntity Get(int id)
        {
            // Here we are working with a DbContext, not PlutoContext. So we don't have DbSets 
            // such as Courses or Authors, and we need to use the generic Set() method to access them.
            return Context.Set<TEntity>().Find(id);
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
            return Context.Set<TEntity>().ToList();
        }

        public List<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            List<TEntity> all = this.GetAll().ToList();
            if(all.Count>0)
            return Context.Set<TEntity>().Where(predicate).ToList<TEntity>();
            return all;
        }
        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().SingleOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }




        public List<TEntity> GetRange(int index, int count)
        {
            try
            {
                return Context.Set<TEntity>().OrderBy(QueryableExtention.GetKeyField(typeof(TEntity))).Skip(index).Take(count).ToList();
            }
            catch(Exception ex)
            {
                throw ex;

            }
        }

        //public static string GetKeyField(Type type)
        //{
        //    var allProperties = type.GetProperties();

        //    var keyProperty = allProperties.SingleOrDefault(p => p.IsDefined(typeof(KeyAttribute), true));

        //    return keyProperty != null ? keyProperty.Name : null;
        //}

        //public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string orderBy)
        //{
        //    return GetOrderByQuery(source,orderBy, "OrderBy");
        //}

        //public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string orderBy)
        //{
        //    return GetOrderByQuery(source,orderBy, "OrderByDescending");
        //}

        //private static IQueryable<T> GetOrderByQuery<T>(this IQueryable<T> source, string orderBy, string methodName)
        //{
        //    var sourceType = typeof(T);
        //    var property = sourceType.GetProperty(orderBy);
        //    var parameterExpression = Expression.Parameter(sourceType, "x");
        //    var getPropertyExpression = Expression.MakeMemberAccess(parameterExpression, property);
        //    var orderByExpression = Expression.Lambda(getPropertyExpression, parameterExpression);
        //    var resultExpression = Expression.Call(typeof(Queryable), methodName,
        //                                           new[] { sourceType, property.PropertyType }, source.Expression,
        //                                           orderByExpression);
            
        //    return source.Provider.CreateQuery<T>(resultExpression);
        //}

    }
}