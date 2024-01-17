using Sys.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ODTradePromotion.API.Models.Paging
{
    public class PagedList<T> : List<T>
    {
        public MetaData MetaData { get; set; }
        public PagedList()
        {
            // Just come here in case Exception
        }
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            MetaData = new MetaData
            {
                TotalCount = count,// count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize)
            };
            AddRange(items);
        }
        public static PagedList<T> ToPagedList(List<T> source, int skip, int top)
        {
            if (top == 0) top = 99999;
            var count = source.Count();
            var items = source
              .Skip(skip)//((pageNumber - 1) * pageSize)
              .Take(top).ToList();
            return new PagedList<T>(items, count, (skip / top) + 1, top);
        }
    }
}
