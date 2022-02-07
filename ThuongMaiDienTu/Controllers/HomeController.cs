using PagedList;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThuongMaiDienTu.Models;
using ThuongMaiDienTu.Models.ViewModel;

namespace ThuongMaiDienTu.Controllers
{ 
    public class HomeController : Controller
    {
        TOYSTORE_MODELEntities3 _db = new TOYSTORE_MODELEntities3();
        public ActionResult Index()
        {
            return View(_db.Products.ToList());
        }
        public ActionResult LEGO(Product pro)
        {
            return View(_db.Products.Where(s => s.Brand.IDBrand == 1).OrderBy(s => s.Gia).Take(4).ToList());
        }
        public ActionResult BARBIE(Product pro)
        {
            return View(_db.Products.Where(s => s.Brand.IDBrand == 2).OrderBy(s => s.Gia).Take(4).ToList());
        }
        public ActionResult LOL(Product pro)
        {
            return View(_db.Products.Where(s => s.Brand.IDBrand == 3).OrderBy(s => s.Gia).Take(4).ToList());
        }
        public ActionResult BRUDER(Product pro)
        {
            return View(_db.Products.Where(s => s.Brand.IDBrand == 4).OrderBy(s => s.Gia).Take(4).ToList());
        }
        public ActionResult Arrival()
        {
            return PartialView(_db.Products.OrderByDescending(m => m.NgayNhap).Take(6));
        }
        public ActionResult Detail(int id)
        {
            var rate = new DanhGia();
            return View(_db.Products.Where(s => s.IDProduct == id).FirstOrDefault());

        }
        [HttpGet]
        public ActionResult Rating()
        {
            return View();
        }
        public ActionResult SearchResult(string search)
        {
            if (search == null)
            {
                return View(_db.Products.ToList());
            }
            else
            {
                return View(_db.Products.Where(s => s.ProductName.Contains(search) || s.Brand.BrandName.Contains(search) || s.Category.CateName.Contains(search)).ToList());
            }
        }
        [HttpPost]
        public ActionResult Rating(FormCollection form, int id)
        {
            string email = Session["Email"].ToString();
            var pro = _db.Products.Where(s => s.IDProduct == id).FirstOrDefault();
            try
            {
                DanhGia _rate = new DanhGia();
                _rate.NgayDanhGia = DateTime.Now;
                _rate.IDProduct = pro.IDProduct;
                _rate.IDKhachHang = _db.KhachHangs.Single(m => m.Email.Equals(email)).IDKhachHang;
                _rate.Content = form["content"];
                _db.DanhGias.Add(_rate);
                _db.SaveChanges();
                return RedirectToAction("Detail", "Home", new { id = pro.IDProduct });
            }
            catch
            {
                return Content("Không được để trống");
            }
        }
        public ActionResult RelatedProducts(int id)
        {
            var pro = _db.Products.Where(s => s.IDProduct == id).FirstOrDefault();

            var result = from r in _db.Products
                         where (r.IDCate == pro.IDCate
                         && r.IDProduct != id)
                         select r;
            return PartialView(result.ToList());
        }
        public ActionResult BestSeller()
        {
            return PartialView(_db.BestSeller_1.OrderByDescending(m => m.BestSell).ToList().Take(4));
        }
        [HttpGet]
        public ActionResult GioiThieu()
        {
            return View();
        }
        [HttpGet]
        public ActionResult LienHe()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LienHe(FormCollection form)
        {
            string email = Session["Email"].ToString();
            LienHe _contact = new LienHe();
            _contact.NgayLienHe = DateTime.Now;
            _contact.IDKhachHang = _db.KhachHangs.Single(m => m.Email.Equals(email)).IDKhachHang;
            _contact.ChuDe = form["ChuDe"];
            _contact.NoiDung = form["NoiDung"];
            _contact.Status = "Chưa phản hồi";
            _db.LienHes.Add(_contact);
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult AllProducts(string searchBy, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 4;
            if (searchBy == "Date")
                return View(_db.Products.OrderByDescending(m => m.NgayNhap).ToList().ToPagedList(pageNumber, pageSize));
            if (searchBy == "Name")
                return View(_db.Products.OrderBy(m => m.ProductName).ToList().ToPagedList(pageNumber, pageSize));
            if (searchBy == "Price")
                return View(_db.Products.OrderBy(m => m.Gia).ToList().ToPagedList(pageNumber, pageSize));
            else
                return View(_db.Products.ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult TatCaSanPham(string Gia)
        {
            if (Gia == "option1")
                return View(_db.Products.Where(s => s.Gia < 50000 ).ToList());
            if (Gia == "option2")
                return View(_db.Products.Where(s => s.Gia >= 500000 && s.Gia < 100000).ToList());
            if (Gia == "option3")
                return View(_db.Products.Where(s => s.Gia >= 100000 && s.Gia < 500000).ToList());
            if (Gia == "option4")
                return View(_db.Products.Where(s => s.Gia >= 500000 ).ToList());
            else
                return View(_db.Products.ToList());
        }

        public ActionResult CateOne(string Gia)
        {
            if (Gia == "option1")
                return View(_db.Products.Where(s => s.Gia < 50000 && s.IDCate == 6).ToList());
            if (Gia == "option2")
                return View(_db.Products.Where(s => s.Gia >= 50000 && s.Gia < 100000 && s.IDCate == 6).ToList());
            if (Gia == "option3")
                return View(_db.Products.Where(s => s.Gia >= 100000 && s.Gia < 500000 && s.IDCate == 6).ToList());
            if (Gia == "option4")
                return View(_db.Products.Where(s => s.Gia >= 500000 && s.IDCate == 6).ToList());
            else
                return View(_db.Products.Where(s => s.Category.IDCate == 6).OrderBy(s => s.Gia).Take(4).ToList());
        }
        public ActionResult CateTwo(string Gia)
        {
            if (Gia == "option1")
                return View(_db.Products.Where(s => s.Gia < 50000 && s.IDCate == 5).ToList());
            if (Gia == "option2")
                return View(_db.Products.Where(s => s.Gia >= 50000 && s.Gia < 100000 && s.IDCate == 5).ToList());
            if (Gia == "option3")
                return View(_db.Products.Where(s => s.Gia >= 100000 && s.Gia < 500000 && s.IDCate == 5).ToList());
            if (Gia == "option4")
                return View(_db.Products.Where(s => s.Gia >= 500000  && s.IDCate == 5).ToList());
            else
                return View(_db.Products.Where(s => s.Category.IDCate == 5).OrderBy(s => s.Gia).Take(4).ToList());
        }
        public ActionResult CateThree(string Gia)
        {
            if (Gia == "option1")
                return View(_db.Products.Where(s => s.Gia < 50000 && s.IDCate == 4).ToList());
            if (Gia == "option2")
                return View(_db.Products.Where(s => s.Gia >= 50000 && s.Gia < 100000 && s.IDCate == 4).ToList());
            if (Gia == "option3")
                return View(_db.Products.Where(s => s.Gia >= 100000 && s.Gia < 500000 && s.IDCate == 4).ToList());
            if (Gia == "option4")
                return View(_db.Products.Where(s => s.Gia >= 500000  && s.IDCate == 4).ToList());
            else
                return View(_db.Products.Where(s => s.Category.IDCate == 4).OrderBy(s => s.Gia).Take(4).ToList());
        }
        public ActionResult Catefour(string Gia)
        {
            if (Gia == "option1")
                return View(_db.Products.Where(s => s.Gia < 50000 && s.IDCate == 9).ToList());
            if (Gia == "option2")
                return View(_db.Products.Where(s => s.Gia >= 50000 && s.Gia < 100000 && s.IDCate == 9).ToList());
            if (Gia == "option3")
                return View(_db.Products.Where(s => s.Gia >= 100000 && s.Gia < 500000 && s.IDCate == 9).ToList());
            if (Gia == "option4")
                return View(_db.Products.Where(s => s.Gia >= 500000  && s.IDCate == 9).ToList());
            else
                return View(_db.Products.Where(s => s.Category.IDCate == 9).OrderBy(s => s.Gia).Take(4).ToList());
        }
        public ActionResult Catefive(string Gia)
        {
            if (Gia == "option1")
                return View(_db.Products.Where(s => s.Gia < 50000 && s.IDCate == 7).ToList());
            if (Gia == "option2")
                return View(_db.Products.Where(s => s.Gia >= 50000 && s.Gia < 100000 && s.IDCate == 7).ToList());
            if (Gia == "option3")
                return View(_db.Products.Where(s => s.Gia >= 100000 && s.Gia < 500000 && s.IDCate == 7).ToList());
            if (Gia == "option4")
                return View(_db.Products.Where(s => s.Gia >= 500000  && s.IDCate == 7).ToList());
            else
                return View(_db.Products.Where(s => s.Category.IDCate == 7).OrderBy(s => s.Gia).Take(4).ToList());
        }
        public ActionResult Catesix(string Gia)
        {
            if (Gia == "option1")
                return View(_db.Products.Where(s => s.Gia < 50000 && s.IDCate == 8).ToList());
            if (Gia == "option2")
                return View(_db.Products.Where(s => s.Gia >= 50000 && s.Gia < 100000 && s.IDCate == 8).ToList());
            if (Gia == "option3")
                return View(_db.Products.Where(s => s.Gia >= 100000 && s.Gia < 500000 && s.IDCate == 8).ToList());
            if (Gia == "option4")
                return View(_db.Products.Where(s => s.Gia >= 500000  && s.IDCate == 8).ToList());
            else
                return View(_db.Products.Where(s => s.Category.IDCate == 8).OrderBy(s => s.Gia).Take(4).ToList());
        }
        public ActionResult FilterOneCate()
        {
            return View(_db.Products.Where(s => s.Category.IDCate == 6).Take(4).ToList());
        }
        public ActionResult SanPhamSale()
        {
            return View(_db.Products.Where(s => s.PromotionPrice<s.Gia).OrderBy(s => s.Gia).Take(4).ToList());
        }
        public ActionResult UuDaiDacBiet(string PromotionPrice)
        {
            if (PromotionPrice == "option1")
                return View(_db.Products.Where(s => s.PromotionPrice < 50000 ).ToList());
            if (PromotionPrice == "option2")
                return View(_db.Products.Where(s => s.PromotionPrice >= 50000 && s.PromotionPrice < 100000 ).ToList());
            if (PromotionPrice == "option3")
                return View(_db.Products.Where(s => s.PromotionPrice >= 100000 && s.PromotionPrice < 500000 ).ToList());
            if (PromotionPrice == "option4")
                return View(_db.Products.Where(s => s.PromotionPrice >= 500000 ).ToList());
            else
                return View(_db.Products.OrderByDescending(s => s.PromotionPrice< s.Gia).Take(4).ToList());
        }
        public static class PaypalConfiguration
        {
            //Variables for storing the clientID and clientSecret key  
            public readonly static string ClientId;
            public readonly static string ClientSecret;
            //Constructor  
            static PaypalConfiguration()
            {
                var config = GetConfig();
                ClientId = config["clientId"];
                ClientSecret = config["clientSecret"];
            }
            // getting properties from the web.config  
            public static Dictionary<string, string> GetConfig()
            {
                return PayPal.Api.ConfigManager.Instance.GetProperties();
            }
            private static string GetAccessToken()
            {
                // getting accesstocken from paypal  
                string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
                return accessToken;
            }
            public static APIContext GetAPIContext()
            {
                // return apicontext object by invoking it with the accesstoken  
                APIContext apiContext = new APIContext(GetAccessToken());
                apiContext.Config = GetConfig();
                return apiContext;
            }
        }
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Home/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return RedirectToAction("OrderFailure", "ShoppingCart");
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("OrderFailure", "ShoppingCart");
            }
            //on successful payment, show success page to user.
            Cart cart = Session["Cart"] as Cart;
            Models.Order _order = TempData["OrderTemp"] as Models.Order;
            _order.StatusPayment = "Thanh toán thành công";
            _db.Orders.Add(_order);
            decimal total = 0;
            foreach (var item in cart.Items)
            {
                OrderDetail _orderDetail = new OrderDetail();
      
                _orderDetail.IDOrder = _order.IDOrder;
                _orderDetail.IDProduct = item._shopping_product.IDProduct;
                _orderDetail.Quantity = item._shopping_quantity;
                _order.TongTien = cart.Total_Money();
                _orderDetail.UnitPrice = (item._shopping_product.PromotionPrice * item._shopping_quantity);
                _order.ThanhTien = _order.TongTien + _order.PhiShip + _order.ThueVAT - _order.GiamGia*_order.TongTien;
                total = (int)_order.ThanhTien;
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

            return RedirectToAction("OrderSuccessfully","ShoppingCart");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };

            Cart cart = Session["Cart"] as Cart;
            Models.Order _order = TempData["OrderTemp"] as Models.Order;
            _order.StatusPayment = "Thanh toán thành công";
            decimal _subtotal = 0;
            decimal giamGia;
            foreach (var item in cart.Items)
            {
                decimal tempPrice = 0;
                //if (item._shopping_product.PromotionPrice != null )
                    tempPrice = (decimal)(item._shopping_product.PromotionPrice)/23000;
                //else
                //    tempPrice = (decimal)(item._shopping_product.Gia / 23000 * (decimal)_order.GiamGia);
                //Adding Item Details like name, currency, price etc  
                itemList.items.Add(new Item()
                {
                    name = item._shopping_product.ProductName.ToString(),
                    currency = "USD",
                    price = tempPrice.ToString("#,##0.00"),
                    quantity = item._shopping_quantity.ToString(),
                    sku = "SP#" + item._shopping_product.IDProduct
                });
                _subtotal += Convert.ToDecimal(tempPrice.ToString("#,##0.00")) * item._shopping_quantity;
            }
            //if (_order.GiamGia!=null)
            //    _subtotal -= _subtotal*(decimal)_order.GiamGia;
            giamGia = _subtotal * 5 / 100;
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {   
                tax = (_subtotal * 10/100).ToString("#,##0.00"),
                shipping = Convert.ToDecimal(_order.PhiShip/(decimal)23000).ToString("#,##0.00"),
                shipping_discount=giamGia.ToString("#,##0.00"),
                subtotal = _subtotal.ToString("#,##0.00"),  
                
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = (Convert.ToDecimal(details.tax)+ Convert.ToDecimal(details.shipping)+ Convert.ToDecimal(details.subtotal) - Convert.ToDecimal(details.shipping_discount)).ToString("#,##0.00"), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            _order.PaypalAmount = Convert.ToDouble(amount.total);
            
            DateTime dt = DateTime.Now;
            String date;
            date = dt.ToString("yyyyddMMhhmmss", DateTimeFormatInfo.InvariantInfo);
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = date,
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }
        private Payment CreatePayment2(APIContext apiContext, string redirectUrl)
        {
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = "Item Name comes here",
                currency = "USD",
                price = "1",
                quantity = "1",
                sku = "sku"
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = "1"
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = "3", // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = "your generated invoice number", //Generate an Invoice No  
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }
    }
}