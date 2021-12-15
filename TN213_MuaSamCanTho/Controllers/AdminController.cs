using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TN213_MuaSamCanTho.Models;

namespace TN213_MuaSamCanTho.Controllers
{
    public class AdminController : Controller
    {
        ModelDbContext db = new ModelDbContext();
        // GET: Admin

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        //Đăng nhập
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
                string username = f["username"];
                string password = Tools.Assistance.MaHoaMatKhau(f["password"]);

                //Tài khoản đăng nhập có quyền quản trị
                TaiKhoan tk = db.TaiKhoans.FirstOrDefault(t => t.TenDangNhap == username && t.MatKhau == password && t.QuyenAdmin == true);

                //nếu tìm thấy bản ghi hợp lệ
                if (tk != null)
                {
                    //Lấy quyền sử dụng
                    String quyen = "Admin";

                    //Gọi tới hàm phân quyền
                    PhanQuyen(username, quyen);

                    //Lưu session
                    tk.MatKhau = "";//Không lưu mật khẩu tài khoản quản trị vào session
                    Session["Admin"] = tk;

                    return RedirectToAction("Index", "Admin");

                }

                //nếu không tìm thấy tài khoản hợp lệ
                TempData["error"] = "Tài khoản không hợp lệ.";
                return View();

            }

            //nếu không thành công
            TempData["error"] = "Đã xảy ra lỗi";
            return View();
        }


        //Hàm phân quyền
        private void PhanQuyen(string TaiKhoan, string Quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1,
                                          TaiKhoan, //username
                                          DateTime.Now, //Thời gian bắt đầu
                                          DateTime.Now.AddHours(3), //Thời gian kết thúc
                                          false, //Ghi nhớ tài khoản?
                                          Quyen, // "admin/user"
                                          FormsAuthentication.FormsCookiePath);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
        }

        //Đăng xuất
        [Authorize]
        public ActionResult DangXuat()
        {
            Session["Admin"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("DangNhap");
        }
    }
}