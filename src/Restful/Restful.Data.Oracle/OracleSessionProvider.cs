using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;
using Remotion.Linq.Parsing.Structure;
using Restful.Data.Attributes;
using Restful.Data.Common;
using Restful.Data.Entity;
using Restful.Data.Extensions;
using Restful.Data.Linq;
using Restful.Data.Oracle.Common;
using Restful.Data.Oracle.Linq;
using Restful.Extensions;

namespace Restful.Data.Oracle
{
    public class OracleSessionProvider : ISessionProvider
    {
        private bool disposed;
        private string connectionStr;
        protected OracleConnection connection;
        protected OracleTransaction transaction;

        #region OracleSessionProvider
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionStr"></param>
        public OracleSessionProvider( string connectionStr )
        {
            this.connectionStr = connectionStr;
            this.connection = new OracleConnection( this.connectionStr );
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
        private void PrepareParameter( OracleCommand command, IDictionary<string, object> parameters )
        {
            if( parameters == null || parameters.Count == 0 )
            {
                return;
            }

            foreach( var item in parameters )
            {
                OracleParameter parameter = command.CreateParameter();
                parameter.ParameterName = item.Key;
                parameter.Value = item.Value == null ? DBNull.Value : item.Value;
                command.Parameters.Add( parameter );
            }
        }
        #endregion

        #region Transaction
        public DbTransaction BeginTransaction()
        {
            return this.transaction = this.connection.BeginTransaction();
        }
        #endregion

        #region ExecuteScalar
        public T ExecuteScalar<T>( string sql, IDictionary<string, object> parameters )
        {
            using( OracleCommand command = connection.CreateCommand() )
            {
                command.CommandText = sql;
                command.Transaction = this.transaction;

                this.PrepareParameter( command, parameters );

                return command.ExecuteScalar().Cast<T>();
            }
        }
        #endregion

        #region ExecuteDataReader
        public IDataReader ExecuteDataReader( string sql, IDictionary<string, object> parameters )
        {
            using( OracleCommand command = connection.CreateCommand() )
            {
                command.CommandText = sql;
                command.Transaction = this.transaction;

                this.PrepareParameter( command, parameters );

                return command.ExecuteReader();
            }
        }
        #endregion

        #region ExecuteDataTable
        public DataTable ExecuteDataTable( string sql, IDictionary<string, object> parameters )
        {
            using( OracleCommand command = connection.CreateCommand() )
            {
                command.CommandText = sql;
                command.Transaction = transaction;

                this.PrepareParameter( command, parameters );

                using( OracleDataAdapter adapter = new OracleDataAdapter( command ) )
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
        public DataSet ExecuteDataSet( string sql, IDictionary<string, object> parameters )
        {
            using( OracleCommand command = connection.CreateCommand() )
            {
                command.CommandText = sql;
                command.Transaction = this.transaction;

                this.PrepareParameter( command, parameters );

                using( OracleDataAdapter adapter = new OracleDataAdapter( command ) )
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
        public int ExecuteNonQuery( string sql, IDictionary<string, object> parameters )
        {
            using( OracleCommand command = connection.CreateCommand() )
            {
                command.CommandText = sql;
                command.Transaction = this.transaction;

                this.PrepareParameter( command, parameters );

                return command.ExecuteNonQuery();
            }
        }
        #endregion

        #region ExecutePageQuery
        public PageQueryResult ExecutePageQuery( string sql, int pageIndex, int pageSize, string orderBy, IDictionary<string, object> parameters )
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
                parameters = new Dictionary<string, object>();
            }

            parameters.Add( "@LimitFrom", ( result.PageIndex - 1 ) * result.PageSize );
            parameters.Add( "@LimitCount", result.PageSize );

            string queryItemSql = string.Format( "SELECT * FROM ( {0} ) T ORDER BY {1} LIMIT @LimitFrom, @LimitCount", sql, orderBy );

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
        public int Insert( EntityObject @object )
        {
            string tableName = @object.GetType().Name;

            IList<string> columns = new List<string>();

            IDictionary<string, object> parameters = new Dictionary<string, object>();

            @object.GetType().GetProperties().Each( s =>
            {
                // 非自增字段
                if( Attribute.GetCustomAttributes( s, typeof( AutoIncreaseAttribute ), true ).Length == 0 )
                {
                    if( @object.ChangedProperties.Contains( s.Name ) )
                    {
                        columns.Add( string.Format( "{0}{1}{2}", Constants.LeftQuote, s.Name, Constants.RightQuote ) );

                        object value = s.EmitGetValue( @object );

                        value = value == null ? DBNull.Value : value;

                        parameters.Add( string.Format( "{0}{1}", Constants.ParameterPrefix, s.Name ), value );
                    }
                }
            } );

            StringBuilder builder = new StringBuilder();

            builder.Append( "INSERT INTO " );
            builder.AppendFormat( "{0}{1}{2} ", Constants.LeftQuote, tableName, Constants.RightQuote );
            builder.Append( "( " );
            builder.Append( string.Join( ", ", columns ) );
            builder.Append( " ) VALUES ( " );
            builder.Append( string.Join( ", ", parameters.Keys ) );
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
        public int Update( EntityObject @object )
        {
            string tableName = @object.GetType().Name;

            IList<string> keys = new List<string>();
            IList<string> columns = new List<string>();
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            var properties = @object.GetType().GetProperties();

            foreach( var property in properties )
            {
                // 非主键字段
                if( Attribute.GetCustomAttributes( property, typeof( PrimaryKeyAttribute ), true ).Length == 0 )
                {
                    if( @object.ChangedProperties.Contains( property.Name ) )
                    {
                        columns.Add( string.Format( "{0}{1}{2} = {3}{1}", Constants.LeftQuote, property.Name, Constants.RightQuote, Constants.ParameterPrefix ) );

                        object value = property.EmitGetValue( @object );

                        value = value == null ? DBNull.Value : value;

                        parameters.Add( string.Format( "{0}{1}", Constants.ParameterPrefix, property.Name ), value );
                    }
                }
                else
                {
                    keys.Add( string.Format( "{0}{1}{2} = {3}{1}", Constants.LeftQuote, property.Name, Constants.RightQuote, Constants.ParameterPrefix ) );

                    object value = property.EmitGetValue( @object );

                    value = value == null ? DBNull.Value : value;

                    parameters.Add( string.Format( "{0}{1}", Constants.ParameterPrefix, property.Name ), value );
                }
            }

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
        public int Delete( EntityObject @object )
        {
            string tableName = @object.GetType().Name;

            IList<string> keys = new List<string>();
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            var properties = @object.GetType().GetProperties();

            foreach( var property in properties )
            {
                // 非主键字段
                if( Attribute.GetCustomAttributes( property, typeof( PrimaryKeyAttribute ), true ).Length > 0 )
                {
                    keys.Add( string.Format( "{0}{1}{2} = {3}{1}", Constants.LeftQuote, property.Name, Constants.RightQuote, Constants.ParameterPrefix ) );

                    object value = property.EmitGetValue( @object );

                    value = value == null ? DBNull.Value : value;

                    parameters.Add( string.Format( "{0}{1}", Constants.ParameterPrefix, property.Name ), value );
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
        public IUpdateable<T> Update<T>() where T : EntityObject
        {
            return new OracleUpdateable<T>( this );
        }
        #endregion

        #region Delete<T>
        public IDeleteable<T> Delete<T>() where T : EntityObject
        {
            return new OracleDeleteable<T>( this );
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
            return new OracleQueryable<T>( QueryParser.CreateDefault(), new OracleQueryExecutor( this ) );
        }
        #endregion

        #region Find<T>
        public IEnumerable<T> Find<T>( string sql, IDictionary<string, object> parameters )
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
