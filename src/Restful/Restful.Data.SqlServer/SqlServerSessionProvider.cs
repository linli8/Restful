using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Remotion.Linq.Parsing.Structure;
using Restful.Data.Attributes;
using Restful.Data.Common;
using Restful.Data.Entity;
using Restful.Data.Extensions;
using Restful.Data.Linq;
using Restful.Data.SqlServer.Common;
using Restful.Data.SqlServer.Linq;
using Restful.Extensions;
using System.Reflection;
using System.Data.SqlClient;

namespace Restful.Data.SqlServer
{
    public class SqlServerSessionProvider : ISessionProvider
    {
        private bool disposed;
        private string connectionStr;
        protected SqlConnection connection;
        protected SqlTransaction transaction;

        #region SqlServerSessionProvider
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionStr"></param>
        public SqlServerSessionProvider( string connectionStr )
        {
            this.connectionStr = connectionStr;
            this.connection = new SqlConnection( this.connectionStr );
            this.connection.Open();
        }
        #endregion

        #region Dispose
        /// <summary>
        /// 析构函数
        /// </summary>
        public void Dispose()
        {
            this.Dispose( true );
            GC.SuppressFinalize( this );
        }

        protected virtual void Dispose( bool disposing )
        {
            if( this.disposed == false )
            {
                if( disposing )
                {
                    this.connection.Close();
                }
            }

            this.disposed = true;
        }
        #endregion

        #region PrepareParameter
        /// <summary>
        /// PrepareParameter
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        private void PrepareParameter( SqlCommand command, IList<object> parameters )
        {
            if( parameters == null || parameters.Count == 0 )
            {
                return;
            }

            string[] parts = command.CommandText.Split('?');

            if (parts.Length - 1 != parameters.Count)
            {
                throw new ArgumentOutOfRangeException("参数数量与参数值数量不一致。");
            }

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < parts.Length; i++)
            {
                builder.Append(parts[i]);

                if (i < parts.Length - 1)
                {
                    string parameterName = string.Format("{0}P{1}", Constants.ParameterPrefix, i);
                    object value = parameters[i];

                    builder.Append(parameterName);

                    SqlParameter parameter = command.CreateParameter();

                    parameter.ParameterName = parameterName;
                    parameter.Value = value == null ? DBNull.Value : value;

                    command.Parameters.Add(parameter);
                }
            }

            command.CommandText = builder.ToString();
        }
        #endregion

        #region Transaction
        public DbTransaction BeginTransaction()
        {
            return this.transaction = this.connection.BeginTransaction();
        }
        #endregion

        #region ExecuteScalar
        public T ExecuteScalar<T>( string sql, IList<object> parameters )
        {
            using( SqlCommand command = connection.CreateCommand() )
            {
                command.CommandText = sql;
                command.Transaction = this.transaction;

                this.PrepareParameter( command, parameters );

                return command.ExecuteScalar().Cast<T>();
            }
        }
        #endregion

        #region ExecuteDataReader
        public IDataReader ExecuteDataReader( string sql, IList<object> parameters )
        {
            using( SqlCommand command = connection.CreateCommand() )
            {
                command.CommandText = sql;
                command.Transaction = this.transaction;

                this.PrepareParameter( command, parameters );

                return command.ExecuteReader();
            }
        }
        #endregion

        #region ExecuteDataTable
        public DataTable ExecuteDataTable( string sql, IList<object> parameters )
        {
            using( SqlCommand command = connection.CreateCommand() )
            {
                command.CommandText = sql;
                command.Transaction = transaction;

                this.PrepareParameter( command, parameters );

                using( SqlDataAdapter adapter = new SqlDataAdapter( command ) )
                {
                    DataTable result = new DataTable( "Table1" );
                    adapter.SelectCommand = command;
                    adapter.Fill( result );
                    return result;
                }
            }
        }
        #endregion

        #region ExecuteDataSet
        public DataSet ExecuteDataSet( string sql, IList<object> parameters )
        {
            using( SqlCommand command = connection.CreateCommand() )
            {
                command.CommandText = sql;
                command.Transaction = this.transaction;

                this.PrepareParameter( command, parameters );

                using( SqlDataAdapter adapter = new SqlDataAdapter( command ) )
                {
                    DataSet result = new DataSet( "DataSet1" );
                    adapter.SelectCommand = command;
                    adapter.Fill( result );
                    return result;
                }
            }
        }
        #endregion

