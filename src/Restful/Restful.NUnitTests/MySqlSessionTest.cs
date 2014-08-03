using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;
using Restful.Data;
using Restful.Data.Attributes;
using Restful.Data.MySql;
using Restful.Linq;
using System.Data;
using System.Data.Common;

namespace Restful.NUnitTests
{

    [TestFixture()]
    public class MySqlSessionTest
    {

        #region Person

        [Serializable]
        public class Person
        {
            [PrimaryKey, AutoIncrease]
            public virtual int Id { get; set; }

            public virtual string Name { get; set; }

            public virtual int? Age { get; set; }

            public virtual decimal? Money { get; set; }

            public virtual DateTime CreateTime { get; set; }

            public virtual bool IsActive { get; set; }
        }

        #endregion

        static MySqlSessionTest()
        {
            SessionProviderFactories.Register<MySqlSessionProviderFactory>();
            SessionFactory.Default = "MySql";
        }

        #region Clear

        [Test]
        public void Clear()
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                session.Delete<Person>().Execute();

                var queryable = session.Find<Person>().Count();

                Assert.AreEqual( 0, queryable );
            }
        }

        #endregion

        #region Insert 100K Records

        [Test]
        public void Insert100KRecords()
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                Stopwatch watch = new Stopwatch();

                watch.Start();

                for( int i = 0; i < 10000; i++ )
                {
                    var person = EntityHelper.CreateProxy<Person>();

                    person.Name = "test";
                    person.Age = 20;
                    person.Money = 100;
                    person.CreateTime = DateTime.Now;
                    person.IsActive = false;

                    session.Insert( person );
                }

                watch.Stop();

                Console.WriteLine( watch.Elapsed );

                int count = session.Find<Person>().Count();

                Assert.AreEqual( 10000, count );
            }
        }

        #endregion

        #region Find 100K Records

        [Test]
        public void Find100KRecords()
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                Stopwatch watch = new Stopwatch();

                for( int i = 0; i < 10; i++ )
                {
                    watch.Start();

                    var queryable = session.Find<Person>();

                    var list = queryable.ToList();

                    watch.Stop();

                    Console.WriteLine( watch.Elapsed );

                    watch.Reset();

                    Assert.LessOrEqual( 10000, list.Count );
                }
            }
        }

        #endregion

        #region Update 100K Records

        [Test]
        public void Update100KRecords()
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                Stopwatch watch = new Stopwatch();

                watch.Start();

                for( int i = 0; i < 100000; i++ )
                {
                    var person = EntityHelper.CreateProxy<Person>();

                    person.Name = "test";
                    person.CreateTime = DateTime.Now;
                    person.IsActive = false;

                    session.Update( person );
                }

                watch.Stop();
                Console.WriteLine( watch.Elapsed );
            }
        }

        #endregion

        #region 测试 ExecuteScalar

        /// <summary>
        /// 测试 ExecuteScalar
        /// </summary>
        [Test]
        public void ExecuteScalar()
        {
            // 新增时，对象务必使用 EntityProxyGenerator 创建实体代理，否则无法跟踪属性变化
            var person = EntityHelper.CreateProxy<Person>();

            person.Name = "test";
            //person.Age = 20;
            person.Money = 100;
            person.CreateTime = DateTime.Now;
            person.IsActive = false;

            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                session.Insert( person );

                int id = session.GetIndentifer<int>();

                string sql = "select count(*) from Person where Id = ? and CreateTime <= ?";

                int count = session.ExecuteScalar<int>( sql, id, DateTime.Now.AddSeconds( 1 ) );

                Assert.AreEqual( 1, count );
            }
        }

        #endregion

        #region 测试 ExecuteDataPage

        /// <summary>
        /// 测试 ExecutePageQuery
        /// </summary>
        [Test()]
        public void ExecuteDataPage()
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                int id = 0;

                for( int i = 0; i < 20; i++ )
                {
                    // 新增时，对象务必使用 EntityProxyGenerator 创建实体代理，否则无法跟踪属性变化
                    var person = EntityHelper.CreateProxy<Person>();

                    person.Name = "test";
                    //person.Age = 20;
                    person.Money = 100;
                    person.CreateTime = DateTime.Now;
                    person.IsActive = false;

                    session.Insert( person );

                    id = session.GetIndentifer<int>();
                }

                string sql = "select * from Person where Id <= ? and CreateTime < ?";

                var dataPage = session.ExecuteDataPage( sql, 1, 20, "CreateTime desc", id, DateTime.Now );

                Assert.AreEqual( 20, dataPage.Data.Rows.Count );
            }
        }

        #endregion

        #region 测试数据新增

        /// <summary>
        /// 测试数据新增
        /// </summary>
        [Test()]
        public void Insert()
        {
            // 新增时，对象务必使用 EntityHelper 创建实体代理，否则无法跟踪属性变化
            var person = EntityHelper.CreateProxy<Person>();

            person.Name = "test";
            //person.Age = 20;
            person.Money = 100;
            person.CreateTime = DateTime.Now;
            person.IsActive = false;

            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                // 直接插入实体
                int i = session.Insert( person );

                // 输出生成的SQL语句
                Console.WriteLine( session.Provider.ExecutedCommandBuilder );

                int id = session.GetIndentifer<int>();

                Assert.AreEqual( 1, i );
                Assert.Greater( id, 0 );

                // Lambda 表达式插入
                i = session.Insert<Person>()
                    .Set( s => s.Name, "test" )
                    .Set( s => s.Age, 20 )
                    .Set( s => s.Money, 200 )
                    .Set( s => s.CreateTime, DateTime.Now )
                    .Set( s => s.IsActive, true )
                    .Execute();

                // 输出生成的SQL语句
                Console.WriteLine( session.Provider.ExecutedCommandBuilder );

                id = session.GetIndentifer<int>();

                Assert.AreEqual( 1, i );
                Assert.Greater( id, 0 );
            }
        }

        #endregion

        #region 测试数据更新

        /// <summary>
        /// 测试数据更新
        /// </summary>
        [Test()]
        public void Update()
        {
            var person = EntityHelper.CreateProxy<Person>();

            person.Name = "test";
            person.Age = 20;
            person.Money = 100;
            person.CreateTime = DateTime.Now;
            person.IsActive = true;

            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                int i = session.Insert( person );

                int id = session.GetIndentifer<int>();

                // Find 方法返回的对象都是原始对象而非代理对象
                person = session.Find<Person>().Where( s => s.Id == id ).Single();

                Console.WriteLine( session.Provider.ExecutedCommandBuilder );

                // 根据原始对象创建代理对象
                person = person.ToEntityProxy();

                person.Name = "test01";
                person.Age = 31;
                person.Money = 200;

                i = session.Update( person );

                Console.WriteLine( session.Provider.ExecutedCommandBuilder );

                person = session.Find<Person>().Where( s => s.Id == id ).Single();

                Console.WriteLine( session.Provider.ExecutedCommandBuilder );

                Assert.AreEqual( 1, i );
                Assert.AreEqual( "test01", person.Name );
                Assert.AreEqual( 31, person.Age.Value );
                Assert.AreEqual( 200, person.Money.Value );

                person = EntityHelper.CreateProxy<Person>();

                person.Name = "test02";
                person.Age = 22;
                person.Money = 101;
                person.CreateTime = DateTime.Now;
                person.IsActive = false;

                i = session.Update<Person>()
                    .Set( person )
                    .Where( s => s.Id == id )
                    .Execute();

                Console.WriteLine( session.Provider.ExecutedCommandBuilder );

                person = session.Find<Person>().Where( s => s.Id == id ).Single();

                Assert.AreEqual( 1, i );
                Assert.AreEqual( "test02", person.Name );
                Assert.AreEqual( 22, person.Age.Value );
                Assert.AreEqual( 101, person.Money.Value );
                Assert.AreEqual( false, person.IsActive );

                i = session.Update<Person>().Set( s => s.Name, "test03" )
                    .Set( s => s.Age, 23 )
                    .Set( s => s.Money, 102 )
                    .Set( s => s.CreateTime, DateTime.Now )
                    .Set( s => s.IsActive, true )
                    .Where( s => s.Id == id )
                    .Execute();

                Console.WriteLine( session.Provider.ExecutedCommandBuilder );

                person = session.Find<Person>().Where( s => s.Id == id ).Single();

                Assert.AreEqual( 1, i );
                Assert.AreEqual( "test03", person.Name );
                Assert.AreEqual( 23, person.Age.Value );
                Assert.AreEqual( 102, person.Money.Value );
                Assert.AreEqual( true, person.IsActive );
            }
        }

        #endregion

        #region 测试数据删除

        /// <summary>
        /// 测试数据删除
        /// </summary>
        [Test()]
        public void Delete()
        {
            var person = EntityHelper.CreateProxy<Person>();

            person.Name = "test";
            person.Age = 20;
            person.Money = 100;
            person.CreateTime = DateTime.Now;
            person.IsActive = true;

            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                int i = session.Insert( person );

                int id = session.GetIndentifer<int>();

                person = session.Find<Person>().Where( s => s.Id == id ).Single();

                i = session.Delete( person );

                Console.WriteLine( session.Provider.ExecutedCommandBuilder );

                var queryable = session.Find<Person>().Where( s => s.Id == id );

                Assert.AreEqual( 1, i );
                Assert.AreEqual( 0, queryable.Count() );

                person = EntityHelper.CreateProxy<Person>();

                person.Name = "test";
                person.Age = 20;
                person.Money = 100;
                person.CreateTime = DateTime.Now;
                person.IsActive = true;

                i = session.Insert( person );

                id = session.GetIndentifer<int>();

                person = session.Find<Person>().Where( s => s.Id == id ).Single();

                i = session.Delete<Person>().Where( s => s.Id == id ).Execute();

                Console.WriteLine( session.Provider.ExecutedCommandBuilder );

                Assert.AreEqual( 1, i );
                Assert.AreEqual( 0, queryable.Count() );
            }
        }

        #endregion

        #region 测试 LINQ 查询

        /// <summary>
        /// 测试 LINQ 查询
        /// </summary>
        [Test()]
        public void Find()
        {
            var person = EntityHelper.CreateProxy<Person>();

            person.Name = "test";
            person.Age = 20;
            person.Money = 100;
            person.CreateTime = DateTime.Now;
            person.IsActive = true;

            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                session.Insert( person );

                int id = session.GetIndentifer<int>();

                var queryable = session.Find<Person>().Where( s => s.Id == id );

                // 测试 Single 函数
                person = queryable.Single();

                Console.WriteLine( session.Provider.ExecutedCommandBuilder );

                Assert.AreEqual( "test", person.Name );
                Assert.AreEqual( 20, person.Age.Value );
                Assert.AreEqual( 100, person.Money.Value );
                Assert.AreEqual( true, person.IsActive );

                // 测试 SingleOrDefault 函数
                person = queryable.SingleOrDefault();

                Console.WriteLine( session.Provider.ExecutedCommandBuilder );

                Assert.AreEqual( "test", person.Name );
                Assert.AreEqual( 20, person.Age.Value );
                Assert.AreEqual( 100, person.Money.Value );
                Assert.AreEqual( true, person.IsActive );

                // 测试 First 函数
                person = queryable.First();

                Console.WriteLine( session.Provider.ExecutedCommandBuilder );

                Assert.AreEqual( "test", person.Name );
                Assert.AreEqual( 20, person.Age.Value );
                Assert.AreEqual( 100, person.Money.Value );
                Assert.AreEqual( true, person.IsActive );

                // 测试 FirstOrDefault 函数
                person = queryable.FirstOrDefault();

                Console.WriteLine( session.Provider.ExecutedCommandBuilder );

                Assert.AreEqual( "test", person.Name );
                Assert.AreEqual( 20, person.Age.Value );
                Assert.AreEqual( 100, person.Money.Value );
                Assert.AreEqual( true, person.IsActive );

                // 测试 Count 函数
                Assert.AreEqual( 1, queryable.Count() );

                Console.WriteLine( session.Provider.ExecutedCommandBuilder );

                // 测试 ToList 函数
                var list = queryable.ToList();

                Console.WriteLine( session.Provider.ExecutedCommandBuilder );

                Assert.AreEqual( 1, list.Count() );

                // 测试包含多个 Where 条件
                queryable = session.Find<Person>()
                    .Where( s => s.Id == id )
                    .Where( s => s.Name.Contains( "test" ) )
                    .OrderBy( s => s.CreateTime )
                    .OrderBy( s => s.Name )
                    .Skip( 0 ).Take( 1 );

                Assert.AreEqual( 1, queryable.Count() );

                Console.WriteLine( session.Provider.ExecutedCommandBuilder );
            }
        }

        #endregion

        #region 测试 Test

        /// <summary>
        /// 测试 ExecuteScalar
        /// </summary>
        [Test]
        public void Test()
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                var persons = from s in  session.Find<Person>()
                                          select new Person(){ Id = s.Id, Name = s.Name, Age = s.Age };

                persons.ToList();
            }
        }

        #endregion

    }
}

