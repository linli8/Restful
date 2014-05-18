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
        public SessionTests()
        {
            SessionFactories.Register<MySqlSessionFactory>();
        }

        #region ExecuteDataTable01
        [TestMethod]
        public void ExecuteDataTable01()
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                string sql = "select * from Person";

                DataTable dt = session.ExecuteDataTable( sql );

                Assert.AreNotEqual<int>( 0, dt.Rows.Count );
            }
        }
        #endregion

        #region ExecutePageQuery02
        [TestMethod]
        public void ExecutePageQuery02()
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                string sql01 = "select * from User where CreateTime < @CreateTime";

                IDictionary<string, object> parameters = new Dictionary<string, object>();

                parameters.Add( "@CreateTime", DateTime.Now );

                PageQueryResult result = session.ExecutePageQuery( sql01, 2, 3, "CreateTime DESC", parameters );

                Assert.AreNotEqual<int>( 0, result.Data.Rows.Count );
            }
        }
        #endregion

        #region Insert
        [TestMethod]
        public void Insert()
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                Person person = new Person();

                person.Name = "testuser01";
                person.Age = 30;
                person.Money = 1999.23m;
                person.CreateTime = DateTime.Now;
                person.IsActive = true;

                session.Insert( person );

                SqlCmd command = SqlCmd.Current;
            }
        }
        #endregion

        #region Update
        [TestMethod]
        public void Update()
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                Person person = new Person();

                person.Id = 2;
                person.Age = 32;
                person.Money = 132.23m;
                person.CreateTime = DateTime.Now;

                int i = session.Update( person );

                Assert.AreEqual<int>( 1, i );
            }
        }
        #endregion

        #region Delete
        [TestMethod]
        public void Delete()
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                Person person = new Person();
                
                person.Id = 2;

                int i = session.Delete( person );

                Assert.AreEqual<int>( 1, i );
            }
        }
        #endregion

        #region UpdateT
        [TestMethod]
        public void UpdateT()
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                Person person = new Person();

                person.Age = 23;
                person.CreateTime = DateTime.Now;

                int i = session.Update<Person>()
                    .Set( person )
                    .Where( s => s.Id == 3 )
                    .Execute();

                Assert.AreNotEqual<int>( 0, i );
            }
        }
        #endregion

        #region DeleteT
        [TestMethod]
        public void DeleteT()
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                int i = session.Delete<Person>()
                    .Where( s => s.Id > 100000 )
                    .Execute();

                Assert.AreNotEqual<int>( 0, i );
            }
        }
        #endregion

        #region FindT
        [TestMethod]
        public void FindT()
        {
            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                using( DbTransaction transaction = session.BeginTransaction() )
                {
                    transaction.Commit();
                }
                var queryable = session.Find<Person>().Where( s => s.Id == 7 )
                    .OrderBy( s => s.CreateTime )
                    .Select( s => new { Id = s.Id, CreateTime = s.CreateTime } );

                var persons = queryable.ToList();

                Assert.AreNotEqual<int>( 0, persons.Count );
            }
        }
        #endregion
    }
}
