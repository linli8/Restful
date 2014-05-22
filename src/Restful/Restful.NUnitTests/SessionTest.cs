using NUnit.Framework;
using System;
using System.Linq;
using Restful.Data;
using Restful.Data.Extensions;
using Restful.Data.Common;
using Restful.Data.MySql;
using Restful.Data.Entity;
using Restful.Data.Attributes;

namespace Restful.NUnitTests
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

    [TestFixture()]
    public class SessionTest
    {
        static SessionTest()
        {
            SessionFactories.Register<MySqlSessionFactory>();
        }

        #region 测试 ExecuteScalar
        /// <summary>
        /// 测试 ExecuteScalar
        /// </summary>
        [Test()]
        public void ExecuteScalar()
        {
            using (ISession session = SessionFactory.CreateDefaultSession())
            {
                string sql = "select count(*) from Person where Id = ? and CreateTime < ?";

                int count = session.ExecuteScalar<int>(sql, 500, DateTime.Now);

                Assert.AreEqual(1, count);
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
            var person = EntityProxyGenerator.CreateProxy<Person>();

            person.Name = "test";
            //person.Age = 20;
            person.Money = 100;
            person.CreateTime = DateTime.Now;
            person.IsActive = false;

            using (ISession session = SessionFactory.CreateDefaultSession())
            {
                int i = session.Insert(person);

                Console.WriteLine(SqlCmd.Current.Sql);

                int id = session.GetIndentifer<int>();

                Assert.AreEqual(1, i);
                Assert.Greater(id, 0);
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
            var person = EntityProxyGenerator.CreateProxy<Person>();

            person.Name = "test";
            person.Age = 20;
            person.Money = 100;
            person.CreateTime = DateTime.Now;
            person.IsActive = true;

            using (ISession session = SessionFactory.CreateDefaultSession())
            {
                int i = session.Insert(person);

                int id = session.GetIndentifer<int>();

                person = session.Find<Person>().Where(s => s.Id == id).Single();

                Console.WriteLine(SqlCmd.Current.Sql);

                person = person.ToEntityProxy<Person>();

                person.Name = "test01";
                person.Age = 31;
                person.Money = 200;

                i = session.Update(person);

                Console.WriteLine(SqlCmd.Current.Sql);

                person = session.Find<Person>().Where(s => s.Id == id).Single();

                Console.WriteLine(SqlCmd.Current.Sql);

                Assert.AreEqual(1, i);
                Assert.AreEqual("test01", person.Name);
                Assert.AreEqual(31, person.Age.Value);
                Assert.AreEqual(200, person.Money.Value);

                person = EntityProxyGenerator.CreateProxy<Person>();

                person.Name = "test";
                person.Age = 20;
                person.Money = 100;
                person.CreateTime = DateTime.Now;
                person.IsActive = true;

                i = session.Update<Person>().Set(person).Where(s => s.Id == id).Execute();

                Console.WriteLine(SqlCmd.Current.Sql);

                Assert.AreEqual(1, i);
                Assert.AreEqual("test", person.Name);
                Assert.AreEqual(20, person.Age.Value);
                Assert.AreEqual(100, person.Money.Value);
                Assert.AreEqual(true, person.IsActive);
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
            var person = EntityProxyGenerator.CreateProxy<Person>();

            person.Name = "test";
            person.Age = 20;
            person.Money = 100;
            person.CreateTime = DateTime.Now;
            person.IsActive = true;

            using (ISession session = SessionFactory.CreateDefaultSession())
            {
                int i = session.Insert(person);

                int id = session.GetIndentifer<int>();

                person = session.Find<Person>().Where(s => s.Id == id).Single();

                i = session.Delete(person);

                Console.WriteLine(SqlCmd.Current.Sql);

                var queryable = session.Find<Person>().Where(s => s.Id == id);

                Assert.AreEqual(1, i);
                Assert.AreEqual(0, queryable.Count());

                person = EntityProxyGenerator.CreateProxy<Person>();

                person.Name = "test";
                person.Age = 20;
                person.Money = 100;
                person.CreateTime = DateTime.Now;
                person.IsActive = true;

                i = session.Insert(person);

                id = session.GetIndentifer<int>();

                person = session.Find<Person>().Where(s => s.Id == id).Single();

                i = session.Delete<Person>().Where(s => s.Id == id).Execute();

                Console.WriteLine(SqlCmd.Current.Sql);

                Assert.AreEqual(1, i);
                Assert.AreEqual(0, queryable.Count());
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
            var person = EntityProxyGenerator.CreateProxy<Person>();

            person.Name = "test";
            person.Age = 20;
            person.Money = 100;
            person.CreateTime = DateTime.Now;
            person.IsActive = true;

            using (ISession session = SessionFactory.CreateDefaultSession())
            {
                session.Insert(person);

                int id = session.GetIndentifer<int>();

                var queryable = session.Find<Person>().Where(s => s.Id == id);

                // 测试 Single 函数
                person = queryable.Single();

                Console.WriteLine(SqlCmd.Current.Sql);

                Assert.AreEqual("test", person.Name);
                Assert.AreEqual(20, person.Age.Value);
                Assert.AreEqual(100, person.Money.Value);
                Assert.AreEqual(true, person.IsActive);

                // 测试 SingleOrDefault 函数
                person = queryable.SingleOrDefault();

                Console.WriteLine(SqlCmd.Current.Sql);

                Assert.AreEqual("test", person.Name);
                Assert.AreEqual(20, person.Age.Value);
                Assert.AreEqual(100, person.Money.Value);
                Assert.AreEqual(true, person.IsActive);

                // 测试 First 函数
                person = queryable.First();

                Console.WriteLine(SqlCmd.Current.Sql);

                Assert.AreEqual("test", person.Name);
                Assert.AreEqual(20, person.Age.Value);
                Assert.AreEqual(100, person.Money.Value);
                Assert.AreEqual(true, person.IsActive);

                // 测试 FirstOrDefault 函数
                person = queryable.FirstOrDefault();

                Console.WriteLine(SqlCmd.Current.Sql);

                Assert.AreEqual("test", person.Name);
                Assert.AreEqual(20, person.Age.Value);
                Assert.AreEqual(100, person.Money.Value);
                Assert.AreEqual(true, person.IsActive);

                // 测试 Count 函数
                Assert.AreEqual(1, queryable.Count());

                Console.WriteLine(SqlCmd.Current.Sql);

                // 测试 ToList 函数
                var list = queryable.ToList();

                Console.WriteLine(SqlCmd.Current.Sql);

                Assert.AreEqual(1, list.Count());

                // 测试包含多个 Where 条件
                queryable = session.Find<Person>()
                    .Where(s => s.Id == id)
                    .Where(s => s.Name.Contains("test"))
                    .Where(s => s.Age == 20 && s.IsActive == true)
                    .Skip(0).Take(1);

                Assert.AreEqual(1, queryable.Count());

                Console.WriteLine(SqlCmd.Current.Sql);
            }
        }
        #endregion
    }
}

