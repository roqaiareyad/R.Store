using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class PaginationResponse<TEntity>
    {
        public PaginationResponse(int pageindex, int pageSize, int totaCount, IEnumerable<TEntity> data)
        {
            Pageindex = pageindex;
            PageSize = pageSize;
            TotaCount = totaCount;
            Data = data;
        }

        public int Pageindex { get; set; }
        public int PageSize { get; set; }
        public int TotaCount { get; set; }
        public IEnumerable<TEntity> Data { get; set; }
    }
}
