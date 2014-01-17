using System;
using System.Collections.Generic;

namespace NCodeBox.Domain
{
    public class PaginatedList<T>
    {
      public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPageCount { get; set; }
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 0);
            }
        }
        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPageCount-1);
            }
        }
        public IEnumerable<T> Result { get; set; }
        public PaginatedList()
        {
         
        }
        public PaginatedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            if (source == null) throw new ArgumentNullException("source");
            Result = source;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPageCount = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
