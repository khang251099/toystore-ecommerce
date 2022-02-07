using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThuongMaiDienTu.Models;
using ThuongMaiDienTu.Models.ViewModel;

namespace ThuongMaiDienTu.Controllers
{
    public class ShoppingCartController : Controller
    {
        TOYSTORE_MODELEntities3 _db = new TOYSTORE_MODELEntities3();

        // GET: ShoppingCart
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        public ActionResult AddToCart(int id, DateTime? today, TimeSpan? time)
        {
            today = DateTime.Now.Date;
            time = DateTime.Now.TimeOfDay;
            var pro = _db.Products.SingleOrDefault(s => s.IDProduct == id);

            if (pro.SoLuong <= 0)
            {
                return Content("Sản phẩm này đã hết");
            }
            if (pro != null)
            {
                GetCart().Add(pro);

            }
            string url = this.Request.UrlReferrer.AbsolutePath;
            //return RedirectToAction("Index", "Home");
            return Redirect(url);
        }
        public ActionResult ShowCart()
        {
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Cart cart = Session["Cart"] as Cart;
                ViewBag.IDMethod = new SelectList(_db.PayMethods, "IDMethod", "MethodName");

                return View(cart);
            }
        }
        public ActionResult Update_Quantity_Cart(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(form["ID_Product"]);
            int quantity = int.Parse(form["Quantity"]);
            var toy = _db.Products.Where(s => s.IDProduct == id_pro).FirstOrDefault();
            if (quantity <= 0)
                return Content("Vui lòng nhập số lượng >= 1");
            if (quantity > toy.SoLuong)
                return Content("Không đủ số lượng để bán ");
            else
                cart.Update_Quantity_Shopping(id_pro, quantity);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public ActionResult RemoveCart(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.Remove_CartItem(id);
            return RedirectToAction("ShowCart", "ShoppingCart");
        }
        public PartialViewResult BagCart()
        {
            int _t_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
            {
                _t_item = cart.Total_Quantity();
            }
            ViewBag.infoCart = _t_item;
            return PartialView("BagCart");
        }
        public ActionResult OrderSuccessfully()
        {
            
            return View();
           
        }
        public ActionResult CheckOut(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            Order _order = new Order();
            _order.NgayDat = DateTime.Now;
            _order.PhiShip = 230000;
            _order.Status = "Chưa xác nhận";
            _order.StatusPayment = "Chưa thanh toán";
            _order.DiaChi_Cus = form["Address_Cus"];
            _order.IDMethod = Convert.ToInt32(form["IDMethod"]);
            _order.TongTien = cart.Total_Money();
            _order.ThueVAT = _order.TongTien * 0.1;
            _order.GiamGia = 0;
            _order.ThanhTien = _order.TongTien + _order.PhiShip + _order.ThueVAT - _order.GiamGia;
            _db.Orders.Add(_order);
            if (_order.IDMethod == 1)
            {
                _order.MethodName = "Hình thức mua hàng ShipCOD";
            }
            else
            {
                _order.MethodName = "Hình thức thanh toán qua Paypal";
            }
            if (Session["IDUser"] != null)
            {
                int id = Convert.ToInt32(Session["IDUser"]);
                var em = Session["Email"].ToString();
                var phone = Session["PhoneNumber"].ToString();
                var ten = Session["FullName"].ToString();
                _order.Email = em;
                _order.Phone_Cus = phone;
                _order.FullName = ten;
                _order.IDKhachHang = id;
            
                OrderDetail _orderDetail = new OrderDetail();
                double point = Convert.ToDouble(_orderDetail.UnitPrice * 0.01);

                var user = _db.KhachHangs.Where(s => s.IDKhachHang == _order.IDKhachHang).FirstOrDefault();
    
                var cus=_db.KhachHangs.Where(s=>s.IDKhachHang==id).FirstOrDefault();
                if(cus.RankID==1)
                {
                    _order.GiamGia = 0;
                    _order.PhiShip = 23000;
                }    
                if(cus.RankID==2)
                {
                    _order.GiamGia = 0.1;
                    _order.PhiShip = 15000;
                }

                Session["RankID"] = user.RankID;
            }
            else
            {
                _order.IDKhachHang = null;
                _order.FullName = form["FullName"];
                _order.Phone_Cus = form["PhoneNumber"];
                _order.Email = form["Email"];
               
            }
            if (_order.IDMethod == 2)
            {
                TempData["OrderTemp"] = _order;
                return RedirectToAction("PaymentWithPaypal", "Home");
            }
            double total = 0;
            foreach (var item in cart.Items)
            {
                OrderDetail _orderDetail = new OrderDetail();
                _orderDetail.IDOrder = _order.IDOrder;
                _orderDetail.IDProduct = item._shopping_product.IDProduct;
                _orderDetail.Quantity = item._shopping_quantity;

                _order.TongTien = cart.Total_Money() ;
        
                _orderDetail.UnitPrice = (item._shopping_product.PromotionPrice * item._shopping_quantity);
     
                _order.ThanhTien = _order.TongTien+_order.PhiShip + _order.ThueVAT - _order.GiamGia*_order.TongTien;
                total = (double)_order.ThanhTien;
                _db.OrderDetails.Add(_orderDetail);
            
                var pro = _db.Products.SingleOrDefault(s => s.IDProduct == _orderDetail.IDProduct);
                pro.SoLuong = pro.SoLuong - _orderDetail.Quantity;
       
                _db.SaveChanges();
            }
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Contents/Client/neworder.html"));
            content = content.Replace("{{CustomerName}}", _order.FullName);
            content = content.Replace("{{Phone}}", _order.Phone_Cus);
            content = content.Replace("{{Email}}", _order.Email);
            content = content.Replace("{{Address}}", _order.DiaChi_Cus);
            content = content.Replace("{{Total}}", total.ToString("N0"));
            content = content.Replace("{{IDOrder}}", _order.IDOrder.ToString());
            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
            new Mail().SendMail(toEmail, "Đơn hàng mới từ CaLaSuToy ", content);
            var toEmails = _order.Email;
            new Mail().SendMail(toEmails, "Bạn có đơn hàng mới từ CaLaSuToy", content);
            _db.SaveChanges();
            cart.ClearCart();
            return RedirectToAction("OrderSuccessfully", "ShoppingCart");
        }

    }
}


