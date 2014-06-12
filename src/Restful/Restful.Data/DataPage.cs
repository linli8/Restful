using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Restful.Data
{
    /// <summary>
    /// 分页查询数据
    /// </summary>
    public class DataPage
    {
        #region Members

        private int m_PageIndex = 1;
        private int m_PageSize = 20;
        private int m_ItemCount = 0;

        #endregion

        #region Properties

        /// <summary>
        /// 获取或设置页索引，设置 PageIndex 时，值必须大于零。
        /// </summary>
        public int PageIndex
        {
            get
            {
                return this.m_PageIndex;
            }
            set
            {
                if( value <= 0 )
                {
                    throw new ArgumentOutOfRangeException( "PageIndex 的值必须大于零。" );
                }

                this.m_PageIndex = value;
            }
        }

        /// <summary>
        /// 获取或设置页大小，设置 PageSize 时，值必须大于零
        /// </summary>
        public int PageSize
        {
            get
            {
                return this.m_PageSize;
            }
            set
            {
                if( value <= 0 )
                {
                    throw new ArgumentOutOfRangeException( "PageSize 的值必须大于零。" );
                }

                this.m_PageSize = value;
            }
        }

        /// <summary>
        /// 获取或设置总条目数
        /// </summary>
        public int ItemCount
        {
            get
            {
                return this.m_ItemCount;
            }
            set
            {
                if( value < 0 )
                {
                    throw new ArgumentOutOfRangeException( "ItemCount 的值必须大于或等于零。" );
                }

                this.m_ItemCount = value;
            }
        }

        /// <summary>
        /// 获取总页数，当 ItemCount = 0 时，PageCount 默认为 1
        /// </summary>
        public int PageCount
        {
            get
            {
                if( this.m_ItemCount == 0 )
                    return 1;

                return ( this.m_ItemCount - 1 ) / this.PageSize + 1;
            }
        }

        /// <summary>
        /// 获取或设置分页查询结果数据
        /// </summary>
        /// <value>The data.</value>
        public DataTable Data { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        public DataPage( int pageIndex, int pageSize )
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }

        #endregion

    }
}
