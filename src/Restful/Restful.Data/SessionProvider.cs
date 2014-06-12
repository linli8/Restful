using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;
using Restful;
using Restful.Linq;
using Restful.Data.Attributes;
using System.Collections;

namespace Restful.Data
{
    /// <summary>
    /// session 提供程序基类
    /// </summary>
    public abstract class SessionProvider : ISessionProvider
    {
        private ISessionProviderFactory factory;
        private bool disposed;
        private string connectionStr;

        protected DbConnection connection;
        protected DbTransaction transaction;

        #region abstract method or properties

        /// <summary>
        /// 执行分页查询
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>分页查询结果</returns>
        public abstract DataPage ExecuteDataPage( CommandBuilder builder, int pageIndex, int pageSize, string orderBy );

        /// <summary>
        /// 获取最新插入数据的自增ID
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public abstract T GetIdentifier<T>();

        #endregion

        #region Properties

        /// <summary>
        /// 获取或设置当前执行的 SQL 命令
        /// </summary>
        /// <value></value>
        public CommandBuilder ExecutedCommandBuilder { get; set; }

        /// <summary>
        /// session 提供程序工厂
        /// </summary>
        public ISessionProviderFactory Factory
        {
            get
            {
                return this.factory;
            }
        }

        #endregion

        #region SessionProvider

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionStr"></param>
        public SessionProvider( ISessionProviderFactory factory, string connectionStr )
        {
            this.factory = factory;
            this.connectionStr = connectionStr;
            this.connection = this.factory.CreateConnection( this.connectionStr );
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
        /// 准备参数
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="parameters">参数列表</param>
        protected virtual void PrepareParameter( DbCommand command, IDictionary<string,object> parameters )
        {
            if( parameters == null || parameters.Count == 0 )
            {
                return;
            }

            foreach( var item in parameters )
            {
                DbParameter parameter = command.CreateParameter();

                parameter.ParameterName = item.Key;
                parameter.Value = item.Value == null ? DBNull.Value : item.Value;

                command.Parameters.Add( parameter );
            }
        }

        #endregion

        #region Transaction

        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        public virtual DbTransaction BeginTransaction()
        {
            return this.transaction = this.connection.BeginTransaction();
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// 执行带参数的 SQL 语句，返回查询结果中的第一行第一列的值
        /// </summary>
        /// <param name="cmd">SQL 命令</param>
        /// <returns>查询结果中的第一行第一列的值</returns>
        /// <typeparam name="T">返回类型</typeparam>
        public virtual T ExecuteScalar<T>( CommandBuilder builder )
        {
            using( DbCommand command = connection.CreateCommand() )
            {
                command.CommandText = builder.ToString();
                command.Transaction = this.transaction;

                this.PrepareParameter( command, builder.Parameters );

                return command.ExecuteScalar().Cast<T>();
            }
        }

        #endregion

        #region ExecuteDataReader

        /// <summary>
        /// 执行带参数 SQL 语句，返回一个 DataReader 对象
        /// </summary>
        /// <param name="cmd">SQL 命令</param>
        /// <returns>DataReader 对象</returns>
        public virtual IDataReader ExecuteDataReader( CommandBuilder builder )
        {
            using( DbCommand command = connection.CreateCommand() )
            {
                command.CommandText = builder.ToString();
                command.Transaction = this.transaction;

                this.PrepareParameter( command, builder.Parameters );

                return command.ExecuteReader();
            }
        }

        #endregion

        #region ExecuteDataTable

        /// <summary>
        /// 执行带参数 SQL 语句，返回一个 DataTable 对象
        /// </summary>
        /// <param name="cmd">SQL 命令</param>
        /// <returns>DataTable 对象</returns>
        public virtual DataTable ExecuteDataTable( CommandBuilder builder )
        {
            using( DbCommand command = connection.CreateCommand() )
            {
                command.CommandText = builder.ToString();
                command.Transaction = transaction;

                this.PrepareParameter( command, builder.Parameters );

                using( DbDataAdapter adapter = this.factory.CreateDataAdapter() )
                {
                    adapter.SelectCommand = command;

                    DataTable dt = new DataTable( "Table1" );

                    adapter.Fill( dt );

                    return dt;
                }
            }
        }

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// 执行带参数 SQL 语句，返回一个 DataTable 对象
        /// </summary>
        /// <param name="cmd">SQL 命令</param>
        /// <returns>DataSet 对象</returns>
        public virtual DataSet ExecuteDataSet( CommandBuilder builder )
        {
            using( DbCommand command = connection.CreateCommand() )
            {
                command.CommandText = builder.ToString();
                command.Transaction = this.transaction;

                this.PrepareParameter( command, builder.Parameters );

                using( DbDataAdapter adapter = this.factory.CreateDataAdapter() )
                {
                    adapter.SelectCommand = command;

                    DataSet ds = new DataSet( "DataSet1" );

                    adapter.Fill( ds );

                    return ds;
                }
            }
        }

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// 执行带参数的非查询 SQL 语句，返回受影响的行数
        /// </summary>
        /// <param name="sql">SQL 命令</param>
        /// <returns>受影响的行数</returns>
        /// <param name="parameters">Parameters.</param>
        public virtual int ExecuteNonQuery( CommandBuilder builder )
        {
            using( DbCommand command = connection.CreateCommand() )
            {
                command.CommandText = builder.ToString();
                command.Transaction = this.transaction;

                this.PrepareParameter( command, builder.Parameters );

                return command.ExecuteNonQuery();
            }
        }

        #endregion

        #region ExecuteStoredProcedure

        /// <summary>
        /// 调用存储过程
        /// </summary>
        /// <param name="storedProcedureName">存储过程名</param>
        /// <param name="parameters">参数集合</param>
        public virtual void ExecuteStoredProcedure( string storedProcedureName, IList<CommandParameter> parameters )
        {
            using( DbCommand command = connection.CreateCommand() )
            {
                command.CommandText = storedProcedureName;
                command.CommandType = CommandType.StoredProcedure;
                command.Transaction = this.transaction;

                if( parameters != null && parameters.Count > 0 )
                {
                    foreach( var item in parameters )
                    {
                        DbParameter parameter = command.CreateParameter();

                        parameter.ParameterName = item.ParameterName;
                        parameter.Size = item.Size;
                        parameter.Direction = item.Direction;
                        parameter.Value = item.Value;

                        command.Parameters.Add( parameter );
                    }
                }

                command.ExecuteNonQuery();
            }
        }

        #endregion

        #region Insert

        /// <summary>
        /// 将实体对象插入到数据库
        /// </summary>
        /// <param name="object">实体对象</param>
        public virtual int Insert( object @object )
        {
            IEntityObject entity = (IEntityObject)@object;

            Type elementType = @object.GetType().BaseType;

            IInsertable insertable = this.factory.CreateInsertProvider( this ).CreateInsert( elementType );

            var properties = @object.GetType().GetProperties( BindingFlags.Public | BindingFlags.Instance );

            properties.Where( property => property.IsAutoIncrease() == false ).Each( property =>
            {
                if( entity.ChangedProperties.Contains( property.Name ) )
                {
                    ParameterExpression pe = Expression.Parameter( @object.GetType(), "s" );
                    MemberExpression me = Expression.MakeMemberAccess( pe, property );

                    insertable.Set( me, property.EmitGetValue( @object ) );
                }
            } );

            return insertable.Execute( elementType );
        }

        #endregion

        #region Insert<T>

        public IInsertable<T> Insert<T>()
        {
            return this.factory.CreateInsertProvider( this ).CreateInsert<T>();
        }

        #endregion

        #region Update

        /// <summary>
        /// 将实体对象更新到数据库，调用该方法时实体对象必须具备主键属性
        /// </summary>
        /// <param name="object">实体对象</param>
        public virtual int Update( object @object )
        {
            IEntityObject entity = (IEntityObject)@object;

            Type elementType = @object.GetType().BaseType;

            IUpdateable updateable = this.factory.CreateUpdateProvider( this ).CreateUpdate( elementType );

            var properties = @object.GetType().GetProperties( BindingFlags.Public | BindingFlags.Instance );

            properties.Each( property =>
            {
                if( property.IsPrimaryKey() == false )
                {
                    if( entity.ChangedProperties.Contains( property.Name ) )
                    {
                        ParameterExpression pe = Expression.Parameter( @object.GetType(), "s" );
                        MemberExpression me = Expression.MakeMemberAccess( pe, property );

                        updateable.Set( me, property.EmitGetValue( @object ) );
                    }
                }
                else
                {
                    ParameterExpression pe = Expression.Parameter( @object.GetType(), "s" );
                    MemberExpression me = Expression.MakeMemberAccess( pe, property );
                    ConstantExpression ce = Expression.Constant( property.EmitGetValue( @object ), property.PropertyType );
                    BinaryExpression be = Expression.Equal( me, ce );

                    updateable = updateable.Where( be );
                }
            } );

            return updateable.Execute( elementType );
        }

        #endregion

        #region Delete

        /// <summary>
        /// 将实体对象从数据库中删除，调用该方法时实体对象必须具备主键属性
        /// </summary>
        /// <param name="object">实体对象</param>
        public virtual int Delete( object @object )
        {
            Type elementType = @object.GetType();

            if( @object.GetType().IsAssignableFrom( typeof( IEntityObject ) ) )
            {
                elementType = @object.GetType().BaseType;
            }
           
            IDeleteable deleteable = this.factory.CreateDeleteProvider( this ).CreateDelete( elementType );

            var properties = elementType.GetProperties( BindingFlags.Public | BindingFlags.Instance );

            properties.Where( property => property.IsPrimaryKey() ).Each( property =>
            {
                ParameterExpression pe = Expression.Parameter( @object.GetType(), "s" );
                MemberExpression me = Expression.MakeMemberAccess( pe, property );
                ConstantExpression ce = Expression.Constant( property.EmitGetValue( @object ), property.PropertyType );
                BinaryExpression be = Expression.Equal( me, ce );

                deleteable = deleteable.Where( be );
            } );

            return deleteable.Execute( elementType );
        }

        #endregion

        #region Update<T>

        /// <summary>
        /// 创建更新执行器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>IUpdator 对象</returns>
        public virtual IUpdateable<T> Update<T>()
        {
            return this.factory.CreateUpdateProvider( this ).CreateUpdate<T>();
        }

        #endregion

        #region Delete<T>

        /// <summary>
        /// 创建删除执行器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>IDeletor 对象</returns>
        public virtual IDeleteable<T> Delete<T>()
        {
            return this.factory.CreateDeleteProvider( this ).CreateDelete<T>();
        }

        #endregion

        #region Find<T>

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual IQueryable<T> Find<T>()
        {
            return this.factory.CreateQueryable<T>( this );
        }

        #endregion

        #region Find<T>

        /// <summary>
        /// 执行 SQL 语句，并返回查询结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> Find<T>( CommandBuilder builder )
        {
            using( IDataReader reader = this.ExecuteDataReader( builder ) )
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

