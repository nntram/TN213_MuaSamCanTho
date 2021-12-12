using alatas.GeoJSON4EntityFramework;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TN213_MuaSamCanTho.Models;


namespace TN213_MuaSamCanTho.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BanDo()
        {
            return View();
        }

        public String LoadData()
        {

            var db = new ModelDbContext();
            var dlm = db.DiaDiems.ToList();
            var features = new FeatureCollection();
            foreach (var dl in dlm)
            {
                var feature = new Feature(dl.The_Geom);
                feature.Properties.Add("maDiaDiem", dl.MaDiaDiem);
                feature.Properties.Add("hinhAnh", dl.HinhAnh);
                feature.Properties.Add("tenDiaDiem", dl.TenDiaDiem);
                feature.Properties.Add("diaChi", dl.DiaChi);
                feature.Properties.Add("loaiDiaDiem", dl.LoaiDiaDiem.MaLoai);
                feature.Properties.Add("tenLoaiDiaDiem", dl.LoaiDiaDiem.TenLoai);
                feature.Properties.Add("thoiGianPhucVu", dl.ThoiGianPhucVu);
                features.Features.Add(feature);
            }

            return features.Serialize(prettyPrint: true);
        }

        public JsonResult LayTenLoaiKhongTrung()
        {
            var db = new ModelDbContext();
            var dsten = db.LoaiDiaDiems.Select(s => s.TenLoai).Distinct().ToList();
            return Json(dsten, JsonRequestBehavior.AllowGet);

        }

        public String LoadDataByType()
        {
            String name = Request["type"];
            var db = new ModelDbContext();
            var dlm = db.DiaDiems.Where(s => s.LoaiDiaDiem.TenLoai == name).ToList();
            var features = new FeatureCollection();
            foreach (var dl in dlm)
            {
                var feature = new Feature(dl.The_Geom);
                feature.Properties.Add("maDiaDiem", dl.MaDiaDiem);
                feature.Properties.Add("hinhAnh", dl.HinhAnh);
                feature.Properties.Add("tenDiaDiem", dl.TenDiaDiem);
                feature.Properties.Add("diaChi", dl.DiaChi);
                feature.Properties.Add("loaiDiaDiem", dl.LoaiDiaDiem.MaLoai);
                feature.Properties.Add("tenLoaiDiaDiem", dl.LoaiDiaDiem.TenLoai);
                feature.Properties.Add("thoiGianPhucVu", dl.ThoiGianPhucVu);

                features.Features.Add(feature);
            }

            return features.Serialize(prettyPrint: true);
        }

        public String FindLocationWithDistance()
        {
            var longtiude = Request["long"];
            var latitude = Request["lat"];
            var value = Request["val"];

            var sqlQueryClosest =
                "SELECT * " +
                "FROM DiaDiem " +
                "WHERE ROUND(geography::STGeomFromText(THE_GEOM.STAsText(), 4326).STDistance(geography::STGeomFromText('POINT(" + longtiude + " " + latitude + ")', 4326))/1000, 2) <" + value;

            var db = new ModelDbContext();
            var dlm = db.DiaDiems.SqlQuery(sqlQueryClosest);
            var features = new FeatureCollection();
            foreach (var dl in dlm)
            {
                var feature = new Feature(dl.The_Geom);
                feature.Properties.Add("maDiaDiem", dl.MaDiaDiem);
                feature.Properties.Add("hinhAnh", dl.HinhAnh);
                feature.Properties.Add("tenDiaDiem", dl.TenDiaDiem);
                feature.Properties.Add("diaChi", dl.DiaChi);
                feature.Properties.Add("loaiDiaDiem", dl.LoaiDiaDiem.MaLoai);
                feature.Properties.Add("tenLoaiDiaDiem", dl.LoaiDiaDiem.TenLoai);
                feature.Properties.Add("thoiGianPhucVu", dl.ThoiGianPhucVu);
                
                features.Features.Add(feature);
            }

            return features.Serialize(prettyPrint: true);
        }

        [ChildActionOnly]
        public ActionResult ThongBaoPartial()
        {
            return PartialView();
        }

        public ActionResult TimDuongDi()
        {
            return View();
        }



        public ActionResult XemChiTiet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelDbContext db = new ModelDbContext();
            DiaDiem diaDiem = db.DiaDiems.Find(id);
            if (diaDiem == null)
            {
                return HttpNotFound();
            }
            var lstBinhLuan = db.BinhLuans.Where(n => n.MaDiaDiem == id).OrderBy(n=>n.ThoiGianBinhLuan);
            ViewBag.DsBinhLuan = lstBinhLuan;
            return View(diaDiem);
        }

    }
}