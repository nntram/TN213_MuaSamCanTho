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
    public class BinhLuansController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: BinhLuans
        public ActionResult Index()
        {
            var binhLuans = db.BinhLuans.Include(b => b.DiaDiem).Include(b => b.TaiKhoan).OrderByDescending(b=>b.ThoiGianBinhLuan);
            return View(binhLuans.ToList());
        }

       

        // GET: BinhLuans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BinhLuan binhLuan = db.BinhLuans.Find(id);
            if (binhLuan == null)
            {
                return HttpNotFound();
            }
            return View(binhLuan);
        }

        // POST: BinhLuans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BinhLuan binhLuan = db.BinhLuans.Find(id);
            db.BinhLuans.Remove(binhLuan);
            db.SaveChanges();
            TempData["success"] = "Đã xóa!";
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
