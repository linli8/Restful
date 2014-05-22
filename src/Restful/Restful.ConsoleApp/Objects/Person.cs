using System;
using Restful.Data.Attributes;

namespace Restful.ConsoleApp
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
}

