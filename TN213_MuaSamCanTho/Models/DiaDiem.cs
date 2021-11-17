namespace TN213_MuaSamCanTho.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DiaDiem")]
    public partial class DiaDiem
    {
        public int Id { get; set; }

        [StringLength(255)]
        [DisplayName("Tên địa điểm")]
        public string TenDiaDiem { get; set; }

        [DisplayName("Loại địa điểm")]
        public int? MaLoai { get; set; }

        [DisplayName("Địa chỉ")]
        [StringLength(255)]
        public string DiaChi { get; set; }

        [DisplayName("Thời gian phục vụ")]
        [StringLength(100)]
        public string ThoiGianPhucVu { get; set; }

        [DisplayName("Vị trí")]
        public DbGeometry The_Geom { get; set; }

        [DisplayName("Hình ảnh")]
        [StringLength(255)]
        public string HinhAnh { get; set; }

        [StringLength(500)]
        public string MoTa { get; set; }

        public virtual LoaiDiaDiem LoaiDiaDiem { get; set; }
    }
}
