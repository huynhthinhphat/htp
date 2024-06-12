using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_QLNH.Models;
using System.Diagnostics;

namespace MVC_QLNH.Controllers
{
    public class HomeController : Controller
    {
        //Đây là khai báo của một biến db kiểu SqlMvcQlnhPerfactContext.
        //Biến này được sử dụng để truy cập và tương tác với cơ sở dữ liệu.
        //Dùng private readonly để đảm bảo rằng biến chỉ được gán giá trị một lần và không thể thay đổi sau khi đã được gán.
        private readonly LatMvcQlnhContext db;

        //Tương tự như trên, đây là khai báo của một biến _logger kiểu ILogger<HomeController>.
        //ILogger là một giao diện được sử dụng để ghi logs trong ứng dụng.
        //Trong trường hợp này, bạn đang sử dụng ILogger để ghi logs cho HomeController.
        private readonly ILogger<HomeController> _logger;

        //Tham số logger được sử dụng để inject một instance của ILogger vào HomeController
        //trong khi _db là một instance của SqlMvcQlnhPerfactContext được inject để sử dụng trong HomeController.
        public HomeController(ILogger<HomeController> logger, LatMvcQlnhContext _db)
        {
            _logger = logger;
            db = _db;
        }
        public async Task<IActionResult> Index()
        {
            //ngày hiện tại
            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow.Date);

            // Tổng tiền
            var data_totalAmount = db.TbBillHistories
                .Where(res => res.BillDate == today)
                .Sum(res => res.TotalAmount);

            // Tính tổng số lượng khách cho ngày hiện tại
            int data_totalCustom = db.TbBillHistories
                .Count(res => res.BillDate == today);

            //tổng bàn trống
            var data_totalTableEmpty = db.TbDstables.Count(res => res.Status == "Trống");

            //in id food bán chạy nhất trong ngày
            var data_nameFoodBestSell = db.TbBillHistories
             .GroupBy(res => res.FoodName)
             .Select(group => new
             {
                 FoodName = group.Key,
                 TotalQuantity = group.Sum(item => item.Quantity)
             })
             .OrderByDescending(item => item.TotalQuantity)
             .Select(item => item.FoodName)
             .FirstOrDefault();

            // Số lần xuất hiện của sản phẩm đó
            int occurrencesOfProduct = db.TbBillHistories
                .Where(res => res.BillDate == today && res.FoodName == data_nameFoodBestSell)
                .Sum(res => res.Quantity);

            // Tổng số lần xuất hiện của tất cả các sản phẩm/ tính theo số lượng sản phẩm đã bán
            int totalOccurrences = db.TbBillHistories
                .Where(res => res.BillDate == today && res.FoodName != null)
                .Sum(res => res.Quantity);

            // Tính phần trăm
            double percentage = (double)occurrencesOfProduct / totalOccurrences * 100;

            //in thông tin số tiền 5 bàn thanh toán gần nhất
            var data_detailTable = db.TbBillHistories
                .OrderByDescending(res => res.BillId) // Sắp xếp giảm dần theo thời gian
                .Take(5) // Chỉ lấy 5 phần tử đầu tiên
                .ToList(); // Chuyển kết quả thành danh sách

            //biểu đồ doanh thu
            var monthlyTotalList = db.TbBillHistories
                .GroupBy(x => new { x.BillDate.Year, x.BillDate.Month, x.BillDate.Day }) // Nhóm theo năm và tháng
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Day = g.Key.Day,
                    TotalAmount = g.Sum(x => x.TotalAmount) // Tính tổng tiền từng tháng
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .Select(x => new MonthlyTotalItem { Year = x.Year, Month = x.Month, Day = x.Day, TotalAmount = x.TotalAmount })
                .ToList();

            if (data_nameFoodBestSell == null)
            {
                data_nameFoodBestSell = null;
            }
            if (data_detailTable == null)
            {
                data_detailTable = null;
            }

            var viewModel = new MyViewModel
            {
                totalAmount = data_totalAmount,
                totalCustom = data_totalCustom,
                totalTableEmpty = data_totalTableEmpty,
                nameFoodBestSell = data_nameFoodBestSell,
                percentage = percentage,
                detailTable = data_detailTable,
                monthlyTotal = monthlyTotalList
            };
             ViewBag.ViewModel = viewModel;
            return View();
        }

        public class MonthlyTotalItem
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public int Day { get; set; }
            public double TotalAmount { get; set; }
        }
        public class MyViewModel
        {
            public int totalAmount { get; set; }
            public int totalCustom { get; set; }
            public double totalTableEmpty { get; set; }
            public string nameFoodBestSell { get; set; }
            public double percentage { get; set; }
            public List<TbBillHistory> detailTable { get; set; }
            public List<MonthlyTotalItem> monthlyTotal { get; set; }
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
