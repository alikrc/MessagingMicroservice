using System.Collections.Generic;

namespace WebBffAggregator.ApiModels
{
    public class PaginatedItemsApiModel<TEntity> where TEntity : class
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long Count { get; set; }

        public List<TEntity> Data { get; set; } = new List<TEntity>();

        public PaginatedItemsApiModel(int pageIndex, int pageSize, long count, List<TEntity> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data ?? new List<TEntity>();
        }
    }
}
