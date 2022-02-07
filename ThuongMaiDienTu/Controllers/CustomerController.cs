using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Controllers
{
    public class CustomerController : Controller
    {
        TOYSTORE_MODELEntities3 _db = new TOYSTORE_MODELEntities3();
        // GET: Customer
        public ActionResult Index(string search, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 5;
            if (search == null)
            {
                return View(_db.KhachHangs.ToList().ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return View(_db.KhachHangs.Where(s => s.FullName.Contains(search) || s.Email.Contains(search) || s.Phone_Cus.Contains(search)).ToList().ToPagedList(pageNumber, pageSize));
            }
        }
        public ActionResult Details(int id)
        {
            return View(_db.KhachHangs.Where(s => s.IDKhachHang == id).FirstOrDefault());
        }
        public ActionResult OrderHistory(int id)
        {
            return PartialView(_db.OrderDetails.Where(s => s.Order.KhachHang.IDKhachHang == id).ToList());
        }
        public ActionResult Edit(int id)
        {
            return View(_db.KhachHangs.Where(s => s.IDKhachHang == id).FirstOrDefault());
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, KhachHang _user)
        {
            try
            {
                // TODO: Add update logic here
                _db.Entry(_user).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Delete(int id)
        {
            if (Session["AdminID"] != null && Session["AdminRole"] != null && Session["AdminRole"].ToString() == "1")
                return View(_db.KhachHangs.Where(s => s.IDKhachHang == id).FirstOrDefault());
            else
                return Content("Bạn không được quyền truy cập trang web này");
        }

        // POST: Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, KhachHang _user)
        {
            try
            {
                // TODO: Add delete logic here
                _user = _db.KhachHangs.Where(x => x.IDKhachHang == id).FirstOrDefault();
                _db.KhachHangs.Remove(_user);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Lỗi ràng buộc khóa ngoại");
            }
        }
       

        //public ActionResult KhachHangVangLai(int id)
        //{
        //    if(Session["IDUser"]!=null)
        //    {
        //        return View(_db.KhachHangs.Where(s => s.IDKhachHang == id).FirstOrDefault());
        //    }    
        //    else
        //    {
        //        return View(_db.Orders.Where(s => s.IDKhachHang == id).FirstOrDefault());

        //    }    
        //    //return View(_db.Orders.Where(s => s.IDKhachHang != (int)Session["IDUser"]).FirstOrDefault());
        //}

    }
     
 }
