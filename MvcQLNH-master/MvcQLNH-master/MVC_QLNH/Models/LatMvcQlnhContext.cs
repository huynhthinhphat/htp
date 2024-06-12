using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MVC_QLNH.Models;

public partial class LatMvcQlnhContext : DbContext
{
    public LatMvcQlnhContext()
    {
    }

    public LatMvcQlnhContext(DbContextOptions<LatMvcQlnhContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbAccount> TbAccounts { get; set; }

    public virtual DbSet<TbBillDetail> TbBillDetails { get; set; }

    public virtual DbSet<TbBillHistory> TbBillHistories { get; set; }

    public virtual DbSet<TbDmfood> TbDmfoods { get; set; }

    public virtual DbSet<TbDstable> TbDstables { get; set; }

    public virtual DbSet<TbFood> TbFoods { get; set; }

    public virtual DbSet<TbReport> TbReports { get; set; }

    public virtual DbSet<TbRevenu> TbRevenus { get; set; }

    public virtual DbSet<TbUserInfo> TbUserInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=HTP\\SQLEXPRESS02;Initial Catalog=LAT_MvcQLNH;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<TbAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__tbAccoun__349DA586B84F2663");

            entity.ToTable("tbAccount");

            entity.HasIndex(e => e.Username, "UQ__tbAccoun__536C85E4F8B56343").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.AccountType).HasMaxLength(20);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<TbBillDetail>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("PK__tbBillDe__11F2FC4A438139CA");

            entity.ToTable("tbBillDetails");

            entity.Property(e => e.BillId).HasColumnName("BillID");
            entity.Property(e => e.CustomerName).HasMaxLength(100);
            entity.Property(e => e.FoodName).HasMaxLength(100);
            entity.Property(e => e.Sdt).HasColumnName("SDT");
            entity.Property(e => e.TableName).HasMaxLength(50);
        });

        modelBuilder.Entity<TbBillHistory>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("PK__tbBillHi__11F2FC4A1ACA0CAF");

            entity.ToTable("tbBillHistory");

            entity.Property(e => e.BillId).HasColumnName("BillID");
            entity.Property(e => e.FoodName).HasColumnName("FoodName");
            entity.Property(e => e.Quantity).HasColumnName("Quantity");
            entity.Property(e => e.Price).HasColumnName("Price");
            entity.Property(e => e.TableName).HasColumnName("TableName");
            entity.Property(e => e.BillDate).HasColumnName("BillDate");
            entity.Property(e => e.TotalAmount).HasColumnName("TotalAmount");
            entity.Property(e => e.UserInfoId).HasColumnName("UserInfoID");
            entity.Property(e => e.CustomerName).HasColumnName("CustomerName");
            entity.Property(e => e.Sdt).HasColumnName("Sdt");

            entity.HasOne(d => d.UserInfo).WithMany(p => p.TbBillHistories)
                .HasForeignKey(d => d.UserInfoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__tbBillHis__UserI__5AEE82B9");
        });

        modelBuilder.Entity<TbDmfood>(entity =>
        {
            entity.HasKey(e => e.DmfoodId).HasName("PK__tbDMFood__3DFF7D6BEB54A8F8");

            entity.ToTable("tbDMFood");

            entity.Property(e => e.DmfoodId).HasColumnName("DMFoodID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<TbDstable>(entity =>
        {
            entity.HasKey(e => e.TableId).HasName("PK__tbDSTabl__7D5F018E93B737D6");

            entity.ToTable("tbDSTable");

            entity.Property(e => e.TableId).HasColumnName("TableID");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.TableName).HasMaxLength(50);
        });

        modelBuilder.Entity<TbFood>(entity =>
        {
            entity.HasKey(e => e.FoodId).HasName("PK__tbFood__856DB3CB54312C1D");

            entity.ToTable("tbFood");

            entity.Property(e => e.FoodId).HasColumnName("FoodID");
            entity.Property(e => e.AvtFood)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DmfoodId).HasColumnName("DMFoodID");
            entity.Property(e => e.FoodName).HasMaxLength(100);

            entity.HasOne(d => d.Dmfood).WithMany(p => p.TbFoods)
                .HasForeignKey(d => d.DmfoodId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__tbFood__DMFoodID__5BE2A6F2");
        });

        modelBuilder.Entity<TbReport>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__tbReport__D5BD48E56E48D43F");

            entity.ToTable("tbReport");

            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.DateCheckin)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateCheckout).HasColumnType("datetime");
            entity.Property(e => e.Idbill).HasColumnName("IDBill");
            entity.Property(e => e.Slban).HasColumnName("SLBan");

            entity.HasOne(d => d.IdbillNavigation).WithMany(p => p.TbReports)
                .HasForeignKey(d => d.Idbill)
                .HasConstraintName("FK__tbReport__IDBill__5CD6CB2B");

            entity.HasOne(d => d.SlbanNavigation).WithMany(p => p.TbReports)
                .HasForeignKey(d => d.Slban)
                .HasConstraintName("FK__tbReport__SLBan__5DCAEF64");
        });

        modelBuilder.Entity<TbRevenu>(entity =>
        {
            entity.HasKey(e => e.RevenuId).HasName("PK__tbRevenu__FBB5DE1D247D729B");

            entity.ToTable("tbRevenu");

            entity.Property(e => e.RevenuId).HasColumnName("RevenuID");

            entity.HasOne(d => d.SlTableNavigation).WithMany(p => p.TbRevenus)
                .HasForeignKey(d => d.SlTable)
                .HasConstraintName("FK__tbRevenu__SlTabl__5EBF139D");
        });

        modelBuilder.Entity<TbUserInfo>(entity =>
        {
            entity.HasKey(e => e.UserInfoId).HasName("PK__tbUserIn__D07EF2C47E2D5890");

            entity.ToTable("tbUserInfo");

            entity.Property(e => e.UserInfoId).HasColumnName("UserInfoID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);

            entity.HasOne(d => d.Account).WithMany(p => p.TbUserInfos)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__tbUserInf__Accou__5FB337D6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
