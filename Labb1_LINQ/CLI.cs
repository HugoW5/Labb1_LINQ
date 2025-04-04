using Labb1_LINQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb1_LINQ
{
	public class CLI
	{
		private static List<Action> MenuItems = new() {
			Electronics, 
		};
		public static void HomeScreen()
		{
			MenuItems[0].Invoke();
		}

		private static void Electronics()
		{
			using (var context = new AppDbContext())
			{
				var electronics = context.Products
					.Where(p => p.Category.Name == "Electronics")
					.ToList();
				foreach (var product in electronics)
				{
					Console.WriteLine($"Product ID: {product.Id}, Name: {product.Name}, Price: {product.Price}");
				}
			}
		}
	}
}
