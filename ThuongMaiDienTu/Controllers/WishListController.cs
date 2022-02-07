using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThuongMaiDienTu.Models;

namespace ThuongMaiDienTu.Controllers
{
    public class WishListController : Controller
    {
        private TOYSTORE_MODELEntities3 db = new TOYSTORE_MODELEntities3();

        // GET: WishList
        public WishList GetWishList()
        {
            WishList list = Session["WishList"] as WishList;
            if (list == null || Session["WishList"] == null)
            {
                list = new WishList();
                Session["WishList"] = list;
            }
            return list;
        }
        public ActionResult AddToWishList(long id)
        {
            var pro = db.Products.SingleOrDefault(s => s.IDProduct == id);
            if (pro != null)
            {
                GetWishList().Add(pro);
            }
            return RedirectToAction("ShowToWishList", "WishList", new { r = Request.Url.ToString() });
        }
        public ActionResult ShowToWishList()
        {
            if (Session["WishList"] == null)
            {
                ViewBag.Empty = "Empty wish list";
            }
            WishList wl = Session["WishList"] as WishList;
            return View(wl);
        }
        public PartialViewResult BagWishList()
        {
            int _t_item = 0;
            WishList wl = Session["WishList"] as WishList;
            if (wl != null)
            {
                _t_item = wl.Total_Quantity();
            }
            ViewBag.infoWishList = _t_item;
            return PartialView("BagWishList");
        }
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
        public ActionResult AddtoCart(long id)
        {
            WishList wl = Session["WishList"] as WishList;
            var pro = db.Products.SingleOrDefault(s => s.IDProduct == id);
            if (pro != null)
            {
                GetCart().Add(pro);
                wl.ClearWishlist();
            }
            return RedirectToAction("ShowCart", "ShoppingCart", new { r = Request.Url.ToString() });
        }
        public ActionResult RemoveWishList(int id)
        {
            WishList wl = Session["WishList"] as WishList;
            wl.Remove_WishList_Item(id);
            return RedirectToAction("ShowToWishList", "WishList");
        }
    }
}