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
        TOYSTORE_MODELEntities7 _db = new TOYSTORE_MODELEntities7();

        // GET: Order
        public ActionResult Index(string search, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 5;
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
    }
}