using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Restful.Data;
using Restful.Data.Attributes;
using Restful.Data.Entity;

namespace Restful.UnitTest.Entities
{
    
    #region Person
    [Serializable]
    public class Person : EntityObject
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
