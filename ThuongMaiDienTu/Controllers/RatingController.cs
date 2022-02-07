using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Controllers
{
    public class RatingController : Controller
    {
        // GET: Rating
        private TOYSTORE_MODELEntities3 _db = new TOYSTORE_MODELEntities3();
        // GET: Rating
        public ActionResult Index(string search, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 5;
            if (search == null)
            {

                return View(_db.DanhGias.ToList().ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return View(_db.DanhGias.Where(s => s.Product.ProductName.Contains(search) || s.Product.Category.CateName.Contains(search) || s.Content.Contains(search) || s.KhachHang.FullName.Contains(search)).ToList().ToPagedList(pageNumber, pageSize));
            }
        }
        public ActionResult Details(int id)
        {
            return View(_db.DanhGias.Where(s => s.IDDanhGia == id).FirstOrDefault());
        }   


    }
}