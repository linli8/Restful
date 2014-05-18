using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Restful.Data;
using Restful.Data.Attributes;
using Restful.Data.Entity;

namespace Restful.ConsoleApp
{
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
}
