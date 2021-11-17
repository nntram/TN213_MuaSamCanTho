using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace TN213_MuaSamCanTho.Models
{
    public partial class ModelDbContext : DbContext
    {
        public ModelDbContext()
            : base("name=ModelDbContext")
        {
        }

        public virtual DbSet<DiaDiem> DiaDiems { get; set; }
        public virtual DbSet<LoaiDiaDiem> LoaiDiaDiems { get; set; }
        public virtual DbSet<TaiKhoanQuanTri> TaiKhoanQuanTris { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
