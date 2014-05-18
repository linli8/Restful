using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Restful.Data;
using Restful.Data.Entity;
using Restful.Data.MySql;
using Restful.Extensions;

namespace Restful.ConsoleApp
{
    class Program
    {
        static void Main( string[] args )
        {
            SessionFactories.Register<MySqlSessionFactory>();

            Stopwatch watch = new Stopwatch();

            using( ISession session = SessionFactory.CreateDefaultSession() )
            {
                for( int i = 0; i < 10; i++ )
                {
                    watch.Start();

                    var queryable = session.Find<Person>().Where( s => s.Id > 0 )
                        .OrderBy( s => s.CreateTime )
                        .Select( s => new { Id = s.Id, CreateTime = s.CreateTime } );

                    var persons = queryable.ToList();

                    watch.Stop();

                    Console.WriteLine( watch.Elapsed );

                    watch.Reset();
                }
            }

            Console.ReadLine();
        }
    }
}
