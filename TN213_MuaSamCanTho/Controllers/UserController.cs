using CaptchaMvc.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TN213_MuaSamCanTho.Models;
using TN213_MuaSamCanTho.Tools;

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
                string spass = Tools.Assistance.MaHoaMatKhau(f["password"]);
                Console.WriteLine(spass);
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

        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }


        [HttpPost]
        public ActionResult DangKy(FormCollection f)
        {
            //Kiểm tra captcha hợp lệ
            if (!this.IsCaptchaValid("Captcha is not valid!"))
                TempData["error"] = "Mã captcha không đúng!";
            else
            {
                if (ModelState.IsValid)
                {
                    var username = f["username"];
                    var kttk = db.TaiKhoans.FirstOrDefault(n => n.TenDangNhap == username);
                    
                    if (kttk == null)
                    {
                        TaiKhoan tk = new TaiKhoan();
                        tk.TenNguoiDung = f["name"];
                        tk.TenDangNhap = f["username"];
                        tk.MatKhau = Tools.Assistance.MaHoaMatKhau(f["password"]);
                        tk.QuyenAdmin = false;

                        db.TaiKhoans.Add(tk); //lưu vào dataset
                        db.SaveChanges();//thêm vào csdl

                        TempData["success"] = "Đăng ký thành công! Vui lòng đăng nhập để kiểm tra.";
                        return RedirectToAction("DangNhap");
                    }
                    else 
                        TempData["error"] = "Tên tài khoản đã tồn tại! Vui lòng chọn tên tài khoản khác.";
                   
                }
                else
                    TempData["error"] = "Đã có lỗi xảy ra.";

            }

            return View();
        }

        [HttpGet]
        public ActionResult SuaThongTin()
        {
            if (Session["TaiKhoan"] != null)
            {
                TaiKhoan tk = Session["TaiKhoan"] as TaiKhoan;
                return View(tk);
            }
            //Nếu chưa đăng nhập thì đưa về trang chủ
            return RedirectToAction("Index", "Home");
        }



        [HttpPost]
        public ActionResult SuaThongTin(FormCollection f)
        {
                if (ModelState.IsValid)
                {
                    int id = int.Parse(f["id"]);
                    var tk = db.TaiKhoans.FirstOrDefault(n => n.MaNguoiDung == id);

                    if (tk != null)
                    {
                        tk.TenNguoiDung = f["name"];
                        if(f["password"] != "")
                            tk.MatKhau = Tools.Assistance.MaHoaMatKhau(f["password"]);

                        db.SaveChanges();//thêm vào csdl
                        TempData["success"] = "Lưu thay đổi thành công!";
                        Session["TaiKhoan"] = tk;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        TempData["error"] = "Tài khoản không tồn tại.";

                }
                else
                    TempData["error"] = "Đã có lỗi xảy ra.";

            return View();
        }
    }

}