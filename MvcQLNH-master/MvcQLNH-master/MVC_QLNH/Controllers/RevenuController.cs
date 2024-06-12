using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
                    latMvcQlnhContext = latMvcQlnhContext.Where(s => s.TableName == "Bàn " + search_tablename && search_fromdate <= s.BillDate && s.BillDate <= search_todate);
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
            ViewData["UserInfoId"] = new SelectList(_context.TbUserInfos, "UserInfoId", "UserInfoId", tbBillHistory.UserInfoId);

            return PartialView("Edit", tbBillHistory);
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
