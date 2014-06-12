using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Restful;
using Restful.Data.Attributes;
using Restful.Linq;
using Restful.Data.MySql.CommandBuilders;
using Restful.Data.MySql.Common;
using Restful.Data.MySql.Visitors;

namespace Restful.Data.MySql.Linq
{
    public class MySqlUpdateable<T> : IUpdateable<T>
    {
        private readonly Type elementType;
        private readonly IDictionary<MemberExpression, object> properties;
        private readonly IUpdateProvider provider;

        public MySqlUpdateable( IUpdateProvider provider )
        {
            this.provider = provider;
            this.elementType = typeof( T );
            this.properties = new Dictionary<MemberExpression, object>();
        }

        public MySqlUpdateable( IUpdateProvider provider, Type elementType )
        {
            this.provider = provider;
            this.elementType = elementType;
            this.properties = new Dictionary<MemberExpression, object>();
        }

        #region IUpdateable implementation

        public Type ElementType
        {
            get
            {
                return this.elementType;
            }
        }

        public IDictionary<MemberExpression, object> Properties
        {
            get
            {
                return this.properties;
            }
        }

        public Expression Predicate { get; set; }

        public IUpdateProvider Provider
        {
            get
            {
                return this.provider;
            }
        }

        #endregion

        //        private readonly MySqlParameterAggregator parameterAggregator;
        //        private readonly MySqlUpdatePartsAggregator updatePartsAggregator;
        //
        //        /// <summary>
        //        /// 构造方法
        //        /// </summary>
        //        /// <param name="provider"></param>
        //        public MySqlUpdateable( ISessionProvider provider ) : base( provider )
        //        {
        //            this.parameterAggregator = new MySqlParameterAggregator();
        //            this.updatePartsAggregator = new MySqlUpdatePartsAggregator();
        //            this.updatePartsAggregator.TableName = typeof( T ).Name;
        //        }
        //
        //        protected override IDbUpdateable<T> OnSet( object @object )
        //        {
        //            IEntityObject entity = (IEntityObject)@object;
        //
        //            this.updatePartsAggregator.Set.Clear();
        //
        //            PropertyInfo[] properties = @object.GetType().GetProperties();
        //
        //            foreach( var property in properties )
        //            {
        //                if( Attribute.GetCustomAttributes( property, typeof( PrimaryKeyAttribute ), true ).Length > 0 )
        //                    continue;
        //
        //                if( entity.ChangedProperties.Contains( property.Name ) == false )
        //                    continue;
        //
        //                object value = property.EmitGetValue( @object );
        //
        //                value = value == null ? DBNull.Value : value;
        //
        //                string parameterName = this.parameterAggregator.AddParameter( value );
        //
        //                if( this.updatePartsAggregator.Set.Length == 0 )
        //                {
        //                    this.updatePartsAggregator.Set.AppendFormat( "{0}{1}{2} = {3}", this.provider.LeftQuote, property.Name, this.provider.RightQuote, parameterName );
        //                }
        //                else
        //                {
        //                    this.updatePartsAggregator.Set.AppendFormat( ", {0}{1}{2} = {3}", this.provider.LeftQuote, property.Name, this.provider.RightQuote, parameterName );
        //                }
        //            }
        //
        //            return this;
        //        }
        //
        //        protected override IDbUpdateable<T> OnWhere( Expression<Func<T, bool>> func )
        //        {
        //            Expression expression = PartialEvaluatingExpressionTreeVisitor.EvaluateIndependentSubtrees( func );
        //
        //            MySqlWhereClauseVisitor visitor = new MySqlWhereClauseVisitor( this.parameterAggregator );
        //
        //            string whereSqlParts = visitor.Translate( expression );
        //
        //            if( this.updatePartsAggregator.Where.Length == 0 )
        //            {
        //                this.updatePartsAggregator.Where.AppendFormat( " ( {0} )", whereSqlParts );
        //            }
        //            else
        //            {
        //                this.updatePartsAggregator.Where.AppendFormat( " AND ( {0} )", whereSqlParts );
        //            }
        //
        //            return this;
        //        }
        //
        //        protected override CommandBuilder Command
        //        {
        //            get
        //            {
        //                return new CommandBuilder( this.updatePartsAggregator.ToString(), this.parameterAggregator.Parameters );
        //            }
        //        }
    }
}
