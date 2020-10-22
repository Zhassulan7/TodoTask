using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoTask.Models;

namespace TodoTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private Context _connection;

        public HomeController(ILogger<HomeController> logger, Context connection)
        {
            _logger = logger;
            _connection = connection;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddTask(string name)
        {
            _connection.Tasks.Add(new Models.Task (){ Name = name, IsCompleted = false });
            _connection.SaveChanges();
            return RedirectToAction("TasksList");
        }

        [HttpGet]
        public IActionResult TasksList(string filter = "All")
        {
            var tasks = _connection.Tasks.ToList();
            switch (filter)
            {
                case "Completed":
                    return PartialView(new TasksViewModel() { Tasks = tasks.Where(t=>t.IsCompleted).ToList(), Quantity = tasks.Count(), Mode = "Completed", ItemsLeft = tasks.Count(t=>!t.IsCompleted) });
                case "Active":
                    return PartialView(new TasksViewModel() { Tasks = tasks.Where(t => !t.IsCompleted).ToList(), Quantity = tasks.Count(), Mode = "Active", ItemsLeft = tasks.Count(t => !t.IsCompleted) });
                default:
                    return PartialView(new TasksViewModel() { Tasks = tasks.ToList(), Quantity = tasks.Count(), Mode = "All", ItemsLeft = tasks.Count(t => !t.IsCompleted) });
            }
            
        }

        [HttpPut]
        public IActionResult SetComletedStatus(int id)
        {
            var task = _connection.Tasks.FirstOrDefault(t => t.Id == id);
            switch (task.IsCompleted)
            {
                case true:
                    task.IsCompleted = false;
                    break;
                default:
                    task.IsCompleted = true;
                    break;
            }
            _connection.Tasks.Update(task);
            _connection.SaveChanges();
            return RedirectToAction("TasksList");
        }

        [HttpPost]
        public IActionResult CompletedAll()
        {
            _connection.Tasks.ToList().ForEach(t=>t.IsCompleted = true);
            _connection.SaveChanges();
            return RedirectToAction("TasksList");
        }

        [HttpPost]
        public IActionResult DeleteTask(int id)
        {
            _connection.Tasks.Remove(_connection.Tasks.FirstOrDefault(t => t.Id == id));
            _connection.SaveChanges();
            return RedirectToAction("TasksList");
        }

        [HttpPost]
        public IActionResult DeleteCompleted()
        {
            _connection.Tasks.RemoveRange(_connection.Tasks.Where(t=>t.IsCompleted));
            _connection.SaveChanges();
            return RedirectToAction("TasksList");
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
