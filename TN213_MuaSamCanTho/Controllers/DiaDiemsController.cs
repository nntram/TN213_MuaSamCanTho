using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TN213_MuaSamCanTho.Models;

namespace TN213_MuaSamCanTho.Controllers
{
    public class DiaDiemsController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: DiaDiems
        public ActionResult Index()
        {
            var diaDiems = db.DiaDiems.Include(d => d.LoaiDiaDiem);

            List<SelectListItem> items = new SelectList(db.LoaiDiaDiems, "MaLoai", "TenLoai", 0).ToList();
            items.Insert(0, (new SelectListItem { Text = "---Tất cả---", Value = "0" }));

            ViewBag.Loai = items;
            return View(diaDiems.ToList());
        }

        

        [HttpPost]
        public ActionResult KQTimKiemPartial(int Loai)
        {
            List<DiaDiem> lstDiaDiem = new List<DiaDiem>();
            if (Loai == 0)
            {
                lstDiaDiem = db.DiaDiems.ToList();
                
            }
                
            else
            {
                lstDiaDiem = db.DiaDiems.Where(s=>s.MaLoai==Loai).ToList();
                
            }

            if (lstDiaDiem.Count() > 0)
                return PartialView(lstDiaDiem);
            else
                return Content("<h2 class=\"text-center\">Không tìm thấy kết quả phù hợp.</h2>");
        }

        // GET: DiaDiems/Create
        public ActionResult Create()
        {
            ViewBag.MaLoai = new SelectList(db.LoaiDiaDiems, "MaLoai", "TenLoai");
            return View();
        }

        // POST: DiaDiems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DiaDiem diaDiem, HttpPostedFileBase hinhAnh, string The_Geom_WKT)
        {
            if (hinhAnh != null)
            {
                if (hinhAnh.ContentLength > 0 && (hinhAnh.ContentType == "image/jpeg"|| hinhAnh.ContentType == "image/png"))
                {
                    var fileName = Path.GetFileName(hinhAnh.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/LocationImages"), fileName); //Chuyển ảnh vào thư mục
                    //if (System.IO.File.Exists(path))
                    //{
                    //    ViewBag.upload = "Hình đã tồn tại";
                    //    return View();
                    //}
                    //else
                    {
                       hinhAnh.SaveAs(path);
                       diaDiem.HinhAnh = fileName;
                    }
                }
                else
                    diaDiem.HinhAnh = "unknown.png";
            }
           
            diaDiem.The_Geom = DbGeometry.FromText(The_Geom_WKT);

            if (ModelState.IsValid)
            {
                db.DiaDiems.Add(diaDiem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaLoai = new SelectList(db.LoaiDiaDiems, "MaLoai", "TenLoai", diaDiem.MaLoai);
            return View(diaDiem);
        }

        // GET: DiaDiems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiaDiem diaDiem = db.DiaDiems.Find(id);
            if (diaDiem == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoai = new SelectList(db.LoaiDiaDiems, "MaLoai", "TenLoai", diaDiem.MaLoai);
            return View(diaDiem);
        }

        // POST: DiaDiems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TenDiaDiem,MaLoai,DiaChi,ThoiGianPhucVu,The_Geom,HinhAnh,MoTa")] DiaDiem diaDiem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(diaDiem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaLoai = new SelectList(db.LoaiDiaDiems, "MaLoai", "TenLoai", diaDiem.MaLoai);
            return View(diaDiem);
        }

        // GET: DiaDiems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiaDiem diaDiem = db.DiaDiems.Find(id);
            if (diaDiem == null)
            {
                return HttpNotFound();
            }
            return View(diaDiem);
        }

        // POST: DiaDiems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DiaDiem diaDiem = db.DiaDiems.Find(id);
            db.DiaDiems.Remove(diaDiem);
            db.SaveChanges();
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
