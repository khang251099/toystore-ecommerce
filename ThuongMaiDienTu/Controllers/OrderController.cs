using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThuongMaiDienTu.Models;
using PagedList.Mvc;
using System.Configuration;
namespace ThuongMaiDienTu.Controllers
{
    public class OrderController : Controller
    {
        TOYSTORE_MODELEntities3 _db = new TOYSTORE_MODELEntities3();

        // GET: Order
        public ActionResult Index(string search, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 5;
            if (search == null)
            {
                return View(_db.Orders.OrderByDescending(s => s.IDOrder).ToList().ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return View(_db.Orders.Where(s => s.KhachHang.FullName.Contains(search) || s.KhachHang.Email.Contains(search) || s.KhachHang.Phone_Cus == search).ToList().ToPagedList(pageNumber, pageSize));
            }
        }
        public ActionResult UnconfirmedToy(string search)
        {
            
           return View(_db.Orders.Where(s => s.Status == "Chưa xác nhận").OrderByDescending(s => s.NgayDat).ToList());
 
        }
        public ActionResult ConfirmedToy(string search)
        {
             return View(_db.Orders.Where(s => s.Status == "Đã xác nhận").OrderByDescending(s => s.NgayDat).ToList());
        }
        public ActionResult DangLayHangToy(string search)
        {
                return View(_db.Orders.Where(s => s.Status == "Đang lấy hàng").OrderByDescending(s => s.NgayDat).ToList()); 
        }
        public ActionResult DangGiaoToy(string search)
        {
             return View(_db.Orders.Where(s => s.Status == "Đang giao").OrderByDescending(s => s.NgayDat).ToList());
           
        }
        public ActionResult GiaoHangThanhCongToy(string search)
        {
           
                return View(_db.Orders.Where(s => s.Status == "Giao hàng thành công").OrderByDescending(s => s.NgayDat).ToList());
           
        }
        public ActionResult CancelToy(string search)
        {

            return View(_db.Orders.Where(s => s.Status == "Đã hủy").OrderByDescending(s => s.NgayDat).ToList());

        }
        
        public ActionResult Details(int id)
        {
            var order = _db.OrderDetails.Where(s => s.IDOrder == id).FirstOrDefault();
            var result = from m in _db.OrderDetails
                         where m.IDOrder == order.IDOrder
                         select m;
            return View(result.ToList());
        }

        // POST: Category/Delete/5
        public ActionResult Confirm(int id ,Product pro)
        {
            
                var order = _db.Orders.Where(s => s.IDOrder == id).FirstOrDefault();
                if(order.Status=="Chưa xác nhận")
                 {
                   order.Status = "Đã xác nhận";
                 }    
                
                _db.SaveChanges();
                return RedirectToAction("Index");
        }
        public ActionResult PickingGood(int id, Product pro)
        {
            var order = _db.Orders.Where(s => s.IDOrder == id).FirstOrDefault();
            if (order.Status == "Đã xác nhận")
            { 
                order.Status = "Đang lấy hàng"; 
            }
            _db.SaveChanges();
            return RedirectToAction("Index"); 
        }
        public ActionResult Delivering(int id, Product pro)
        {
            var order = _db.Orders.Where(s => s.IDOrder == id).FirstOrDefault();
            if (order.Status == "Đang lấy hàng")
            { order.Status = "Đang giao";
            }
          
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult SuccessfullyDelivery(int id, Product pro)
        {

            var order = _db.Orders.Where(s => s.IDOrder == id).FirstOrDefault();
            if (order.Status == "Đang giao")
            { order.Status = "Giao hàng thành công";
                
            }
            
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Unconfirm(int id,Product pro)
        {            
                var order = _db.Orders.Where(s => s.IDOrder == id).FirstOrDefault();
                order.Status = "Chưa xác nhận";
                _db.SaveChanges();
                return RedirectToAction("Index");    
        }
        public ActionResult Cancel(int id, Product pro)
        {
            var order = _db.Orders.Where(s => s.IDOrder == id).FirstOrDefault();
            if (order.Status == "Chưa xác nhận" || order.Status == "Đã xác nhận" || order.Status == "Đang giao")
            { order.Status = "Đã hủy"; }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult CountChuaXacNhan()
        {
            ViewData["Number"] = _db.Orders.Where(s => s.Status == "Chưa xác nhận").Count();
            return View();
        }
        public ActionResult CountDaXacNhan()
        {
            ViewData["Number"] = _db.Orders.Where(s => s.Status == "Đã xác nhận").Count();
            return View();
        }
        public ActionResult CountDangLayHang()
        {
            ViewData["Number"] = _db.Orders.Where(s => s.Status == "Đang lấy hàng").Count();
            return View();
        }
        public ActionResult CountDangGiao()
        {
            ViewData["Number"] = _db.Orders.Where(s => s.Status == "Đang giao").Count();
            return View();
        }
        public ActionResult CountGiaoThanhCong()
        {
            ViewData["Number"] = _db.Orders.Where(s => s.Status == "Giao hàng thành công").Count();
            return View();
        }
        public ActionResult Sort(string sortBy, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 5;
            if (sortBy == "Newest")
                return View(_db.Orders.OrderBy(m => m.IDOrder).ToList().ToPagedList(pageNumber, pageSize));
            else
                return View(_db.Orders.ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult SortGiaoHangThanhCong(DateTime? fromDate, DateTime? toDate)
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
                      .Where(c => c.NgayDat >= fromDate && c.NgayDat < toDate && c.Status=="Giao hàng thành công")
                      .ToList();
            return View(day);
        }
        public ActionResult SortChuaXacNhan(DateTime? fromDate, DateTime? toDate)
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
                      .Where(c => c.NgayDat >= fromDate && c.NgayDat < toDate && c.Status == "Chưa xác nhận")
                      .ToList();
            return View(day);
        }
        public ActionResult SortDaXacNhan(DateTime? fromDate, DateTime? toDate)
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
                      .Where(c => c.NgayDat >= fromDate && c.NgayDat < toDate && c.Status == "Đã xác nhận")
                      .ToList();
            return View(day);
        }
        public ActionResult SortDangLayHang(DateTime? fromDate, DateTime? toDate)
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
                      .Where(c => c.NgayDat >= fromDate && c.NgayDat < toDate && c.Status == "Đang lấy hàng")
                      .ToList();
            return View(day);
        }
        public ActionResult SortDangGiao(DateTime? fromDate, DateTime? toDate)
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
                      .Where(c => c.NgayDat >= fromDate && c.NgayDat < toDate && c.Status == "Đang giao")
                      .ToList();
            return View(day);
        }
        public ActionResult SortDaHuy(DateTime? fromDate, DateTime? toDate)
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
                      .Where(c => c.NgayDat >= fromDate && c.NgayDat < toDate && c.Status == "Đã hủy")
                      .ToList();
            return View(day);
        }

    }
}
