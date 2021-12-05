using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TN213_MuaSamCanTho.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        //Đăng nhập
        public ActionResult DangNhap()
        {
            return View();
        }
    }
}