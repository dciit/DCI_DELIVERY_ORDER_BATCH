using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DCI_DELIVERY_ORDER_BATCH.Models;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DCI_DELIVERY_ORDER_BATCH.Contexts
{
    public partial class DBSCM : DbContext
    {
        public DBSCM()
        {
        }

        public DBSCM(DbContextOptions<DBSCM> options)
            : base(options)
        {
        }

        public virtual DbSet<AlPart> AlPart { get; set; }
        public virtual DbSet<DoDictMstr> DoDictMstr { get; set; }
        public virtual DbSet<DoPartMaster> DoPartMaster { get; set; }
        public virtual DbSet<DoStockAlpha> DoStockAlpha { get; set; }
        public virtual DbSet<ViDoPlan> ViDoPlan { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=192.168.226.86;Database=dbSCM;TrustServerCertificate=True;uid=sa;password=decjapan");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlPart>(entity =>
            {
                entity.HasKey(e => new { e.DrawingNo, e.Cm, e.VenderCode });

                entity.ToTable("AL_Part");

                entity.Property(e => e.DrawingNo).HasMaxLength(50);

                entity.Property(e => e.Cm)
                    .HasColumnName("CM")
                    .HasMaxLength(2);

                entity.Property(e => e.VenderCode).HasMaxLength(20);

                entity.Property(e => e.Catmat)
                    .HasColumnName("CATMAT")
                    .HasMaxLength(1);

                entity.Property(e => e.CnvCode).HasMaxLength(50);

                entity.Property(e => e.CnvWt)
                    .HasColumnName("CnvWT")
                    .HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Description).HasMaxLength(300);

                entity.Property(e => e.Ivunit)
                    .HasColumnName("IVUnit")
                    .HasMaxLength(5);

                entity.Property(e => e.OrderType).HasMaxLength(1);

                entity.Property(e => e.QtyBox).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Remark1).HasMaxLength(255);

                entity.Property(e => e.Route).HasMaxLength(1);

                entity.Property(e => e.UpdateBy).HasMaxLength(25);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.Property(e => e.VenderName).HasMaxLength(300);

                entity.Property(e => e.Whunit)
                    .HasColumnName("WHUnit")
                    .HasMaxLength(5);
            });

            modelBuilder.Entity<DoDictMstr>(entity =>
            {
                entity.HasKey(e => e.DictId);

                entity.ToTable("DO_DictMstr");

                entity.Property(e => e.DictId).HasColumnName("DICT_ID");

                entity.Property(e => e.Code)
                    .HasColumnName("CODE")
                    .HasMaxLength(20);

                entity.Property(e => e.CreateDate)
                    .HasColumnName("CREATE_DATE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(50);

                entity.Property(e => e.DictStatus)
                    .HasColumnName("DICT_STATUS")
                    .HasMaxLength(20);

                entity.Property(e => e.DictType)
                    .HasColumnName("DICT_TYPE")
                    .HasMaxLength(20);

                entity.Property(e => e.Note)
                    .HasColumnName("NOTE")
                    .HasMaxLength(50);

                entity.Property(e => e.RefCode)
                    .HasColumnName("REF_CODE")
                    .HasMaxLength(20);

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("UPDATE_DATE")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<DoPartMaster>(entity =>
            {
                entity.HasKey(e => e.PartId);

                entity.ToTable("DO_PART_MASTER");

                entity.Property(e => e.PartId).HasColumnName("PART_ID");

                entity.Property(e => e.BoxMax).HasColumnName("BOX_MAX");

                entity.Property(e => e.BoxMin).HasColumnName("BOX_MIN");

                entity.Property(e => e.BoxQty).HasColumnName("BOX_QTY");

                entity.Property(e => e.Cm)
                    .IsRequired()
                    .HasColumnName("CM")
                    .HasMaxLength(5);

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(50);

                entity.Property(e => e.Partno)
                    .IsRequired()
                    .HasColumnName("PARTNO")
                    .HasMaxLength(30);

                entity.Property(e => e.Pdlt).HasColumnName("PDLT");

                entity.Property(e => e.Unit)
                    .HasColumnName("UNIT")
                    .HasMaxLength(10);

                entity.Property(e => e.VdCode)
                    .HasColumnName("VD_CODE")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<DoStockAlpha>(entity =>
            {
                entity.HasKey(e => new { e.Partno, e.DatePd, e.Vdcode, e.Cm });

                entity.ToTable("DO_STOCK_ALPHA");

                entity.Property(e => e.Partno)
                    .HasColumnName("PARTNO")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DatePd)
                    .HasColumnName("DATE_PD")
                    .HasColumnType("date")
                    .HasComment("วันที่ มี STOCK ALPHA");

                entity.Property(e => e.Vdcode)
                    .HasColumnName("VDCODE")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Cm)
                    .HasColumnName("CM")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.InsertDt)
                    .HasColumnName("INSERT_DT")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Rev)
                    .HasColumnName("REV")
                    .HasComment("999 คือใช้งาน นอกนั้นให้เก็บเป็น REV = 1, 2 , 3 , ....");

                entity.Property(e => e.Stock).HasColumnName("STOCK");
            });

            modelBuilder.Entity<ViDoPlan>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vi_DO_Plan");

                entity.Property(e => e.Catmat)
                    .HasColumnName("CATMAT")
                    .HasMaxLength(1);

                entity.Property(e => e.Cm)
                    .HasColumnName("CM")
                    .HasMaxLength(1);

                entity.Property(e => e.Consumption)
                    .HasColumnName("CONSUMPTION")
                    .HasColumnType("decimal(38, 6)");

                entity.Property(e => e.Exp)
                    .HasColumnName("EXP")
                    .HasMaxLength(1);

                entity.Property(e => e.Model)
                    .HasColumnName("MODEL")
                    .HasMaxLength(50);

                entity.Property(e => e.Partname)
                    .HasColumnName("PARTNAME")
                    .HasMaxLength(300);

                entity.Property(e => e.Partno)
                    .HasColumnName("PARTNO")
                    .HasMaxLength(20);

                entity.Property(e => e.Prdltymd)
                    .HasColumnName("PRDLTYMD")
                    .HasMaxLength(8);

                entity.Property(e => e.Prdym)
                    .HasColumnName("PRDYM")
                    .HasMaxLength(6);

                entity.Property(e => e.Prdymd)
                    .HasColumnName("PRDYMD")
                    .HasMaxLength(8);

                entity.Property(e => e.Qty)
                    .HasColumnName("QTY")
                    .HasColumnType("decimal(10, 0)");

                entity.Property(e => e.Ratio)
                    .HasColumnName("RATIO")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Reqqty)
                    .HasColumnName("REQQTY")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Route)
                    .HasColumnName("ROUTE")
                    .HasMaxLength(1);

                entity.Property(e => e.Vender)
                    .HasColumnName("VENDER")
                    .HasMaxLength(10);

                entity.Property(e => e.Wcno).HasColumnName("WCNO");

                entity.Property(e => e.Whunit)
                    .HasColumnName("WHUNIT")
                    .HasMaxLength(10);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