        #region ExecuteNonQuery
        public int ExecuteNonQuery( string sql, IList<object> parameters )
        {
            using( SqlCommand command = connection.CreateCommand() )
            {
                command.CommandText = sql;
                command.Transaction = this.transaction;

                this.PrepareParameter( command, parameters );

                return command.ExecuteNonQuery();
            }
        }
        #endregion

        #region ExecutePageQuery
        public PageQueryResult ExecutePageQuery( string sql, int pageIndex, int pageSize, string orderBy, IList<object> parameters )
        {
            if( string.IsNullOrEmpty( orderBy ) )
            {
                throw new ArgumentNullException( "orderBy" );
            }

            PageQueryResult result = new PageQueryResult( pageIndex, pageSize );

            #region 查询满足条件的条目数量
            string queryItemCountSql = string.Format( "SELECT COUNT(*) FROM ( {0} ) T1", sql );

            result.ItemCount = this.ExecuteScalar<int>( queryItemCountSql, parameters );

            if( result.ItemCount == 0 ) // 如果满足条件的数据条目数量为零，则重置页索引为 1
            {
                result.PageIndex = 1;
            }
            else if( result.PageIndex > result.PageCount )  // 如果指定的页索引大于查询直接总页数，则重置页索引为总页数
            {
                result.PageIndex = result.PageCount;
            }
            #endregion

            #region 查询最终的结果集
            if( parameters == null )
            {
                parameters = new List<object>();
            }

            parameters.Add( ( result.PageIndex - 1 ) * result.PageSize );
            parameters.Add( result.PageSize );

            string queryItemSql = string.Format( "SELECT * FROM ( {0} ) T ORDER BY {1} LIMIT ?, ?", sql, orderBy );

            result.Data = this.ExecuteDataTable( queryItemSql, parameters );
            #endregion

            return result;
        }
        #endregion

        #region ExecuteStoredProcedure
        public void ExecuteStoredProcedure( string storedProcedureName, IList<DbParameter> parameters )
        {
            using( DbCommand command = connection.CreateCommand() )
            {
                command.CommandText = storedProcedureName;
                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = this.transaction;

                if( parameters != null && parameters.Count > 0 )
                {
                    foreach( DbParameter parameter in parameters )
                    {
                        command.Parameters.Add( parameter );
                    }
                }

                command.ExecuteNonQuery();
            }
        }
        #endregion

        #region Insert
        public int Insert( object @object )
        {
            IEntityObject entity = (IEntityObject)@object;

            string tableName = @object.GetType().BaseType.Name;

            IList<string> columns = new List<string>();

            IList<object> parameters = new List<object>();

            PropertyInfo[] properties = @object.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                // 忽略自增字段
                if (Attribute.GetCustomAttributes(property, typeof(AutoIncreaseAttribute), true).Length > 0)
                    continue;

                if (entity.ChangedProperties.Contains(property.Name))
                {
                    columns.Add(string.Format("{0}{1}{2}", Constants.LeftQuote, property.Name, Constants.RightQuote));

                    object value = property.EmitGetValue(@object);

                    value = value == null ? DBNull.Value : value;

                    parameters.Add(value);
                }
            }

            StringBuilder builder = new StringBuilder();

            builder.Append( "INSERT INTO " );
            builder.AppendFormat( "{0}{1}{2} ", Constants.LeftQuote, tableName, Constants.RightQuote );
            builder.Append( "( " );
            builder.Append( string.Join( ", ", columns ) );
            builder.Append( " ) VALUES ( " );
            for (int i = 0; i < parameters.Count; i++)
            {
                if (i == 0)
                {
                    builder.Append("?");
                }
                else
                {
                    builder.Append(", ?");
                }
            }
            builder.Append( " );" );

            SqlCmd command = new SqlCmd( builder.ToString(), parameters );
            
            SqlCmd.Current = command;

