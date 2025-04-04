using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Labb1_LINQ.Models
{
	internal class Supplier
	{
		public int Id { get; set; }
		[Required]
		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;
		public string ContactPerson { get; set; } = string.Empty;
		[EmailAddress]
		public string Email { get; set; } = string.Empty;
		public string Phone { get; set; } = string.Empty;

	}
}
