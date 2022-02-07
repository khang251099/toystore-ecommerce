using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Controllers
{
    public class BrandsController : Controller
    {
        private TOYSTORE_MODELEntities3 db = new TOYSTORE_MODELEntities3();

        // GET: Brands
        public ActionResult Index()
        {
            return View(db.Brands.ToList());
        }

        // GET: Brands/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // GET: Brands/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDBrand,BrandName,Image")] Brand brand, HttpPostedFileBase Images)
        {
            if (ModelState.IsValid)
            {
                if (Images != null)
                {
                    var fileName = System.IO.Path.GetFileName(Images.FileName);


                    brand.Image = fileName;


                    string path = Path.Combine(Server.MapPath("~/Contents/Image"), fileName);


                    Images.SaveAs(path);

                }
                db.Brands.Add(brand);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(brand);
        }

        // GET: Brands/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDBrand,BrandName,Image")] Brand brand, HttpPostedFileBase Images)
        {
            if (ModelState.IsValid)
            {
                if (Images != null)
                {
                    var fileName = Path.GetFileName(Images.FileName);


                    brand.Image = fileName;


                    string path = Path.Combine(Server.MapPath("~/Content/Image/Brand"), fileName);


                    Images.SaveAs(path);

                }
                db.Brands.Add(brand);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(brand);
        }

        // GET: Brands/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["AdminID"] != null && Session["AdminRole"] != null && Session["AdminRole"].ToString() == "1")
                return View(db.Brands.Where(s => s.IDBrand == id).FirstOrDefault());
            else
                return Content("Bạn không được quyền truy cập trang web này");
        }


        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Brand bra)
        {
            try
            {
                // TODO: Add delete logic here
                bra = db.Brands.Where(x => x.IDBrand == id).FirstOrDefault();
                db.Brands.Remove(bra);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Lỗi ràng buộc khóa ngoại");
            }
        }
    }
}
