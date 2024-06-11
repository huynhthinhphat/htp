using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MVC_QLNH.Models;
using X.PagedList;

namespace MVC_QLNH.Controllers
{
    public class RevenuController : Controller
    {
        private readonly SqlMvcQlnhPerfactContext _context;

        public RevenuController(SqlMvcQlnhPerfactContext context)
        {
            _context = context;
        }

        // GET: Revenu
        public async Task<IActionResult> Index(int? page, int? searchString_id, string searchOption, DateOnly? searchString_startTime, DateOnly? searchString_endTime)
        {
            var data = from s in _context.TbBillHistories.Include(t => t.Food).Include(t => t.Table).Include(t => t.UserInfo) select s;

            // Số dòng trên mỗi trang
            int pageSize = 2;

            // Trang hiện tại, nếu không có thì mặc định là trang 1
            int pageNumber = (page ?? 1);

            ViewData["CurrentFilter_id"] = searchString_id;
            ViewData["CurrentFilter_startTime"] = searchString_startTime;
            ViewData["CurrentFilter_endTime"] = searchString_endTime;

            if (searchString_id != null)
            {
                switch (searchOption)
                {
                    case "BillId":
                        // Lọc dữ liệu theo BillId
                        data = data.Where(s => s.BillId == searchString_id);
                        break;
                    case "TableID":
                        if (searchString_startTime.HasValue && searchString_endTime.HasValue)
                        {
                            // Lọc dữ liệu theo TableID và thời gian
                            data = data.Where(s => s.BillDate >= searchString_startTime.Value && s.BillDate <= searchString_endTime.Value && s.TableID == searchString_id);
                        }
                        else if (searchString_startTime.HasValue)
                        {
                            // Lọc dữ liệu theo TableID và thời gian bắt đầu
                            data = data.Where(s => s.BillDate >= searchString_startTime.Value && s.TableID == searchString_id);
                        }
                        else if (searchString_endTime.HasValue)
                        {
                            // Lọc dữ liệu theo TableID và thời gian kết thúc
                            data = data.Where(s => s.BillDate <= searchString_endTime.Value && s.TableID == searchString_id);
                        }
                        else
                        {
                            // Lọc dữ liệu chỉ theo TableID
                            data = data.Where(s => s.TableID == searchString_id);
                        }
                        break;
                }
            }
            else
            {
                if (searchString_startTime.HasValue && searchString_endTime.HasValue)
                {
                    // Lọc dữ liệu theo TableID và thời gian
                    data = data.Where(s => s.BillDate >= searchString_startTime.Value && s.BillDate <= searchString_endTime.Value);
                }
                else if (searchString_startTime.HasValue)
                {
                    // Lọc dữ liệu theo TableID và thời gian bắt đầu
                    data = data.Where(s => s.BillDate >= searchString_startTime.Value);
                }
                else if (searchString_endTime.HasValue)
                {
                    // Lọc dữ liệu theo TableID và thời gian kết thúc
                    data = data.Where(s => s.BillDate <= searchString_endTime.Value);
                }
            }

            // Phân trang dữ liệu và trả về view
            var pagedData = await data.AsNoTracking().ToPagedListAsync(pageNumber, pageSize);
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
                .Include(t => t.Food)
                .Include(t => t.Table)
                .Include(t => t.UserInfo)
                .FirstOrDefaultAsync(m => m.BillId == id);
            if (tbBillHistory == null)
            {
                return NotFound();
            }

            return View(tbBillHistory);
        }

        // GET: Revenu/Create
        /*public IActionResult Create()
        {
            ViewData["FoodId"] = new SelectList(_context.TbFoods, "FoodId", "FoodId");
            ViewData["TableID"] = new SelectList(_context.TbDstables, "TableId", "TableId");
            ViewData["UserInfoId"] = new SelectList(_context.TbUserInfos, "UserInfoId", "UserInfoId");
            return View();
        }*/

        // POST: Revenu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BillId,TableID,UserInfoId,FoodId,Quantity,Price,TotalPrice,BillDate")] TbBillHistory tbBillHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tbBillHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FoodId"] = new SelectList(_context.TbFoods, "FoodId", "FoodId", tbBillHistory.FoodId);
            ViewData["TableID"] = new SelectList(_context.TbDstables, "TableId", "TableId", tbBillHistory.TableID);
            ViewData["UserInfoId"] = new SelectList(_context.TbUserInfos, "UserInfoId", "UserInfoId", tbBillHistory.UserInfoId);
            return View(tbBillHistory);
        }*/

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
            ViewData["FoodId"] = new SelectList(_context.TbFoods, "FoodId", "FoodName", tbBillHistory.FoodId);
            ViewData["TableID"] = new SelectList(_context.TbDstables, "TableId", "TableId", tbBillHistory.TableID);
            ViewData["UserInfoId"] = new SelectList(_context.TbUserInfos, "UserInfoId", "FullName", tbBillHistory.UserInfoId);
            return View(tbBillHistory);
        }

        // POST: Revenu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BillId,TableID,UserInfoId,FoodId,Quantity,Price,TotalPrice,BillDate")] TbBillHistory tbBillHistory)
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
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbBillHistoryExists(tbBillHistory.BillId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FoodId"] = new SelectList(_context.TbFoods, "FoodId", "FoodId", tbBillHistory.FoodId);
            ViewData["TableID"] = new SelectList(_context.TbDstables, "TableId", "TableId", tbBillHistory.TableID);
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
                .Include(t => t.Food)
                .Include(t => t.Table)
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
