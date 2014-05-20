using NUnit.Framework;
using System;
using System.Linq;
using Restful.Data;
using Restful.Data.Common;
using Restful.Data.MySql;
using Restful.Data.Entity;
using Restful.Data.Attributes;

namespace Restful.NUnitTests
{
    #region Person
    public class Person : EntityObject
    {
        private int m_Id;
        private string m_Name;
        private int? m_Age;
        private decimal? m_Money;
        private DateTime m_CreateTime;
        private bool m_IsActive;


        [PrimaryKey, AutoIncrease]
        public int Id
        {
            get { return this.m_Id; }
            set { this.m_Id = value; this.OnPropertyChanged( "Id", value ); }
        }

        public string Name
        {
            get { return this.m_Name; }
            set { this.m_Name = value; this.OnPropertyChanged( "Name", value ); }
        }

        public int? Age
        {
            get { return this.m_Age; }
            set { this.m_Age = value; this.OnPropertyChanged( "Age", value ); }
        }

        public decimal? Money
        {
            get { return this.m_Money; }
            set { this.m_Money = value; this.OnPropertyChanged( "Money", value ); }
        }

        public DateTime CreateTime
        {
            get { return this.m_CreateTime; }
            set { this.m_CreateTime = value; this.OnPropertyChanged( "CreateTime", value ); }
        }

        public bool IsActive
        {
            get { return this.m_IsActive; }
            set { this.m_IsActive = value; this.OnPropertyChanged( "IsActive", value ); }
        }
    }
    #endregion

    [TestFixture()]
    public class SessionTest
    {
        static SessionTest()
        {
            SessionFactories.Register<MySqlSessionFactory>();
        }

        #region 测试数据新增
        /// <summary>
        /// 测试数据新增
        /// </summary>
        [Test()]
        public void Insert()
        {
            var person = new Person(){ Name = "test", Age = 20, Money = 100, CreateTime = DateTime.Now, IsActive = true };

            using (ISession session = SessionFactory.CreateDefaultSession())
            {
                int i = session.Insert(person);

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
            var person = new Person(){ Name = "test", Age = 20, Money = 100, CreateTime = DateTime.Now, IsActive = true };

            using (ISession session = SessionFactory.CreateDefaultSession())
            {
                int i = session.Insert(person);

                int id = session.GetIndentifer<int>();

                person = session.Find<Person>().Where(s => s.Id == id).Single();

                person.Name = "test01";
                person.Age = 31;
                person.Money = 200;
                person.IsActive = false;

                i = session.Update(person);

                person = session.Find<Person>().Where(s => s.Id == id).Single();

                Assert.AreEqual(1, i);
                Assert.AreEqual("test01", person.Name);
                Assert.AreEqual(31, person.Age.Value);
                Assert.AreEqual(200, person.Money.Value);
                Assert.AreEqual(false, person.IsActive);

                person = new Person(){ Name = "test", Age = 20, Money = 100, CreateTime = DateTime.Now, IsActive = true };

                i = session.Update<Person>().Set(person).Where(s => s.Id == id).Execute();

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
            var person = new Person(){ Name = "test", Age = 20, Money = 100, CreateTime = DateTime.Now, IsActive = true };

            using (ISession session = SessionFactory.CreateDefaultSession())
            {
                int i = session.Insert(person);

                int id = session.GetIndentifer<int>();

                person = session.Find<Person>().Where(s => s.Id == id).Single();

                i = session.Delete(person);

                var queryable = session.Find<Person>().Where(s => s.Id == id);

                Assert.AreEqual(1, i);
                Assert.AreEqual(0, queryable.Count());

                person = new Person(){ Name = "test", Age = 20, Money = 100, CreateTime = DateTime.Now, IsActive = true };

                i = session.Insert(person);

                id = session.GetIndentifer<int>();

                person = session.Find<Person>().Where(s => s.Id == id).Single();

                i = session.Delete<Person>().Where(s => s.Id == id).Execute();

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
            var person = new Person(){ Name = "test", Age = 20, Money = 100, CreateTime = DateTime.Now, IsActive = true };

            using (ISession session = SessionFactory.CreateDefaultSession())
            {
                session.Insert(person);

                int id = session.GetIndentifer<int>();

                var queryable = session.Find<Person>().Where(s => s.Id == id);

                // 测试 Single 函数
                person = queryable.Single();

                Assert.AreEqual("test", person.Name);
                Assert.AreEqual(20, person.Age.Value);
                Assert.AreEqual(100, person.Money.Value);
                Assert.AreEqual(true, person.IsActive);

                // 测试 SingleOrDefault 函数
                person = queryable.SingleOrDefault();

                Assert.AreEqual("test", person.Name);
                Assert.AreEqual(20, person.Age.Value);
                Assert.AreEqual(100, person.Money.Value);
                Assert.AreEqual(true, person.IsActive);

                // 测试 First 函数
                person = queryable.First();

                Assert.AreEqual("test", person.Name);
                Assert.AreEqual(20, person.Age.Value);
                Assert.AreEqual(100, person.Money.Value);
                Assert.AreEqual(true, person.IsActive);

                // 测试 FirstOrDefault 函数
                person = queryable.FirstOrDefault();

                Assert.AreEqual("test", person.Name);
                Assert.AreEqual(20, person.Age.Value);
                Assert.AreEqual(100, person.Money.Value);
                Assert.AreEqual(true, person.IsActive);

                // 测试 Count 函数
                Assert.AreEqual(1, queryable.Count());

                // 测试 ToList 函数
                var list = queryable.ToList();

                Assert.AreEqual(1, list.Count());

                // 测试包含多个 Where 条件
                queryable = session.Find<Person>()
                    .Where(s => s.Id == id)
                    .Where(s => s.Name == "test")
                    .Where(s => s.Age == 20 && s.IsActive == true);

                Assert.AreEqual(1, queryable.Count());

                Console.WriteLine(SqlCmd.Current.Sql);
            }
        }
        #endregion
    }
}

