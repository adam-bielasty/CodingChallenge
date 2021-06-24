using System;
using System.Collections.Generic;
using X.PagedList;

namespace Paymentsense.Coding.Challenge.Api.Models
{
    public class PagedListResponse<T>
    {
        public IEnumerable<T> Items { get; set; }

        public PagedListMetaData MetaData { get; set; }
        
        public PagedListResponse()
        {
            Items = new List<T>();
            MetaData = null;
        }

        public PagedListResponse(IEnumerable<T> items, PagedListMetaData metaData)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            MetaData = metaData ?? throw new ArgumentNullException(nameof(metaData));
        }

        public PagedListResponse(IPagedList<T> pagedList)
        {
            Items = pagedList ?? throw new ArgumentNullException(nameof(pagedList));
            MetaData = pagedList.GetMetaData();
        }
    }
}