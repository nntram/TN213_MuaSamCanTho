namespace TN213_MuaSamCanTho.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BinhLuan")]
    public partial class BinhLuan
    {
        [Key]
        public int MaBinhLuan { get; set; }

        public int? MaNguoiDung { get; set; }

        public int? MaDiaDiem { get; set; }

        [StringLength(500)]
        public string NoiDungBinhLuan { get; set; }

        public virtual DiaDiem DiaDiem { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}
