using InoxServer.SharedKernel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Application.Features.Products.Queries.GetProducts
{
    public class ProductQueryRequest : PaginationRequest
    {
        public string? Keyword { get; set; }
        public Guid? categoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsActive { get; set; }
    }
}
