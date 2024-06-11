using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_QLNH.Models;
using MVC_QLNH.ViewModels;
using X.PagedList;
using static MVC_QLNH.Controllers.ThucDonController;
namespace MVC_QLNH.Controllers
{
    public class ThucDonController : Controller
    {

        SqlMvcQlnhPerfactContext db = new SqlMvcQlnhPerfactContext();
        
        public IActionResult Index(int? page)
        {
            int pageSize = 9;
            int pageNumber = (page ?? 1);

            var foods = db.TbFoods.ToPagedList(pageNumber, pageSize);
            return View(foods);
        }
    }
}

