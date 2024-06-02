using JopPortal.Data;
using JopPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace JopPortal.Controllers
{
    public class HomeController : Controller
    {
        /*private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/

        private readonly AppDbContext context;
        public HomeController(AppDbContext context)
        {
            this.context = context;
        }



        public IActionResult Index()
        {

            var Email = HttpContext.Session.GetString("Email");
            ViewBag.EmailUser = Email;
            /*if(Email == null)
            {
                return RedirectToAction("LogIn", "LoginUser");
            }*/
            return View(new Search());
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

        public IActionResult Explore_Jobs()
        {
            var Email = HttpContext.Session.GetString("Email");
            ViewBag.EmailUser = Email;
            if (Email == null)
            {
                return RedirectToAction("LogIn", "LoginUser");
            }
            var jobs = context.jop.ToList();
            return View(jobs);
        }

        public IActionResult Filtering_Jobs()
        {
            return View(new Search());
        }

        public IActionResult Show_results(Search search)
        {
            var Email = HttpContext.Session.GetString("Email");
            ViewBag.EmailUser = Email;
            var jobs = context.jop.AsQueryable();

            if (search.Category != J_Category.Any)
            {
                jobs = jobs.Where(j => j.JobCategory == (JobCategory)search.Category);
            }
            if (search.Type != J_Type.Any)
            {
                jobs = jobs.Where(j => j.JobType == (JobType)search.Type);
            }
            if (search.Location != J_Location.Any)
            {
                jobs = jobs.Where(j => j.JopLocation == (Locations)search.Location);
            }
            if (search.Min_Salary > 0)
            {
                jobs = jobs.Where(j => j.JopSalary >= search.Min_Salary);
            }
            if (search.Max_Salary > 0)
            {
                jobs = jobs.Where(j => j.JopSalary <= search.Max_Salary);
            }

            return View(jobs.ToList());
        }



    }
}
