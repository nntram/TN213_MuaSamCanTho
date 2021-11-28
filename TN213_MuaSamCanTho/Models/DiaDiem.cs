namespace TN213_MuaSamCanTho.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DiaDiem")]
    public partial class DiaDiem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DiaDiem()
        {
            BinhLuans = new HashSet<BinhLuan>();
        }

        [Key]
        public int MaDiaDiem { get; set; }

        [StringLength(255)]
        public string TenDiaDiem { get; set; }

        public int? MaLoai { get; set; }

        [StringLength(255)]
        public string DiaChi { get; set; }

        [StringLength(100)]
        public string ThoiGianPhucVu { get; set; }

        public DbGeometry The_Geom { get; set; }

        [StringLength(255)]
        public string HinhAnh { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BinhLuan> BinhLuans { get; set; }

        public virtual LoaiDiaDiem LoaiDiaDiem { get; set; }
    }
}
