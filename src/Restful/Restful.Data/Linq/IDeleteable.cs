using System;
using System.Linq.Expressions;
using Restful.Data.Entity;

namespace Restful.Data.Linq
{
    public interface IDeleteable<T> : IExecuteable
    {
        /// <summary>
        /// 设置过滤条件
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        IDeleteable<T> Where( Expression<Func<T,bool>> func );
    }
}
