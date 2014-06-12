using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace Restful.Data
{
    public class EntityInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors( Type type, MethodInfo method, IInterceptor[] interceptors )
        {
            if( method.Name.StartsWith( "set_" ) )
                return interceptors;

            return null;
        }
    }
}

