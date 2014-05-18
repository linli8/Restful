Restful.Data - 轻量级数据持久层组件
=================

1、什么是Restful.Data？
  Restful.Data是一套通用的轻量级数据持久层组件，除封装了ADO.NET基本的数据库操作以外，也提供了一些orm相关的API，用户可以方便的定义实体类，并使用这些API对数据进行增删改查等操作。
  Restful.Data借鉴了业界如nhibernate、entity framework等知名的数据持久层组件，但从一开始设计的初衷就是为了让用户能快速的学习和使用，并写出更加简洁优雅的代码，所以摒弃了一些复杂的设计和功能，用户可以使用变通的方式或方法使用Restful.Data组件实现其目的。
  Restful.Data充分考虑了实体框架的执行效率问题，进行了反复的推敲和论证，尽可能的采用高效的设计方案来提高性能。
    
2、谁需要Restful.Data?
  敢于冒险、追求完美、勇于挑战并极具责任感的程序设计人员。
  
3、Restful.Data提供哪些功能？
  
  基本的ADO.NET操作：
    BeginTransaction
    ExecuteScalar
    ExecuteDataReader
    ExecuteDataTable
    ExecuteDataSet
    ExecutePageQuery
    ExecuteStoredProcedure
    
  ORM相关操作：
    Insert
    Updete
    Delete
    Find
  
