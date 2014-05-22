using System;
using Castle.Core.Interceptor;

namespace Restful.Data.Entity
{
    internal class EntityInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var @object = (IEntityObject)invocation.InvocationTarget;

            @object.OnPropertyChanged(invocation.Method.Name.Replace("set_", ""));

            invocation.Proceed();
        }
    }
}

