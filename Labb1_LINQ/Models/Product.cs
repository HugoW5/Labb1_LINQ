using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb1_LINQ.Models
{
	internal class Product
	{
		public int Id { get; set; }
		[Required]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public decimal Price { get; set; }
		public int StockQuantity { get; set; }
		public int CategoryId { get; set; }
		public int SupplierId { get; set; }

		// Navigation properties
		public virtual Category Category { get; set; } = null!;
		public virtual Supplier Supplier { get; set; } = null!;
	}
}