4、如何使用Restful.Data?
  使用前的准备：
  下载Restful.dll、Restful.Data、Restful.Data.MySql、Remotion.Linq.dll、MySql.Data.dll，或者直接下载源代码进行编译并获取这5个dll，并在项目中引用这些dll。
  
  在 Web.config 或 App.config 中配置连接字符串，如下：
  <connectionStrings>
    <clear />
    <add name="MySql1" connectionString="server=192.168.1.101;database=Restful;user id=linli8;password=linli8" providerName="Restful.Data.MySql"/>
    <add name="MySql2" connectionString="server=192.168.1.102;database=Restful;user id=linli8;password=linli8" providerName="Restful.Data.MySql"/>
  </connectionStrings>
  
  注册提供程序工厂：
  SessionFactories.Register<MySqlSessionFactory>();
  提供程序工厂在一个Application中仅需注册一次。
  
  如何进行基本的数据库操作：
  
  using( ISession session = SessionFactory.CreateDefaultSession() )
  {
    string sql = "select * from Person";

    DataTable dt = session.ExecuteDataTable( sql );
  }
  
  CreateDefaultSession默认情况下根据配置文件中连接字符串节点的第一项创建数据库连接，你可以调用CreateSession进行指定，或者你也可以使用 SessionFactory.Default = "MySql2"指定默认连接。
  
  未防止 SQL 注入，你也使用呆参数方法：
  
  using( ISession session = SessionFactory.CreateDefaultSession() )
  {
    string sql = "select * from Person where Id = @Id;";
    
    IDictionary<string, object> parameters = new Dictionary<string, object>();
    
    parameters.Add( "@Id", 5 );

    DataTable dt = session.ExecuteDataTable( sql, parameters );
  }
  
  与此类似的还有ExecuteScalar、ExecuteDataReader、ExecuteDataTable、ExecuteDataSet等方法。
  
  如何进行分页查询：
  
  using( ISession session = SessionFactory.CreateDefaultSession() )
  {
      string sql = "select * from User where CreateTime < @CreateTime";

      IDictionary<string, object> parameters = new Dictionary<string, object>();

      parameters.Add( "@CreateTime", DateTime.Now );

      // 查询第2页，每页10条，并根据 CreateTime 字段降序排列
      PageQueryResult result = session.ExecutePageQuery( sql01, 2, 10, "CreateTime DESC", parameters );
  }
  
  如何进行数据新增：
  
  using( ISession session = SessionFactory.CreateDefaultSession() )
  {
    var person = new Person();
    
    // person.Id = 1; 若Id字段为自增类型，无需指定。
    person.Name = "test01";
    person.CreateTime = DateTime.Now;
    person.IsActive = true;
    
    int i = session.Insert( person );
  }
  
  如何进行数据更新：
  
  using( ISession session = SessionFactory.CreateDefaultSession() )
  {
    var person = new Person();
    
    person.Id = 1; 
    person.Name = "test01";
    person.CreateTime = DateTime.Now;
    person.IsActive = true;
    
    // 在调用此方法时，务必指定实例的主键值。
    int i = session.Update( person );
  }
  
  或者你也可以批量更新：
  
  using( ISession session = SessionFactory.CreateDefaultSession() )
  {
    var person = new Person();
    
    // person.Id = 1; 
    person.Name = "test01";
    person.CreateTime = DateTime.Now;
    person.IsActive = true;
    
    // 在调用此方法时，不需要指定主键值，且不会更新主键字段
    session.Update<Person>().Set( person ).Where( s => s.IsActive == false ).Execute();
  }
  
  如何进行数据删除：
  
  using( ISession session = SessionFactory.CreateDefaultSession() )
  {
    var person = new Person() { Id = 1 };
    
    // 在调用此方法时，需要指定主键值
    session.Delete( person );
  }
  
  或者你也可以批量删除：
  
  using( ISession session = SessionFactory.CreateDefaultSession() )
  {
    // 在调用此方法时，不需要指定主键值
    session.Delete<Person>().Where( s => s.IsActive == false ).Execute();
  }
  
  如何进行单表查询：
  
  using( ISession session = SessionFactory.CreateDefaultSession() )
  {
    var queryable = session.Find<Person>()
        .Where( s => s.Name.Contains("a") )
        .Where( s => s.CreateTime < DateTime.Now )
        .OrderBy( s => s.CreateTime )
        .Skip(5)
        .Take(10);
        
    var list = queryable.ToList();
    var count = queryable.Count();
    var first = queryable.FirstOrDefault();
    
    var queryable1 = from s in session.Find<Person>()
                where s => s.Name.Contains("a")
                orderby s.CreateTime descending
                select new { Id = s.Id, Name = s.Name };
    // ...
  }
  
  目前只支持对单表的LINQ查询，且为了降低复杂度，后期也并不打算支持多表查询，对函数的支持也有限，仅支持string类型的StartsWith、EndsWith、Contains、Equals、IsNullOrEmpty等方法，对于其他方法后期将会继续完善。
  
  如果你需要实现一个复杂的查询并将其转换成对象，你也可以这样：
  
  using( ISession session = SessionFactory.CreateDefaultSession() )
  {
    string sql = "...";
    
    T @object = session.Find<T>( sql );
  }
  
  如何支持事务处理：
  
  using( ISession session = SessionFactory.CreateDefaultSession() )
  {
    using( DbTransaction transaction = session.BeginTransaction() )
    {
        // ...
        // ...
        
        transaction.Commit();
    }
  }
  
  SessionHelper的使用：
  
  SessionHelper对session对象的方法进行了静态封装，如果你只是需要执行单条语句，并马上关闭连接，你可以使用 SessionHelper 类中提供的一些辅助方法。
  
5、如何定义实体类

    [Serializable]
    public class Person : EntityObject  // 需继承与 EntityObject 类
    {
        private int m_Id;
        private string m_Name;
        private int? m_Age;
        private decimal? m_Money;
        private DateTime m_CreateTime;
        private bool m_IsActive;


        [PrimaryKey, AutoIncrease]  // 如果是自增字段，标记为 AutoIncrease；如果是主键标记为 PrimaryKey
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
    
6、总结

  因作者时间关系，组件目前并非十分完善，测试工作也只简单的进行了一部分，但您可以完全放心的应用于商业项目中，如遇到问题，作者将尽可能的解决。后期还将持续优化，需要小伙伴们的支持和鼓励，若对这个开源项目感兴趣，请加QQ群：338570336。
