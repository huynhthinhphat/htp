using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MVC_QLNH.Models;
using X.PagedList;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MVC_QLNH.Controllers
{
    public class RevenuController : Controller
    {
        private readonly LatMvcQlnhContext _context;

        public RevenuController(LatMvcQlnhContext context)
        {
            _context = context;
        }

        // GET: Revenu
        public async Task<IActionResult> Index(int? page, int? search_billid, string? search_tablename, DateOnly? search_fromdate, DateOnly? search_todate)
        {
            var latMvcQlnhContext = from s in _context.TbBillHistories.Include(t => t.UserInfo) select s;

            ViewData["search_billid"] = search_billid;
            ViewData["search_tablename"] = search_tablename;
            ViewData["search_fromdate"] = search_fromdate;
            ViewData["search_todate"] = search_todate;

            if (search_billid != null)
            {
                latMvcQlnhContext = latMvcQlnhContext.Where(s => s.BillId == search_billid);

            }else if (search_billid == null && search_tablename != null)
            {
                if (search_fromdate.HasValue && search_todate.HasValue)
                {
                    if (search_fromdate.Value > search_todate.Value)
                    {
                        latMvcQlnhContext = from s in _context.TbBillHistories.Include(t => t.UserInfo) select s;
                    }
                    else if (search_fromdate.Value <= search_todate.Value)
                    {
                        latMvcQlnhContext = latMvcQlnhContext.Where(s => s.TableName == "Bàn " + search_tablename && search_fromdate <= s.BillDate && s.BillDate <= search_todate);
                    }
                }
                else if (search_fromdate.HasValue && !search_todate.HasValue)
                {
                    latMvcQlnhContext = latMvcQlnhContext.Where(s => s.TableName == "Bàn " + search_tablename && search_fromdate <= s.BillDate);
                }
                else if (!search_fromdate.HasValue && search_todate.HasValue)
                {
                    latMvcQlnhContext = latMvcQlnhContext.Where(s => s.TableName == "Bàn " + search_tablename && s.BillDate <= search_todate);
                }else if (!search_fromdate.HasValue && !search_todate.HasValue)
                {
                    latMvcQlnhContext = latMvcQlnhContext.Where(s => s.TableName == "Bàn " + search_tablename);
                }
            }
            else if (search_billid == null && search_tablename == null)
            {
                latMvcQlnhContext = latMvcQlnhContext.Where(s => search_fromdate <= s.BillDate && s.BillDate <= search_todate);
            }


            if (search_billid == null && search_tablename == null && !search_fromdate.HasValue && !search_todate.HasValue)
            {
                latMvcQlnhContext = from s in _context.TbBillHistories.Include(t => t.UserInfo) select s;
            }

            // Số dòng trên mỗi trang
            int pageSize = 3;

            // Trang hiện tại, nếu không có thì mặc định là trang 1
            int pageNumber = (page ?? 1);


            // Phân trang dữ liệu và trả về view
            var pagedData = await latMvcQlnhContext.AsNoTracking().ToPagedListAsync(pageNumber, pageSize);
            return View(pagedData);
        }

        // GET: Revenu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbBillHistory = await _context.TbBillHistories
                .Include(t => t.UserInfo)
                .FirstOrDefaultAsync(m => m.BillId == id);
            if (tbBillHistory == null)
            {
                return NotFound();
            }

            return View(tbBillHistory);
        }

        // GET: Revenu/Create
        public IActionResult Create()
        {
            ViewData["UserInfoId"] = new SelectList(_context.TbUserInfos, "UserInfoId", "UserInfoId");
            return View();
        }

        // POST: Revenu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BillId,FoodName,Quantity,Price,TableName,BillDate,TotalAmount,UserInfoId,CustomerName,Sdt")] TbBillHistory tbBillHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbBillHistory);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["UserInfoId"] = new SelectList(_context.TbUserInfos, "UserInfoId", "UserInfoId", tbBillHistory.UserInfoId);
            return View(tbBillHistory);
        }

        // GET: Revenu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbBillHistory = await _context.TbBillHistories.FindAsync(id);
            if (tbBillHistory == null)
            {
                return NotFound();
            }
            ViewData["FullName"] = new SelectList(_context.TbUserInfos, "UserInfoId", "FullName", tbBillHistory.UserInfoId);
            ViewData["TableName"] = new SelectList(_context.TbDstables, "TableName", "TableName");

            TempData["Price"] = tbBillHistory.Price;
            TempData["Quantity"] = tbBillHistory.Quantity;

            return PartialView("Edit", tbBillHistory);
        }

        [HttpPost]
        public IActionResult GetPrice(string? foodname)
        {
            // Truy vấn giá tiền của món ăn dựa trên tên món ăn
            var price = _context.TbFoods.FirstOrDefault(f => f.FoodName == foodname)?.Price;

            return Json(price);
        }

        [HttpPost]
        public IActionResult GetFoodName(string? foodname)
        {
            // Kiểm tra xem tên món ăn có null hoặc rỗng không
            if (!string.IsNullOrEmpty(foodname))
            {
                // Tìm các tên món ăn gần giống với tên món ăn cụ thể
                var similarFoodNames = _context.TbFoods
                    .Where(f => EF.Functions.Like(f.FoodName, $"%{foodname}%"))
                    .Select(f => f.FoodName)
                    .ToList();

                return Json(similarFoodNames);
            }

            // Trả về null nếu tên món ăn là null hoặc rỗng
            return Json(null);
        }


        // POST: Revenu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BillId,FoodName,Quantity,Price,TableName,BillDate,TotalAmount,UserInfoId,CustomerName,Sdt")] TbBillHistory tbBillHistory)
        {
            if (id != tbBillHistory.BillId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbBillHistory);
                    await _context.SaveChangesAsync();

                    TempData["SaveSuccessMessage"] = "1";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbBillHistoryExists(tbBillHistory.BillId))
                    {
                        TempData["SaveSuccessMessage"] = "2";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["UserInfoId"] = new SelectList(_context.TbUserInfos, "UserInfoId", "UserInfoId", tbBillHistory.UserInfoId);
            return View(tbBillHistory);
        }

        // GET: Revenu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbBillHistory = await _context.TbBillHistories
                .Include(t => t.UserInfo)
                .FirstOrDefaultAsync(m => m.BillId == id);
            if (tbBillHistory == null)
            {
                return NotFound();
            }

            return View(tbBillHistory);
        }

        // POST: Revenu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbBillHistory = await _context.TbBillHistories.FindAsync(id);
            if (tbBillHistory != null)
            {
                _context.TbBillHistories.Remove(tbBillHistory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbBillHistoryExists(int id)
        {
            return _context.TbBillHistories.Any(e => e.BillId == id);
        }
    }
}
