using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Controllers
{
    public class ProductsController : Controller
    {
        
        private TOYSTORE_MODELEntities3 db = new TOYSTORE_MODELEntities3();

        // GET: Products
        public ActionResult Index(string search, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 5;
            if (search == null)
            {


                return View(db.Products.ToList().ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return View(db.Products.Where(s => s.IDProduct.ToString() == search || s.ProductName.Contains(search) || s.MaSP.Contains(search) || s.Category.CateName.Contains(search)).ToList().ToPagedList(pageNumber, pageSize));
            }
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.IDAge = new SelectList(db.Ages, "IDAge", "AgeName");
            ViewBag.IDBrand = new SelectList(db.Brands, "IDBrand", "BrandName");
            ViewBag.IDCate = new SelectList(db.Categories, "IDCate", "CateName");
            ViewBag.IDSex = new SelectList(db.Sexes, "IDSex", "SexName");
            Product pro = new Product();
            return View(pro);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDProduct,ProductName,NgayNhap,MoTa,MaSP,ChuDe,XuatXu,Images,Image1,Image2,Image3,Image4,SoLuong,KichThuoc,Gia,IDSex,IDCate,IDBrand,IDAge")] Product product, HttpPostedFileBase Images, HttpPostedFileBase Image1, HttpPostedFileBase Image2, HttpPostedFileBase Image3, HttpPostedFileBase Image4)
        {
            if (ModelState.IsValid)
            {
                if (Images != null || Image1 != null || Image2 != null || Image3 != null || Image4 != null)
                {
                    var fileName = Path.GetFileName(Images.FileName);
                    var fileName1 = Path.GetFileName(Image1.FileName);
                    var fileName2 = Path.GetFileName(Image2.FileName);
                    var fileName3 = Path.GetFileName(Image3.FileName);
                    var fileName4 = Path.GetFileName(Image4.FileName);

                    product.Images = fileName;
                    product.Image1 = fileName1;
                    product.Image2 = fileName2;
                    product.Image3 = fileName3;
                    product.Image4 = fileName4;

                    string path = Path.Combine(Server.MapPath("~/Contents/Image/Toy"), fileName);
                    string path1 = Path.Combine(Server.MapPath("~/Contents/Image/Toy"), fileName1);
                    string path2 = Path.Combine(Server.MapPath("~/Contents/Image/Toy"), fileName2);
                    string path3 = Path.Combine(Server.MapPath("~/Contents/Image/Toy"), fileName3);
                    string path4 = Path.Combine(Server.MapPath("~/Contents/Image/Toy"), fileName4);

                    Images.SaveAs(path);
                    Image1.SaveAs(path1);
                    Image2.SaveAs(path2);
                    Image3.SaveAs(path3);
                    Image4.SaveAs(path4);
                }
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDAge = new SelectList(db.Ages, "IDAge", "AgeName", product.IDAge);
            ViewBag.IDBrand = new SelectList(db.Brands, "IDBrand", "BrandName", product.IDBrand);
            ViewBag.IDCate = new SelectList(db.Categories, "IDCate", "CateName", product.IDCate);
            ViewBag.IDSex = new SelectList(db.Sexes, "IDSex", "SexName", product.IDSex);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDAge = new SelectList(db.Ages, "IDAge", "AgeName", product.IDAge);
            ViewBag.IDBrand = new SelectList(db.Brands, "IDBrand", "BrandName", product.IDBrand);
            ViewBag.IDCate = new SelectList(db.Categories, "IDCate", "CateName", product.IDCate);
            ViewBag.IDSex = new SelectList(db.Sexes, "IDSex", "SexName", product.IDSex);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDProduct,ProductName,NgayNhap,MoTa,MaSP,ChuDe,XuatXu,Images,Image1,Image2,Image3,Image4,SoLuong,KichThuoc,Status,Gia,IDSex,IDCate,IDBrand,IDAge")] Product product, HttpPostedFileBase Images, HttpPostedFileBase Image1, HttpPostedFileBase Image2, HttpPostedFileBase Image3, HttpPostedFileBase Image4)
        {
            if (ModelState.IsValid)
            {
                if (Images != null || Image1 != null || Image2 != null || Image3 != null || Image4 != null)
                {
                    var fileName = Path.GetFileName(Images.FileName);
                    var fileName1 = Path.GetFileName(Image1.FileName);
                    var fileName2 = Path.GetFileName(Image2.FileName);
                    var fileName3 = Path.GetFileName(Image3.FileName);
                    var fileName4 = Path.GetFileName(Image4.FileName);

                    product.Images = fileName;
                    product.Image1 = fileName1;
                    product.Image2 = fileName2;
                    product.Image3 = fileName3;
                    product.Image4 = fileName4;

                    string path = Path.Combine(Server.MapPath("~/Contents/Image/Toy"), fileName);
                    string path1 = Path.Combine(Server.MapPath("~/Contents/Image/Toy"), fileName1);
                    string path2 = Path.Combine(Server.MapPath("~/Contents/Image/Toy"), fileName2);
                    string path3 = Path.Combine(Server.MapPath("~/Contents/Image/Toy"), fileName3);
                    string path4 = Path.Combine(Server.MapPath("~/Contents/Image/Toy"), fileName4);

                    Images.SaveAs(path);
                    Image1.SaveAs(path1);
                    Image2.SaveAs(path2);
                    Image3.SaveAs(path3);
                    Image4.SaveAs(path4);
                }
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDAge = new SelectList(db.Ages, "IDAge", "AgeName", product.IDAge);
            ViewBag.IDBrand = new SelectList(db.Brands, "IDBrand", "BrandName", product.IDBrand);
            ViewBag.IDCate = new SelectList(db.Categories, "IDCate", "CateName", product.IDCate);
            ViewBag.IDSex = new SelectList(db.Sexes, "IDSex", "SexName", product.IDSex);
            return View(product);
        }


        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["AdminID"] != null && Session["AdminRole"] != null && Session["AdminRole"].ToString() == "1")
            {
                return View((db.Products.Where(s => s.IDProduct == id).FirstOrDefault()));
            }
            else
            {
                return Content("Bạn không có quyền truy cập vào trang web này");
            }
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Product pro)
        {
            try
            {
                // TODO: Add delete logic here
                pro = db.Products.Where(x => x.IDProduct == id).FirstOrDefault();
                db.Products.Remove(pro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Lỗi ràng buộc khóa ngoại");
            }
        }


        public ActionResult Report()
        {
            return View(db.BestSeller_1.OrderByDescending(m => m.BestSell).ToList());
        }
        public ActionResult DoanhThuMotNgay()
        {
            var today = DateTime.Now.Day;
            var doanhThu = db.Orders.Where(s => s.NgayDat.Value.Day==today);
            var dt = doanhThu.Sum(m => m.ThanhTien);
            if (dt != null)
            {
                ViewData["DoanhThuMotNgay"] = dt.ToString();
            }
            else
            {
                ViewData["DoanhThuMotNgay"]="Chưa có";
            }
            return View();
        }
        public ActionResult MostRevenue()
        {
            return PartialView(db.BestSeller_1.OrderByDescending(m => m.ThuVe).ToList());
        }
        public ActionResult GetData()
        {
            int doChoiTriTue = db.OrderDetails.Where(x => x.Product.IDCate == 4).Count();
            int mayBayDieuKhien = db.OrderDetails.Where(x => x.Product.IDCate == 5).Count();
            int doChoiBupBe = db.OrderDetails.Where(x => x.Product.IDCate == 6).Count();
            int doChoiXepHinh = db.OrderDetails.Where(x => x.Product.IDCate == 7).Count();
            int xeCongTrinh = db.OrderDetails.Where(x => x.Product.IDCate == 8).Count();
            int xeDap = db.OrderDetails.Where(x => x.Product.IDCate == 9).Count();

            Ratio obj = new Ratio();
            obj.DoChoiTriTue = doChoiTriTue;
            obj.MayBayDieuKhien = mayBayDieuKhien;
            obj.DoChoiBupBe = doChoiBupBe;
            obj.DoChoiXepHinh = doChoiXepHinh;
            obj.XeCongTrinh = xeCongTrinh;


            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetHinhThucThanhToan()
        {
            int payPal = db.Orders.Where(s => s.IDMethod == 2).Count();
            int shipCOD = db.Orders.Where(s => s.IDMethod == 1).Count();
            HinhThucThanhToan ht = new HinhThucThanhToan();

            ht.Paypal = payPal;
            ht.ShipCOD = shipCOD;

            return Json(ht, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TongDonHang()
        {
            ViewData["TongDonHang"] = db.Orders.Where(s => s.Status == "Giao hàng thành công").Count();
            return View();
        }
        public ActionResult TongSoDoChoiBanRa()
        {
            ViewData["TongSoDoChoiBanRa"] = db.OrderDetails.Sum(s => s.Quantity);
            return View();
        }
        public ActionResult DoanhThu()
        {
            ViewData["TongDoanhThu"] = db.Orders.Where(s=>s.Status == "Giao hàng thành công").Sum(s=>s.ThanhTien.Value).ToString("N0");
            ViewData["TongPayPal"] = db.Orders.Where(s => s.Status == "Giao hàng thành công" && s.IDMethod == 2).Sum(s => s.ThanhTien.Value).ToString("N0");
            ViewData["TongShipCOD"] = db.Orders.Where(s => s.Status == "Giao hàng thành công" && s.IDMethod == 1).Sum(s => s.ThanhTien.Value).ToString("N0");
            return View();
        }
        public ActionResult DoanhThuTheoNgay()
        {
            var query = db.Orders.Include("DoanhThu")
               .GroupBy(m => m.NgayDat.Value.Day)
               .Select(s => new { name = s.Key, count = s.Sum(m => m.OrderDetails.Where(x=>x.Order.Status == "Giao hàng thành công").Sum(x => x.Order.ThanhTien)) }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }


        public ActionResult DonHangTheoNgay()
        {
            var query = db.Orders.Include("Order")
               .GroupBy(m => m.NgayDat.Value.Day)
               .Select(s => new { name = s.Key, donhang = s.Count(), ve = s.Sum(w => w.OrderDetails.Sum(m => m.Quantity)) }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DoanhThuTheoThang()
        {
            var query = db.Orders.Include("DonHang")
               .GroupBy(m => m.NgayDat.Value.Month)
               .Select(s => new { name = s.Key, count = s.Sum(w => w.OrderDetails.Where(m => m.Order.Status == "Giao hàng thành công").Sum(m => m.Order.ThanhTien)) }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DonHangTheoThang()
        {
            var query = db.Orders.Include("Order")
               .GroupBy(m => m.NgayDat.Value.Month)
               .Select(s => new { name = s.Key, donhang = s.Count(), ve = s.Sum(w => w.OrderDetails.Sum(m => m.Quantity)) }).ToList();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public class Ratio
        {
            public int DoChoiTriTue { get; set; }
            public int MayBayDieuKhien { get; set; }
            public int DoChoiBupBe { get; set; }
            public int DoChoiXepHinh { get; set; }
            public int XeCongTrinh { get; set; }
            public int XeDap { get; set; }
        }


        public class HinhThucThanhToan
        {
            public int Paypal { get; set; }
            public int ShipCOD { get; set; }

        }
        //Thống kê doanh thu
        public ActionResult Income(DateTime? fromDate, DateTime? toDate)
        {
            var result = from or in db.Orders
                         join odt in db.OrderDetails on or.IDOrder equals odt.IDOrder into t
                         from odt in t.DefaultIfEmpty()
                         select new InCome
                         {
                             NgayDat = or.NgayDat,
                             IDOrder = odt.IDOrder,
                             IDKhachHang= or.IDKhachHang,
                             Quanity = t.Sum(s => s.Quantity),
                             Price = t.Sum(s => s.Order.ThanhTien)
                         };
            var order = result.OrderByDescending(m => m.NgayDat);
            if (!fromDate.HasValue) fromDate = DateTime.Now.Date;
            if (!toDate.HasValue) toDate = fromDate.GetValueOrDefault(DateTime.Now.Date).Date.AddDays(1);
            if (toDate <= fromDate) toDate = fromDate.GetValueOrDefault(DateTime.Now.Date).Date.AddDays(1);
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            var search = order.Where(c => c.NgayDat >= fromDate && c.NgayDat <= toDate).ToList();
            return View(search);
        }
        //public ActionResult TrangThaiDonHang()
        //{
        //    var query = db.Orders.Include("DoanhThu")
        //       .GroupBy(m => m.NgayDat.Value.Day)
        //       .Select(s => new { name = s.Key, count = s.Sum(x=>x.(bool)Status=="Chưa xác nhận" && x.Status=="Đã xác nhận" && x.Status == "Đang lấy hàng" && x.Status == "Đang giao" && x.Status == "Giao hàng thành công" ) }).ToList();
        //    return Json(query, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult SortNgay(DateTime? fromDate, DateTime? toDate)
        {

            if (!fromDate.HasValue)
                fromDate = DateTime.Now.Date;
            if (!toDate.HasValue)
                toDate = fromDate.GetValueOrDefault(DateTime.Now.Date).Date.AddDays(1);
            if (toDate < fromDate)
                toDate = fromDate.GetValueOrDefault(DateTime.Now.Date).Date.AddDays(1);

            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;

            var day = db.Orders
                      .Where(c => c.NgayDat >= fromDate && c.NgayDat < toDate && c.Status == "Giao hàng thành công")
                      .ToList();
            return View(day);
        }
    }
}

