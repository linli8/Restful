using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Restful.Data;
using Restful.Data.Common;
using Restful.Data.Entity;
using Restful.Data.MySql;
using Restful.UnitTest.Entities;

namespace Restful.UnitTest
{
    [TestClass]
    public class SessionTests
    {
        static SessionTests()
        {
            SessionFactories.Register<MySqlSessionFactory>();
        }

        #region 测试 LINQ 查询
        /// <summary>
        /// 测试 LINQ 查询
        /// </summary>
        [TestMethod()]
        public void Find()
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                var person = new Person() { Name = "test", Age = 20, Money = 100, CreateTime = DateTime.Now, IsActive = true };

                session.Insert( person );

                int id = session.GetIndentifer<int>();

                var queryable = session.Find<Person>().Where( s => s.Id == id );

                // 测试 Single 函数
                person = queryable.Single();

                Assert.AreEqual( "test", person.Name );
                Assert.AreEqual( 20, person.Age.Value );
                Assert.AreEqual( 100, person.Money.Value );
                Assert.AreEqual( true, person.IsActive );

                // 测试 SingleOrDefault 函数
                person = queryable.SingleOrDefault();

                Assert.AreEqual( "test", person.Name );
                Assert.AreEqual( 20, person.Age.Value );
                Assert.AreEqual( 100, person.Money.Value );
                Assert.AreEqual( true, person.IsActive );

                // 测试 First 函数
                person = queryable.First();

                Assert.AreEqual( "test", person.Name );
                Assert.AreEqual( 20, person.Age.Value );
                Assert.AreEqual( 100, person.Money.Value );
                Assert.AreEqual( true, person.IsActive );

                // 测试 FirstOrDefault 函数
                person = queryable.FirstOrDefault();

                Assert.AreEqual( "test", person.Name );
                Assert.AreEqual( 20, person.Age.Value );
                Assert.AreEqual( 100, person.Money.Value );
                Assert.AreEqual( true, person.IsActive );

                // 测试 Count 函数
                Assert.AreEqual( 1, queryable.Count() );

                // 测试 ToList 函数
                var list = queryable.ToList();

                Assert.AreEqual( 1, list.Count() );

                // 测试包含多个 Where 条件
                queryable = session.Find<Person>()
                    .Where( s => s.Id == id )
                    .Where( s => s.Name == "test" )
                    .Where( s => s.Age == 20 && s.IsActive == true );

                Assert.AreEqual( 1, queryable.Count() );

                Console.WriteLine( SqlCmd.Current.Sql );
            }
        }
        #endregion
    }
}
