using System;
using Castle.DynamicProxy.Generators;
using System.Collections.Generic;
using System.Diagnostics;

namespace Restful.Data
{
    public class EntityProxyNameScope : INamingScope
    {
        private const string CASTLE_NAMESPACE = "Castle.Proxies.";
        private const string RESTFUL_NAMESPACE = "Restful.Data.Entity.Proxies.";

        private readonly IDictionary<string, int> names = new Dictionary<string, int>();
        private readonly INamingScope parentScope;

        public INamingScope ParentScope
        {
            get
            {
                return this.parentScope;
            }
        }

        public EntityProxyNameScope()
        {
        }

        private EntityProxyNameScope( INamingScope parent )
        {
            this.parentScope = parent;
        }

        public string GetUniqueName( string suggestedName )
        {
            Debug.Assert( !string.IsNullOrEmpty( suggestedName ), "string.IsNullOrEmpty(suggestedName) == false" );

            suggestedName = suggestedName.Replace( CASTLE_NAMESPACE, RESTFUL_NAMESPACE );

            int counter;

            if( !this.names.TryGetValue( suggestedName, out counter ) )
            {
                this.names.Add( suggestedName, 0 );

                return suggestedName;
            }
            else
            {
                counter++;

                this.names[suggestedName] = counter;

                return suggestedName + "_" + counter.ToString();
            }
        }

        public INamingScope SafeSubScope()
        {
            return new EntityProxyNameScope( this );
        }
    }
}

