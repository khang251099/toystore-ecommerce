using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        TOYSTORE_MODELEntities3 _db = new TOYSTORE_MODELEntities3();

        //public ActionResult Index()
        //{

        //    return View();
        //}

        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(Administrator _ad)
        {
            var check = _db.Administrators.Where(m => m.Email.Equals(_ad.Email) &&
              m.PasswordAdmin.Equals(_ad.PasswordAdmin)).FirstOrDefault();
            if (check == null)
            {
                _ad.LogInErrorMessage = "Sai thông tin đăng nhập!";
                return View("SignIn", _ad);
            }
            else
            {
                Session["AdminID"] = check.IDAdmin;
                Session["AdminEmail"] = check.Email;
                Session["AdminName"] = check.FullName;
                Session["AdminRole"] = check.IDRole;
                Session["AdminRoleName"] = check.Role.RoleName;
                return RedirectToAction("Report", "Products");
            }
        }
        public ActionResult LogOut()
        {
            int UserID = (int)Session["AdminID"];
            Session.Abandon();
            return RedirectToAction("SignIn", "Admin");
        }
        public ActionResult Index(string search, int? page)
        {
            if (Session["AdminID"] != null && Session["AdminRole"] != null && Session["AdminRole"].ToString() == "1")
            {
                if (search == null)
                {
                    return View(_db.Administrators.ToList());
                }
                else
                {
                    return View(_db.Administrators.Where(s => s.FullName.Contains(search) || s.Email.Contains(search) || s.Role.RoleName.Contains(search)).ToList());
                }
            }
            else
            {
                return Content("Bạn không có quyền đăng nhập trang này!!");
            }

        }
        public ActionResult Details(int id)
        {
            if (Session["AdminID"] != null && Session["AdminRole"] != null && Session["AdminRole"].ToString() == "1")
                return View(_db.Administrators.Where(s => s.IDAdmin == id).FirstOrDefault());
            else
                return Content("Bạn không được quyền truy cập trang này");
        }
        public ActionResult ChooseRole()
        {
            Role role = new Role();
            role.RoleCollection = _db.Roles.ToList<Role>();
            return PartialView(role);
        }

        public ActionResult Edit(int id)
        {
            if (Session["AdminID"] != null && Session["AdminRole"] != null && Session["AdminRole"].ToString() == "1")
                if (id == 1)
                    return Content("Không thể edit tài khoản này");
                else
                    return View(_db.Administrators.Where(s => s.IDAdmin == id).FirstOrDefault());
            else
                return Content("Bạn không được quyền truy cập trang này");
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(Administrator _ad)
        {
            try
            {
                // TODO: Add update logic here
                _db.Entry(_ad).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Staff");
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["AdminID"] != null && Session["AdminRole"] != null && Session["AdminRole"].ToString() == "1")
                return View();
            else
                return Content("Bạn không được quyền truy cập trang này!");
        }
        [HttpPost]
        public ActionResult Create(Administrator _ad)
        {
            var check = _db.Administrators.FirstOrDefault(s => s.Email == _ad.Email);
            if (check == null)
            {
                _db.Configuration.ValidateOnSaveEnabled = false;
                _db.Administrators.Add(_ad);
                _db.SaveChanges();
                return RedirectToAction("Staff");
            }
            else
            {
                ViewBag.error = "Email này đã tồn tại!";
                return View();
            }
        }
        public ActionResult Delete(int id)
        {
            if (Session["AdminID"] != null && Session["AdminRole"] != null && Session["AdminRole"].ToString() == "1")
                return View(_db.Administrators.Where(s => s.IDAdmin == id).FirstOrDefault());
            else
                return Content("Bạn không được quyền truy cập trang này!");
        }

        // POST: Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Administrator _ad)
        {
            var rolename = _db.Administrators.Where(s => s.IDAdmin == id).FirstOrDefault();
            if (rolename.IDRole != 1)
            {
                try
                {
                    // TODO: Add delete logic here
                    _ad = _db.Administrators.Where(x => x.IDAdmin == id).FirstOrDefault();
                    _db.Administrators.Remove(_ad);
                    _db.SaveChanges();
                    return RedirectToAction("Staff");
                }
                catch
                {
                    return Content("Lỗi ràng buộc khóa ngoại");
                }
            }
            else
            {
                return Content("Không thể xóa tài khoản này");
            }
        }
    }
}