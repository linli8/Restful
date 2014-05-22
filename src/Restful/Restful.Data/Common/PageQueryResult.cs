using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Restful.Data.Common
{
    /// <summary>
    /// 分页查询结果
    /// </summary>
    public class PageQueryResult
    {
        private int m_PageIndex;
        private int m_PageSize;
        private int m_ItemCount;

        public PageQueryResult( int pageIndex, int pageSize )
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }

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
                    throw new ArgumentOutOfRangeException( "ItemCount 的值必须大于零。" );
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
                if( this.m_ItemCount == 0 ) return 1;

                return ( this.m_ItemCount - 1 ) / this.PageSize + 1;
            }
        }

        /// <summary>
        /// 获取或设置查询结果数据集
        /// </summary>
        public DataTable Data { get; set; }
    }
}
