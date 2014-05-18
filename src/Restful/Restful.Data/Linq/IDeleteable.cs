using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Restful.Data.Entity;

namespace Restful.Data.Linq
{
    public interface IDeleteable<T> : IExecuteable where T : EntityObject
    {
        /// <summary>
        /// 设置过滤条件
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        IDeleteable<T> Where( Expression<Func<T,bool>> func );
    }
}
