using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Restful.Data;
using Restful.Data.Entity;
using Restful.Data.MySql;
using Restful.Extensions;
using Restful.Data.Dapper;
using System.Reflection.Emit;
using Restful.Reflection.Emit;
using Restful.Data.Common;

namespace Restful.ConsoleApp
{
    class Program
    {
        static void Main( string[] args )
        {
            SessionFactories.Register<MySqlSessionFactory>();

            string sql01 = "select * from Person where Id = ? and Name = ?";
            string sql02 = "select * from Person where Id = ? and Name = ?;";

            var parts01 = sql01.Split('?');
            var parts02 = sql02.Split('?');

            var person = new Person();

            person.Name = "test";
            person.Money = 100;
            person.CreateTime = DateTime.Now;
            person.IsActive = false;


            Stopwatch watch = new Stopwatch();

            for (int i = 0; i < 10; i++)
            {
                watch.Start();

                var handler = DynamicHelper.CreateDynamicPropertySetHandler(person.GetType().GetProperty("Age"));

                handler(person, 20);

                var proxy = EntityProxyGenerator.CreateProxy<Person>(person);

                watch.Stop();

                Console.WriteLine(watch.Elapsed);

                watch.Reset();
            }



//            using (ISession session = SessionFactory.CreateDefaultSession())
//            {
//                int i = session.Insert(person);
//
//                Console.WriteLine(SqlCmd.Current.Sql);
//
//                int id = session.GetIndentifer<int>();
//            }

            Console.ReadLine();
        }
    }
}
