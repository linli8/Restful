using System;
using Castle.DynamicProxy;
using System.Reflection;

namespace Restful.Data.Entity
{
    internal class EntityInterceptorFilter : IProxyGenerationHook
    {
        public void NonVirtualMemberNotification(Type type, MemberInfo memberInfo)
        {
        }

        public void MethodsInspected()
        {
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo memberInfo)
        {
            return memberInfo.IsSpecialName && (memberInfo.Name.StartsWith("set_") || memberInfo.Name.StartsWith("get_"));
        }
    }
}

