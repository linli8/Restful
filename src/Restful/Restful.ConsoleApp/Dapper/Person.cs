using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restful.ConsoleApp.Dapper
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public decimal? Money { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsActive { get; set; }
    }
}
