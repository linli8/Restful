using System;
using Castle.Core.Interceptor;
using System.Reflection;

namespace Restful.Data.Entity
{
    internal class EntityInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            if (method.Name.StartsWith("set_")) return interceptors;

            return null;
        }
    }
}

