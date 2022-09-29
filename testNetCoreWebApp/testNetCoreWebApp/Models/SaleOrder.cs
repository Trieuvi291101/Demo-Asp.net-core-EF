using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace testNetCoreWebApp.Models
{
    public partial class SaleOrder
    {
        public SaleOrder()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public decimal? Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
