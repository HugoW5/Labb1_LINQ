using Labb1_LINQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Labb1_LINQ
{
	public class CLI
	{
		private static List<Action> MenuItems = new() {
			Electronics,
			SuppliersStockUnder10,
			TotalOrderValueLastMonth,
			BestSoldProducts,
			CategoriesAndProductCount,
			OrderOver1000
		};
		public static void HomeScreen()
		{
			while (true)
			{
				Console.WriteLine("1. Hämta alla produkter i kategorin \"Electronics\" och sortera dem efter pris (högst först)");
				Console.WriteLine("2. Lista alla leverantörer som har produkter med ett lagersaldo under 10 enheter");
				Console.WriteLine("3. Beräkna det totala ordervärdet för alla ordrar gjorda under den senaste månaden");
				Console.WriteLine("4. Hitta de 3 mest sålda produkterna baserat på OrderDetail-data");
				Console.WriteLine("5. Lista alla kategorier och antalet produkter i varje kategori");
				Console.WriteLine("6. Hämta alla ordrar med tillhörande kunduppgifter och orderdetaljer där totalbeloppet överstiger 1000 kr");
				Console.Write("Val: ");
				if (int.TryParse(Console.ReadLine(), out int result))
				{
					if (result >= 1 && result <= MenuItems.Count)
					{
						MenuItems[result - 1].Invoke();
						Console.Clear();
					}
					else
					{
						Console.WriteLine("Ogiltigt val, försök igen.");
					}
				}
				else
				{
					Console.WriteLine("Ogiltigt val, försök igen.");
				}
			}
		}
		private static void Electronics()
		{
			using (var context = new AppDbContext())
			{
				var electronics = from p in context.Products
								  where p.Category.Name == "Electronics"
								  orderby p.Price descending
								  select new
								  {
									  p.Name,
									  p.Price
								  };
				DisplayTable(electronics);
			}
		}
		private static void SuppliersStockUnder10()
		{
			using (var context = new AppDbContext())
			{
				var suppliers = from s in context.Suppliers
								join p in context.Products on s.Id equals p.SupplierId
								where p.StockQuantity < 10
								select s;
				DisplayTable(suppliers.Distinct());
			}
		}
		private static void TotalOrderValueLastMonth()
		{
			using (var context = new AppDbContext())
			{
				var lastMonth = DateTime.Now.AddMonths(-1);
				var totalOrderValue = (from o in context.Orders
									   where o.OrderDate >= lastMonth
									   select o.TotalAmount).Sum();
				Console.WriteLine($"Totalt ordervärde för senaste månaden: {totalOrderValue} kr");
				Console.WriteLine("Tryck retur för att återgå hem");
				Console.ReadLine();
			}
		}
		private static void BestSoldProducts()
		{
			using (var context = new AppDbContext())
			{
				var bestSoldProducts = (from od in context.OrderDetails
										group od by od.Product into g
										orderby g.Sum(od => od.Quantity) descending
										select new
										{
											ProductName = g.Key.Name,
											TotalSold = g.Sum(od => od.Quantity)
										}).Take(3);
				DisplayTable(bestSoldProducts);

				/*
				 select TOP(3) ProductId,
Name,
SUM(Quantity) as 'total' from OrderDetails 
join Products on ProductId = Products.Id
Group by ProductId, name
Order by total DESC
				 */
			}
		}
		private static void CategoriesAndProductCount()
		{
			using (var context = new AppDbContext())
			{
				var categories = from c in context.Categories
								 select new
								 {
									 c.Name,
									 ProductCount = context.Products.Count(p => p.CategoryId == c.Id)
								 };
				DisplayTable(categories);
			}
		}
		private static void OrderOver1000()
		{
			using (var context = new AppDbContext())
			{
				var orders = from o in context.Orders
							 join c in context.Customers on o.CustomerId equals c.Id
							 join od in context.OrderDetails on o.Id equals od.OrderId
							 where o.TotalAmount > 1000
							 select new
							 {
								 o.Id,
								 c.Name,
								 c.Email,
								 c.Phone,
								 c.Address,
								 o.OrderDate,
								 o.TotalAmount
							 };
				DisplayTable(orders);
			}
		}
		private static void DisplayTable(IQueryable data)
		{
			Console.Clear();
			if (data == null) return;
			Console.Write("\x1B[4m");
			var properties = data.ElementType.GetProperties();
			List<List<string>> tableData = new();

			List<string> headers = properties.Select(p => p.Name).ToList();
			tableData.Add(headers);

			foreach (var item in data)
			{
				List<string> row = properties
					.Select(p => p.GetValue(item)?.ToString() ?? string.Empty)
					.ToList();
				tableData.Add(row);
			}

			var columnWidths = new int[properties.Length];
			for (int col = 0; col < properties.Length; col++)
			{
				columnWidths[col] = tableData.Max(row => row[col].Length);
			}
			Console.BackgroundColor = ConsoleColor.DarkBlue;
			for (int col = 0; col < headers.Count; col++)
			{
				Console.Write(headers[col].PadRight(columnWidths[col] + 1));
			}
			Console.BackgroundColor = ConsoleColor.Black;
			Console.Write("\x1B[4m\n");

			foreach (var row in tableData.Skip(1)) 
			{
				for (int col = 0; col < row.Count; col++)
				{
					Console.Write(row[col].PadRight(columnWidths[col] + 1));
				}
				Console.WriteLine();
			}
			Console.Write("\x1B[0m");
			Console.WriteLine("\nTryck retur för att återgå hem");
			Console.ReadLine();
		}

	}
}
