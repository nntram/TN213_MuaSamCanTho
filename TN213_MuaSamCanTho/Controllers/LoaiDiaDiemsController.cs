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
    public class LoaiDiaDiemsController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: LoaiDiaDiems
        public ActionResult Index()
        {
            return View(db.LoaiDiaDiems.ToList());
        }

        

        // GET: LoaiDiaDiems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoaiDiaDiems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaLoai,TenLoai,GhiChu")] LoaiDiaDiem loaiDiaDiem)
        {
            if (ModelState.IsValid)
            {
                db.LoaiDiaDiems.Add(loaiDiaDiem);
                db.SaveChanges();
                TempData["success"] = "Thêm mới thành công!";
                return RedirectToAction("Index");
            }

            return View(loaiDiaDiem);
        }

        // GET: LoaiDiaDiems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiDiaDiem loaiDiaDiem = db.LoaiDiaDiems.Find(id);
            if (loaiDiaDiem == null)
            {
                return HttpNotFound();
            }
            return View(loaiDiaDiem);
        }

        // POST: LoaiDiaDiems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaLoai,TenLoai,GhiChu")] LoaiDiaDiem loaiDiaDiem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loaiDiaDiem).State = EntityState.Modified;
                db.SaveChanges();
                TempData["success"] = "Lưu thay đổi thành công!";
                return RedirectToAction("Index");
            }
            return View(loaiDiaDiem);
        }

        // GET: LoaiDiaDiems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiDiaDiem loaiDiaDiem = db.LoaiDiaDiems.Find(id);
            if (loaiDiaDiem == null)
            {
                return HttpNotFound();
            }
            DiaDiem kiemTraKhoaNgoai = db.DiaDiems.FirstOrDefault(s => s.MaLoai == id);
            if(kiemTraKhoaNgoai != null)
            {
                TempData["error"] = "Không thể xóa do ràng buộc dữ liệu.";
                return RedirectToAction("Index");
            }

            return View(loaiDiaDiem);
        }

        // POST: LoaiDiaDiems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LoaiDiaDiem loaiDiaDiem = db.LoaiDiaDiems.Find(id);
            db.LoaiDiaDiems.Remove(loaiDiaDiem);
            db.SaveChanges();
            TempData["success"] = "Xóa thành công";
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
