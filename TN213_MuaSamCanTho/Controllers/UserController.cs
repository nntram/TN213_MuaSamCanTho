using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TN213_MuaSamCanTho.Models;

namespace TN213_MuaSamCanTho.Controllers
{
    public class UserController : Controller
    {

        ModelDbContext db = new ModelDbContext();

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }


        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            if (ModelState.IsValid)
            {
                string sname = f["username"].ToString();
                string spass = f["password"].ToString();//Tool.MaHoaMatKhau(f["password"]);

                TaiKhoan kh = db.TaiKhoans.SingleOrDefault(n => n.TenDangNhap == sname && n.MatKhau == spass);

                //nếu thành công
                if (kh != null)
                {
                    kh.MatKhau = ""; //Không lưu mật khẩu để bảo mật
                    Session["TaiKhoan"] = kh;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["error"] = "Sai tên đăng nhập hoặc mật khẩu!";
                    return View();
                }
            }

            //nếu không thành công
            TempData["error"] = "Lối xảy ra khi đăng nhập!";
            return View();

        }
    }

}