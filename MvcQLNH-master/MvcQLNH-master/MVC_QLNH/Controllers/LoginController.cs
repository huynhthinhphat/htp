using Microsoft.AspNetCore.Mvc;
using MVC_QLNH.Models;

namespace MVC_QLNH.Controllers
{
    public class LoginController : Controller
    {
        SqlMvcQlnhPerfactContext db = new SqlMvcQlnhPerfactContext();
        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Username")==null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "home");
            }
        }
        [HttpPost]
        public IActionResult Login(TbAccount tk)
        {
            if (HttpContext.Session.GetString("Username")==null)
            {
                var u = db.TbAccounts.Where(x => x.Username.Equals(tk.Username) && x.Password.Equals(tk.Password)).FirstOrDefault();
                if (u!=null)
                {
                    HttpContext.Session.SetString("Username", u.Username.ToString());
                    return RedirectToAction("index", "home");
                }
                // Login failed, return a specific view
                return View("LoginFailed");

            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
       
    }
}
