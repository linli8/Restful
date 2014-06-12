using System;
using Castle.DynamicProxy;

namespace Restful.Data
{
    public class EntityInterceptor : IInterceptor
    {
        public void Intercept( IInvocation invocation )
        {
            var @object = (IEntityObject)invocation.InvocationTarget;

            @object.OnPropertyChanged( invocation.Method.Name.Replace( "set_", "" ) );

            invocation.Proceed();
        }
    }
}

