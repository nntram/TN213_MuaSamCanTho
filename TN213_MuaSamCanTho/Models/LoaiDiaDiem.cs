namespace TN213_MuaSamCanTho.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LoaiDiaDiem")]
    public partial class LoaiDiaDiem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LoaiDiaDiem()
        {
            DiaDiems = new HashSet<DiaDiem>();
        }

        [Key]
        public int MaLoai { get; set; }

        [StringLength(50)]
        public string TenLoai { get; set; }

        [StringLength(50)]
        public string GhiChu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiaDiem> DiaDiems { get; set; }
    }
}
