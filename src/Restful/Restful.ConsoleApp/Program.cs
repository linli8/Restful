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

            ExecuteRestful();

            ExecuteDapper();


            Console.ReadLine();
        }

        private static void ExecuteRestful()
        {
            Stopwatch watch = new Stopwatch();

            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                for( int i = 0; i < 5; i++ )
                {
                    watch.Start();

                    var queryable = session.Find<Person>( string.Format( "select * from Person where Id > {0}", i ) );

                    var persons = queryable.ToList();
                    //var queryable = from s in session.Find<Person>()
                    //                select new { Id = s.Id, Name = s.Name };

                    //var persons = queryable.ToList();

                    watch.Stop();

                    Console.WriteLine( "restful:" + watch.ElapsedMilliseconds );

                    watch.Reset();
                }
            }
        }

        private static void ExecuteDapper()
        {
            Stopwatch watch = new Stopwatch();

            string connectionStr = ConfigurationManager.ConnectionStrings[0].ConnectionString;

            using( IDbConnection connection = new MySqlConnection( connectionStr ) )
            {
                for( int i = 0; i < 5; i++ )
                {
                    watch.Start();

                    var queryable = connection.Query<Dapper.Person>( string.Format( "select * from Person where Id > {0}", i ) );

                    var persons = queryable.ToList();

                    watch.Stop();

                    Console.WriteLine( "Dapper:" + watch.ElapsedMilliseconds );

                    watch.Reset();
                }
            }
        }
    }
}
