using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        private TOYSTORE_MODELEntities3 _db = new TOYSTORE_MODELEntities3();
        // GET: Contact
        public ActionResult Index(string search, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 5;
            if (search == null)
            {
                return View(_db.LienHes.ToList().ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return View(_db.LienHes.Where(s => s.KhachHang.FullName.Contains(search) ||s.KhachHang.Email.Contains(search)).ToList().ToPagedList(pageNumber, pageSize));
            }
        }

        public ActionResult ChuaTraLoiThu()
        {
            return View(_db.LienHes.Where(s => s.Status == "Chưa phản hồi").OrderByDescending(s => s.NgayLienHe).ToList());
        }
        public ActionResult Unconfirm(int id)
        {
            var lh = _db.LienHes.Where(s => s.IDLienHe == id).FirstOrDefault();
            lh.Status = "Chưa phản hồi";
            _db.SaveChanges();
            return RedirectToAction("Index");

        }
      
        public ActionResult PhanHoiThu()
        {
            return View(_db.LienHes.Where(s => s.Status == "Đã phản hồi").OrderByDescending(s => s.NgayLienHe).ToList());
        }
        public ActionResult ConfirmedMess(int id)
        {
            var lh = _db.LienHes.Where(s => s.IDLienHe == id).FirstOrDefault();
            if (lh.Status == "Chưa phản hồi")
            {
                lh.Status = "Đã phản hồi";
            }
            _db.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult Details(int id)
        {
            return View(_db.LienHes.Where(s => s.IDLienHe == id).FirstOrDefault());
        }
        public ActionResult Delete(int id)
        {
            if (Session["AdminID"] != null && Session["AdminRole"] != null && Session["AdminRole"].ToString() == "1")
                return View(_db.LienHes.Where(s => s.IDLienHe == id).FirstOrDefault());
            else
                return Content("Bạn không được quyền truy cập trang web này");
        }

        // POST: Category/Delete/5
        [HttpPost]
        public ActionResult DeleteConfirm(int id, LienHe lh)
        {
            try
            {
                // TODO: Add delete logic here
                lh = _db.LienHes.Where(x => x.IDLienHe == id).FirstOrDefault();
                _db.LienHes.Remove(lh);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Lỗi ràng buộc khóa ngoại");
            }
        }
        public ActionResult CountChuaPhanHoi()
        {
            ViewData["Number"] = _db.LienHes.Where(s => s.Status == "Chưa phản hồi").Count();
            return View();
        }
        public ActionResult CountDaPhanHoi()
        {
            ViewData["Number"] = _db.LienHes.Where(s => s.Status == "Đã phản hồi").Count();
            return View();
        }
    }
}