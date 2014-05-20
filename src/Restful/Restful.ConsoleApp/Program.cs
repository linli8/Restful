using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using MySql.Data.MySqlClient;
using Restful.Data;
using Restful.Data.Entity;
using Restful.Data.MySql;
using Restful.Extensions;
using Restful.Data.Dapper;

namespace Restful.ConsoleApp
{
    class Program
    {
        static void Main( string[] args )
        {
            SessionFactories.Register<MySqlSessionFactory>();

			using (ISession session = SessionFactory.CreateDefaultSession())
			{
				var person = new Person(){ Name = "test", Age = 20, Money = 100, CreateTime = DateTime.Now, IsActive = true };

				int i = session.Insert(person);

				int id = session.GetIndentifer<int>();

				person = session.Find<Person>().Where(s => s.Id == id).Single();
			}

            Console.ReadLine();
        }
    }
}
