﻿using iText.StyledXmlParser.Jsoup.Nodes;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        TOYSTORE_MODELEntities7 db = new TOYSTORE_MODELEntities7();
        // GET: SignIn
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(KhachHang _user)
        {
            if (ModelState.IsValid)
            {
                var check = db.KhachHangs.FirstOrDefault(s => s.Email == _user.Email);
                if (check == null)
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    _user.PointKH = 0;
                    _user.RankID = 1;
                    db.KhachHangs.Add(_user);
                
                    db.SaveChanges();
                    return RedirectToAction("SignIn", "User");
                }
                else
                {
                    ViewBag.error = "Email này đã tồn tại, mời bạn nhập địa chỉ Email khác";
                    return View();
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(KhachHang _user)
        {
            var check = db.KhachHangs.Where(m => m.Email.Equals(_user.Email) &&
              m.PasswordKH.Equals(_user.PasswordKH)).FirstOrDefault();
            if (check == null)
            {
                _user.LogInErrorMessage = "Sai thông tin đăng nhập, vui lòng khách hàng nhập lại";
                return View("SignIn", _user);
            }
            else
            {

                Session["IDUser"] = check.IDKhachHang;
                Session["RankID"] = check.RankID;
                Session["Email"] = check.Email;
                Session["FullName"] = check.FullName;
                Session["PhoneNumber"] = check.Phone_Cus;
                Session["Address_Cus"] = check.DiaChi_Cus;
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Details()
        {
            string email = Session["Email"].ToString();
            int id = db.KhachHangs.Single(m => m.Email.Equals(email)).IDKhachHang;
            return View(db.KhachHangs.Where(s => s.IDKhachHang == id).FirstOrDefault());
        }
        public ActionResult LogOut()
        {
            int UserID = (int)Session["IDUser"];
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult OrderHistory()
        {
            string email = Session["Email"].ToString();
            int id = db.KhachHangs.Single(m => m.Email.Equals(email)).IDKhachHang;
            return PartialView(db.OrderDetails.Where(s => s.Order.IDKhachHang == id).ToList());
        }
        public ActionResult ChiTietDonHang(int id)
        {
            var order = db.OrderDetails.Where(s => s.IDOrder == id).FirstOrDefault();
            var result = from m in db.OrderDetails
                         where m.IDOrder == order.IDOrder
                         select m;
            return View(result.ToList());
        }



        public ActionResult LichSuMuaHang()
        {
            string email = Session["Email"].ToString();
            int id = db.KhachHangs.Single(m => m.Email.Equals(email)).IDKhachHang;
            return PartialView(db.OrderDetails.Where(s => s.Order.IDKhachHang == id && s.Order.Status=="Giao hàng thành công ").ToList());
        }
        public ActionResult ChuaXacNhan()
        {
            string email = Session["Email"].ToString();
            int id = db.KhachHangs.Single(m => m.Email.Equals(email)).IDKhachHang;
            return PartialView(db.OrderDetails.Where(s => s.Order.IDKhachHang == id && s.Order.Status=="Chưa xác nhận").ToList());
        }
        


        public ActionResult DaXacNhan()
        {
            string email = Session["Email"].ToString();
            int id = db.KhachHangs.Single(m => m.Email.Equals(email)).IDKhachHang;
            return PartialView(db.OrderDetails.Where(s => s.Order.IDKhachHang == id && s.Order.Status == "Đã xác nhận").ToList());
        }
        public ActionResult DangLayHang()
        {
            string email = Session["Email"].ToString();
            int id = db.KhachHangs.Single(m => m.Email.Equals(email)).IDKhachHang;
            return PartialView(db.OrderDetails.Where(s => s.Order.IDKhachHang == id && s.Order.Status == "Đang lấy hàng").ToList());
        }
        public ActionResult DangGiao()
        {
            string email = Session["Email"].ToString();
            int id = db.KhachHangs.Single(m => m.Email.Equals(email)).IDKhachHang;
            return PartialView(db.OrderDetails.Where(s => s.Order.IDKhachHang == id && s.Order.Status == "Đang giao").ToList());
        }
        public ActionResult GiaoHangThanhCong()
        {
            string email = Session["Email"].ToString();
            int id = db.KhachHangs.Single(m => m.Email.Equals(email)).IDKhachHang;
            return PartialView(db.OrderDetails.Where(s => s.Order.IDKhachHang == id && s.Order.Status == "Giao hàng thành công").ToList());
        }
        public ActionResult DaHuy()
        {
                
            string email = Session["Email"].ToString();
            int id = db.KhachHangs.Single(m => m.Email.Equals(email)).IDKhachHang;
            return PartialView(db.OrderDetails.Where(s => s.Order.IDKhachHang == id && s.Order.Status == "Đã hủy").ToList());    
        }
        public ActionResult UpdateInfo()
        {
            string email = Session["Email"].ToString();
            int id = db.KhachHangs.Single(m => m.Email.Equals(email)).IDKhachHang;
            return View(db.KhachHangs.Where(m => m.IDKhachHang == id).FirstOrDefault());
        }
        public ActionResult Delivering(int id, Product pro)
        {
            var order = db.Orders.Where(s => s.IDOrder == id).FirstOrDefault();
            if (order.Status == "Đang lấy hàng")
            {
                order.Status = "Đang giao";
            }
               
              
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult XacNhanGium(int id)
        {
            var order = db.Orders.Where(s => s.IDOrder == id).FirstOrDefault();
            if (order.Status == "Đang giao")
            {
                order.Status = "Giao hàng thành công";
                db.Entry(order).State = EntityState.Modified;
                order.StatusPayment = "Thanh toán thành công";
               
                db.SaveChanges();          
            }
            
            var user = db.KhachHangs.Where(s => s.IDKhachHang == order.IDKhachHang).FirstOrDefault();
            int point = Convert.ToInt32(order.TongTien * 0.01);

            var query2 = "UPDATE KhachHang SET PointKH = @diem WHERE IDKhachHang = @ma";
            db.Database.ExecuteSqlCommand(query2, new SqlParameter("@ma", user.IDKhachHang), new SqlParameter("@diem", user.PointKH + point));
            if (user.PointKH + point >= 20000)
            {
                var query1 = "UPDATE KhachHang SET RankID = 2 WHERE IDKhachHang = @ma";
                db.Database.ExecuteSqlCommand(query1, new SqlParameter("@ma", user.IDKhachHang));  
            }
            db.SaveChanges();
            Session["RankID"] = user.RankID;
            return RedirectToAction("GiaoHangThanhCong");
        }
        public ActionResult SuccessfullyDelivery(int id, Product pro)
        {

            var order = db.Orders.Where(s => s.IDOrder == id).FirstOrDefault();
            if (order.Status == "Đang giao")
            {
                order.Status = "Giao hàng thành công";
            }
              
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult XacNhanTheoDoi(int id)
        {
            var order = db.Orders.Where(s => s.IDOrder == id).FirstOrDefault();
            if (order.Status == "Đang giao")
            {
                order.Status = "Giao hàng thành công";
                db.Entry(order).State = EntityState.Modified;
                order.StatusPayment = "Thanh toán thành công";
            }
            db.SaveChanges();
            return RedirectToAction("TheoDoiDonHang");
        }
        public ActionResult CancelOrder(int id)
        {
            var order = db.Orders.Where(s => s.IDOrder == id).FirstOrDefault();
            if (order.Status == "Chưa xác nhận" || order.Status == "Đã xác nhận" || order.Status == "Đang giao")
            { order.Status = "Đã hủy";
                db.Entry(order).State = EntityState.Modified;
            }
            db.SaveChanges();
            return RedirectToAction("Details","User");
        }



        public ActionResult HuyBenTheoDoi(int id)
        {
            var order = db.Orders.Where(s => s.IDOrder == id).FirstOrDefault();
            if (order.Status == "Chưa xác nhận" || order.Status == "Đã xác nhận" || order.Status == "Đang giao")
            {
                order.Status = "Đã hủy";
                db.Entry(order).State = EntityState.Modified;
            }
            db.SaveChanges();
            return RedirectToAction("HuyRoiDoTroi", "User");
        }
        public ActionResult HuyRoiDoTroi()
        {
            return PartialView(db.OrderDetails.Where(s =>s.Order.Status == "Đã hủy").ToList());
        }
        [HttpPost]
        public ActionResult UpdateInfo(KhachHang _user)
        {
            try
            {
                // TODO: Add update logic here
                db.Entry(_user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "User");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult OrderDetails(int id)
        {
            var order = db.OrderDetails.Where(s => s.IDOrder == id).FirstOrDefault();
            var result = from r in db.OrderDetails
                         where r.IDOrder == order.IDOrder
                         select r;
            return View(result.ToList());
        }
        public ActionResult TheoDoiDonHang(string search)
        {           
                return View(db.Orders.Where(s => s.KhachHang.Phone_Cus.ToString() == search || s.Phone_Cus.ToString() == search || s.KhachHang.Email.Contains(search)).ToList());         
        }
        public ActionResult CountChuaXacNhan()
        {
            string email = Session["Email"].ToString();
            int id = db.KhachHangs.Single(m => m.Email.Equals(email)).IDKhachHang;
            ViewData["Number"] = db.Orders.Where(s => s.Status == "Chưa xác nhận" && s.IDKhachHang == id).Count();
            return View();
        }
        public ActionResult CountDaXacNhan()
        {
            string email = Session["Email"].ToString();
            int id = db.KhachHangs.Single(m => m.Email.Equals(email)).IDKhachHang;
            ViewData["Number"] = db.Orders.Where(s => s.Status == "Đã xác nhận" && s.IDKhachHang == id).Count();
            return View();
        }
        public ActionResult CountDangLayHang()
        {
            string email = Session["Email"].ToString();
            int id = db.KhachHangs.Single(m => m.Email.Equals(email)).IDKhachHang;
            ViewData["Number"] = db.Orders.Where(s => s.Status == "Đang lấy hàng" && s.IDKhachHang == id).Count();
            return View();
        }
      
        public ActionResult CountDangGiao()

        {
            string email = Session["Email"].ToString();
            int id = db.KhachHangs.Single(m => m.Email.Equals(email)).IDKhachHang;
            ViewData["Number"] = db.Orders.Where(s => s.Status == "Đang giao" && s.IDKhachHang==id).Count();
            return View();
        }
        public ActionResult CountGiaoThanhCong()
        {
            string email = Session["Email"].ToString();
            int id = db.KhachHangs.Single(m => m.Email.Equals(email)).IDKhachHang;
            ViewData["Number"] = db.Orders.Where(s => s.Status == "Giao hàng thành công" && s.IDKhachHang == id).Count();
            return View();
        }


        public ActionResult CountChoXacNhan()
        {
            ViewData["Number"] = db.Orders.Where(s => s.Status == "Chưa xác nhận").Count();
            return View();
        }
    }
}