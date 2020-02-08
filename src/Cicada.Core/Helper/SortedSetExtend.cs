using System;
using System.Collections.Generic;
using System.Linq;

namespace Cicada.Core.Helper
{
    public static class SortedSetExtend
    {
        /// <summary>
        /// 拿出一个，并从原列表删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="data">操作的列表</param>
        /// <returns></returns>
        public static TSource TakeOne<TSource>(this SortedSet<TSource> source, Func<TSource, bool> predicate)
        {
            TSource result = source.FirstOrDefault(predicate);
            if (result != null)
            {
                source.Remove(result);
            }
            return result;
        }

        /// <summary>
        /// 从列表中拿出指定数组，并从原列表删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="data">操作的列表</param>
        /// <returns></returns>
        public static TSource[] TakeMany<TSource>(this SortedSet<TSource> source, Func<TSource, bool> predicate, int maxCount = 10)
        {
            TSource[] result = source.Where(predicate).Take(maxCount)?.ToArray();
            foreach (TSource i in result)
            {
                source.Remove(i);
            }
            return result;
        }
    }
}
