using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Controllers
{
    public class HoaDonBanHangController : Controller
    {
        // GET: HoaDonBanHang
        TOYSTORE_MODELEntities3 _db = new TOYSTORE_MODELEntities3();

        // GET: Order
        public ActionResult Index(string search, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 5;
            //if (!fromDate.HasValue)
            //    fromDate = DateTime.Now.Date;
            //if (!toDate.HasValue)
            //    toDate = fromDate.GetValueOrDefault(DateTime.Now.Date).Date.AddDays(1);
            //if (toDate < fromDate)
            //    toDate = fromDate.GetValueOrDefault(DateTime.Now.Date).Date.AddDays(1);

            //ViewBag.fromDate = fromDate;
            //ViewBag.toDate = toDate;

            if (search == null)
            {
                return View(_db.OrderDetails.OrderByDescending(s => s.IDOrder).ToList().ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return View(_db.OrderDetails.Where(s => s.Order.KhachHang.FullName.Contains(search) || s.Order.KhachHang.Email.Contains(search) || s.Order.Phone_Cus == search).ToList().ToPagedList(pageNumber, pageSize));
            }
        }
       
        public ActionResult Details(int id)
        {
            var order = _db.OrderDetails.Where(s => s.IDOrder == id).FirstOrDefault();
            var result = from m in _db.OrderDetails
                         where m.IDOrder == order.IDOrder
                         select m;
            return View(result.ToList());
        }

        public ActionResult CountGiaoThanhCong()
        {
            ViewData["Number"] = _db.Orders.Where(s => s.Status == "Giao hàng thành công").Count();
            return View();
        }
        public ActionResult SortHoaDon(DateTime? fromDate, DateTime? toDate)
        {

            if (!fromDate.HasValue)
                fromDate = DateTime.Now.Date;
            if (!toDate.HasValue)
                toDate = fromDate.GetValueOrDefault(DateTime.Now.Date).Date.AddDays(1);
            if (toDate < fromDate)
                toDate = fromDate.GetValueOrDefault(DateTime.Now.Date).Date.AddDays(1);

            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;

            var day = _db.Orders
                      .Where(c => c.NgayDat >= fromDate && c.NgayDat < toDate)
                      .ToList();
            return View(day);
        }
    }
}