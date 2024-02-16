using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TodoListApp.Models;

namespace TodoListApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ApplicationDbContext _context;

		public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
		{
			_logger = logger;
			_context = context;
		}

		public IActionResult Index()
		{
			var Todos = _context.ToDoItems.ToList();

			TodoModel TodoModel = new TodoModel()
			{
				IsComplete = true,
				Name = "test"
			};
			_context.ToDoItems.Add(TodoModel);
			_context.SaveChanges();
			List<TodoModel> TodoModels = new List<TodoModel>();
			for (int i = 0; i < 3; i++)
			{
				TodoModel todoModel = new TodoModel() { 
					Id=i,
					IsComplete=true,
					Name="test"
				};
				TodoModels.Add(todoModel);

			}
			return View(TodoModels);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
