using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb1_LINQ.Models
{
	internal class Order
	{
		public int Id { get; set; }
		public DateTime OrderDate { get; set; }
		public int CustomerId { get; set; }
		public decimal TotalAmount { get; set; }
		public string Status { get; set; } = string.Empty;

		// Navigation properties
		public virtual Customer Customer { get; set; } = null!;
		public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
	}
}
