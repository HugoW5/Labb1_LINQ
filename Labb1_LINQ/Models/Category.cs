using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Labb1_LINQ.Models
{
	internal class Category
	{
		public int Id { get; set; }
		[Required]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;
		[MaxLength(500)]
		public string Description { get; set; } = string.Empty;
	}
}
