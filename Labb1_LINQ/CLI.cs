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
		private static void DisplayTable(IQueryable data)
		{
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
		}


	}
}
