using System;
using System.Linq.Expressions;
using System.Reflection;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Restful.Data.Attributes;
using Restful.Data.Common;
using Restful.Data.Entity;
using Restful.Data.Linq;
using Restful.Data.Oracle.Common;
using Restful.Data.Oracle.SqlParts;
using Restful.Data.Oracle.Visitors;
using Restful.Extensions;

namespace Restful.Data.Oracle.Linq
{
    public class OracleUpdateable<T> : IUpdateable<T> where T : EntityObject
    {
        #region Member
        /// <summary>
        /// Session 提供程序
        /// </summary>
        private OracleSessionProvider provider;

        /// <summary>
        /// 参数聚合器
        /// </summary>
        private OracleParameterAggregator parameterAggregator;

        /// <summary>
        /// DELETE 语句
        /// </summary>
        private OracleUpdatePartsAggregator updatePartsAggregator;
        #endregion

        #region OracleUpdateable
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="provider"></param>
        public OracleUpdateable( OracleSessionProvider provider )
        {
            this.provider = provider;
            this.parameterAggregator = new OracleParameterAggregator();
            this.updatePartsAggregator = new OracleUpdatePartsAggregator();
            this.updatePartsAggregator.TableName = typeof( T ).Name;
        }
        #endregion

        #region Set
        /// <summary>
        /// 设置更新字段
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public IUpdateable<T> Set( T @object )
        {
            this.updatePartsAggregator.Set.Clear();

            PropertyInfo[] properties = @object.GetType().GetProperties();

            foreach( var property in properties )
            {
                if( Attribute.GetCustomAttributes( property, typeof( PrimaryKeyAttribute ), true ).Length > 0 ) continue;

                if( @object.ChangedProperties.Contains( property.Name ) == false ) continue;

                object value = property.EmitGetValue( @object );

                value = value == null ? DBNull.Value : value;

                string parameterName = this.parameterAggregator.AddParameter( value );

                if( this.updatePartsAggregator.Set.Length == 0 )
                {
                    this.updatePartsAggregator.Set.AppendFormat( "{0}{1}{2} = {3}", Constants.LeftQuote, property.Name, Constants.RightQuote, parameterName );
                }
                else
                {
                    this.updatePartsAggregator.Set.AppendFormat( ", {0}{1}{2} = {3}", Constants.LeftQuote, property.Name, Constants.RightQuote, parameterName );
                }
            }

            return this;
        }
        #endregion

        #region Where
        /// <summary>
        /// 设置过滤条件
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public IUpdateable<T> Where( Expression<Func<T, bool>> func )
        {
            Expression expression = PartialEvaluatingExpressionTreeVisitor.EvaluateIndependentSubtrees( func );

            OracleWhereClauseVisitor visitor = new OracleWhereClauseVisitor( this.parameterAggregator );

            string whereSqlParts = visitor.Translate( expression );

            if( this.updatePartsAggregator.Where.Length == 0 )
            {
                this.updatePartsAggregator.Where.AppendFormat( " ( {0} )", whereSqlParts );
            }
            else
            {
                this.updatePartsAggregator.Where.AppendFormat( " AND ( {0} )", whereSqlParts );
            }

            return this;
        }
        #endregion

        #region Provider
        /// <summary>
        /// 
        /// </summary>
        public ISessionProvider Provider
        {
            get { return this.provider; }
        }
        #endregion

        #region Execute
        public int Execute()
        {
            SqlCmd command = new SqlCmd( this.updatePartsAggregator.ToString(), this.parameterAggregator.Parameters );

            SqlCmd.Current = command;

            return this.provider.ExecuteNonQuery( command.Sql, command.Parameters );
        }
        #endregion
    }
}
