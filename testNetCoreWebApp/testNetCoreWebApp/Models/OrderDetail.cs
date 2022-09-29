using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace testNetCoreWebApp.Models
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal? UnitPrice { get; set; }
        public string Num { get; set; }

        public virtual SaleOrder Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
