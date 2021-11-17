namespace TN213_MuaSamCanTho.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaiKhoanQuanTri")]
    public partial class TaiKhoanQuanTri
    {
        [Key]
        public int MaNguoiDung { get; set; }

        [StringLength(50)]
        public string TenDangNhap { get; set; }

        [StringLength(50)]
        public string MatKhau { get; set; }

        [StringLength(50)]
        public string TenNguoiDung { get; set; }
    }
}