            return this.ExecuteNonQuery( command.Sql, command.Parameters );
        }
        #endregion

        #region GetIdentifier
        public T GetIdentifier<T>()
        {
            return this.ExecuteScalar<T>( "SELECT LAST_INSERT_ID();", null );
        }
        #endregion

        #region Update
        public int Update( object @object )
        {
            IEntityObject entity = (IEntityObject)@object;

            string tableName = @object.GetType().BaseType.Name;

            IList<string> keys = new List<string>();
            IList<string> columns = new List<string>();
            IList<object> parameters = new List<object>();

            var properties = @object.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // 需更新的字段
            properties.Where(property => Attribute.GetCustomAttributes(property, typeof(PrimaryKeyAttribute), true).Length == 0).Each(property =>
            {
                if (@entity.ChangedProperties.Contains(property.Name))
                {
                    columns.Add(string.Format("{0}{1}{2} = ?", Constants.LeftQuote, property.Name, Constants.RightQuote));

                    object value = property.EmitGetValue(@object);

                    value = value == null ? DBNull.Value : value;

                    parameters.Add(value);
                }
            });

            // 主键字段
            properties.Where(property => Attribute.GetCustomAttributes(property, typeof(PrimaryKeyAttribute), true).Length > 0).Each(property =>
            {
                keys.Add( string.Format( "{0}{1}{2} = ?", Constants.LeftQuote, property.Name, Constants.RightQuote, Constants.ParameterPrefix ) );

                object value = property.EmitGetValue( @object );

                value = value == null ? DBNull.Value : value;

                parameters.Add(value);
            });

            StringBuilder builder = new StringBuilder();

            builder.Append( "UPDATE " );
            builder.AppendFormat( "{0}{1}{2} ", Constants.LeftQuote, tableName, Constants.RightQuote );
            builder.Append( "SET " );
            builder.Append( string.Join( ", ", columns ) );
            builder.Append( " WHERE " );
            builder.Append( string.Join( " AND ", keys ) );
            builder.Append( ";" );

            SqlCmd command = new SqlCmd( builder.ToString(), parameters );

            SqlCmd.Current = command;

            return this.ExecuteNonQuery( command.Sql, command.Parameters );
        }
        #endregion

        #region Delete
        public int Delete( object @object )
        {
            string tableName = @object.GetType().Name;

            if (@object.GetType().IsAssignableFrom(typeof(IEntityObject)))
            {
                tableName = @object.GetType().BaseType.Name;
            }

            IList<string> keys = new List<string>();
            IList<object> parameters = new List<object>();

            var properties = @object.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach( var property in properties )
            {
                // 非主键字段
                if( Attribute.GetCustomAttributes( property, typeof( PrimaryKeyAttribute ), true ).Length > 0 )
                {
                    keys.Add( string.Format( "{0}{1}{2} = ?", Constants.LeftQuote, property.Name, Constants.RightQuote, Constants.ParameterPrefix ) );

                    object value = property.EmitGetValue( @object );

                    value = value == null ? DBNull.Value : value;

                    parameters.Add(value);
                }
            }

            StringBuilder builder = new StringBuilder();

            builder.Append( "DELETE FROM " );
            builder.AppendFormat( "{0}{1}{2} ", Constants.LeftQuote, tableName, Constants.RightQuote );
            builder.AppendFormat( "WHERE " );
            builder.Append( string.Join( " AND ", keys ) );
            builder.Append( ";" );

            SqlCmd command = new SqlCmd( builder.ToString(), parameters );

            SqlCmd.Current = command;

            return this.ExecuteNonQuery( command.Sql, command.Parameters );
        }
        #endregion

        #region Update<T>
        public IUpdateable<T> Update<T>()
        {
            return new SqlServerUpdateable<T>( this );
        }
        #endregion

        #region Delete<T>
        public IDeleteable<T> Delete<T>()
        {
            return new SqlServerDeleteable<T>( this );
        }
        #endregion

        #region Find<T>
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> Find<T>()
        {
            return new SqlServerQueryable<T>( QueryParser.CreateDefault(), new SqlServerQueryExecutor( this ) );
        }
        #endregion

        #region Find<T>
        public IEnumerable<T> Find<T>( string sql, IList<object> parameters )
        {
            using( IDataReader reader = this.ExecuteDataReader( sql, parameters ) )
            {
                var tuple = reader.GetDeserializerState<T>();

                while( reader.Read() )
                {
                    yield return (T)tuple.Func( reader );
                }
            }
        }
        #endregion
    }
}
