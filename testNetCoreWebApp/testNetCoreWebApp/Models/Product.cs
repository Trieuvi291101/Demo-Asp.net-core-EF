using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace testNetCoreWebApp.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public string Manufacturer { get; set; }
        public string Image { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<OrderDetail> OrderDetail { get; set; }

        [NotMapped]
        public IFormFile ThumbnailImage { get; set; }

    }
}
