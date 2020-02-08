using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Cicada.EFCore.Shared.Constants
{
    public static class DbSetExtension
    {
        /// <summary>
        ///  Add Or Update, it will search the table by primay key, and use Add or Update Method.
        ///  this method will not call SaveChange method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="data"></param>
        public static void AddOrUpdate<T>(this DbSet<T> dbSet, T data) where T : class
        {
            var context = dbSet.GetContext();
            var ids = context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(x => x.Name);

            List<PropertyInfo> keyFields = new List<PropertyInfo>();
            var t = typeof(T);
            foreach (var propt in t.GetProperties())
            {
                var keyAttr = ids.Contains(propt.Name);
                if (keyAttr)
                {
                    keyFields.Add(propt);
                }
            }
            if (keyFields.Count <= 0)
            {
                throw new Exception($"{t.FullName} does not have a KeyAttribute field. Unable to exec AddOrUpdate call.");
            }

            ParameterExpression parameter = Expression.Parameter(typeof(T));

            Expression lambdaWhere = null;

            foreach (var keyField in keyFields)
            {
                Expression _templambdaWhere = Expression.Equal(Expression.Property(parameter, keyField), Expression.Constant(keyField.GetValue(data, null)));

                if (lambdaWhere == null)
                    lambdaWhere = _templambdaWhere;
                else
                    lambdaWhere = Expression.AndAlso(lambdaWhere, _templambdaWhere);
            }

            T val = dbSet.AsNoTracking().SingleOrDefault(Expression.Lambda<Func<T, bool>>(lambdaWhere, new ParameterExpression[1]
            {
                parameter
            }));

            if (val != null)
            {
                dbSet.Update(data);
                return;
            }
            dbSet.Add(data);
        }


        /// <summary>
        /// 取消跟踪DbContext中所有被跟踪的实体
        /// </summary>
        public static void DetachAll(this DbContext dbContext)
        {
            //循环遍历DbContext中所有被跟踪的实体
            while (true)
            {
                //每次循环获取DbContext中一个被跟踪的实体
                var currentEntry = dbContext.ChangeTracker.Entries().FirstOrDefault();

                //currentEntry不为null，就将其State设置为EntityState.Detached，即取消跟踪该实体
                if (currentEntry != null)
                {
                    //设置实体State为EntityState.Detached，取消跟踪该实体，之后dbContext.ChangeTracker.Entries().Count()的值会减1
                    currentEntry.State = EntityState.Detached;
                }
                //currentEntry为null，表示DbContext中已经没有被跟踪的实体了，则跳出循环
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 取消跟踪DbContext中所有被跟踪的实体
        /// </summary>
        public static void DetachTrack<T>(this DbContext dbContext, Expression<Func<T, bool>> query) where T : class
        {
            var currentEntry = dbContext.ChangeTracker.Entries().Where(f => f.Metadata.Name == typeof(T).ToString()).FirstOrDefault();

            //currentEntry不为null，就将其State设置为EntityState.Detached，即取消跟踪该实体
            if (currentEntry != null)
            {
                //设置实体State为EntityState.Detached，取消跟踪该实体，之后dbContext.ChangeTracker.Entries().Count()的值会减1
                currentEntry.State = EntityState.Detached;
            }
        }
    }

    internal static class HackyDbSetGetContextTrick
    {
        public static DbContext GetContext<TEntity>(this DbSet<TEntity> dbSet)
            where TEntity : class
        {
            return (DbContext)dbSet
                .GetType().GetTypeInfo()
                .GetField("_context", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(dbSet);
        }
    }
}
