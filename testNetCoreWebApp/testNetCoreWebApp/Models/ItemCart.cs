using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace testNetCoreWebApp.Models
{
	public class ItemCart
	{
		public int idCart { get; set; }
		public string NameCart { get; set; }

		public string Image { get; set; }
		public decimal? priceCart { get; set; }
		public int quantity { get; set; }

		public double total => (double)(priceCart * quantity);

		[NotMapped]
		public IFormFile ThumbnailImage { get; set; }
	}
}
