using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TN213_MuaSamCanTho.Models;

namespace TN213_MuaSamCanTho.Controllers
{
    public class TaiKhoansController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: TaiKhoans
        public ActionResult Index()
        {
            return View(db.TaiKhoans.ToList());
        }


        // GET: TaiKhoans/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TaiKhoans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNguoiDung,TenDangNhap,MatKhau,TenNguoiDung,QuyenAdmin")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                TaiKhoan kt = db.TaiKhoans.FirstOrDefault(t => t.TenDangNhap == taiKhoan.TenDangNhap);
                if(kt != null)
                {
                    TempData["error"] = "Tên tài khoản đã tồn tại.";
                    return RedirectToAction("Index");
                }

                taiKhoan.MatKhau = Tools.Assistance.MaHoaMatKhau("12345");
                db.TaiKhoans.Add(taiKhoan);
                db.SaveChanges();
                TempData["success"] = "Đã thêm thành công.";
                return RedirectToAction("Index");
            }

            return View(taiKhoan);
        }

        // GET: TaiKhoans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // POST: TaiKhoans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection f)
        {
            if (ModelState.IsValid)
            {
                var id = int.Parse( f["MaNguoiDung"]);
                TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
                if (taiKhoan == null)
                {
                    return HttpNotFound();
                }
                var matKhau = f["MatKhau"];
                if (matKhau != null && matKhau.ToString().Trim() != "")
                {
                    taiKhoan.MatKhau = Tools.Assistance.MaHoaMatKhau(matKhau); //Trường hợp có cấp lại mật khẩu
                }
                taiKhoan.QuyenAdmin = bool.Parse(f["QuyenAdmin"]);
                db.SaveChanges();
                TempData["success"] = "Lưu thay đổi thành công.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Đã có lỗi xảy ra.";
            return RedirectToAction("Index");
        }

        // GET: TaiKhoans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // POST: TaiKhoans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Xóa bình luận của tài khoản này
            var binhLuan = db.BinhLuans.Where(b => b.MaNguoiDung == id);
            if (binhLuan != null)
            {
                foreach (var i in binhLuan)
                {
                    db.BinhLuans.Remove(i);
                }

            }
            TaiKhoan taiKhoan = db.TaiKhoans.Find(id);
            db.TaiKhoans.Remove(taiKhoan);
            db.SaveChanges();
            TempData["success"] = "Đã xóa tài khoản.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
