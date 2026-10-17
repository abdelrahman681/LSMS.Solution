using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Application.DTO.Common
{
    public class ProductParams
    {
        public int PageIndex { get; set; } = 1;

        private int pageSize = 5;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > 10 ? 10 : value; }
        }
    }
}
