using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MVC_QLNH.Models;

public partial class SqlMvcQlnhPerfactContext : DbContext
{
    public SqlMvcQlnhPerfactContext()
    {
    }

    public SqlMvcQlnhPerfactContext(DbContextOptions<SqlMvcQlnhPerfactContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbAccount> TbAccounts { get; set; }

    public virtual DbSet<TbBillHistory> TbBillHistories { get; set; }

    public virtual DbSet<TbDmfood> TbDmfoods { get; set; }

    public virtual DbSet<TbDstable> TbDstables { get; set; }

    public virtual DbSet<TbFood> TbFoods { get; set; }

    public virtual DbSet<TbRevenu> TbRevenus { get; set; }

    public virtual DbSet<TbUserInfo> TbUserInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=HTP\\SQLEXPRESS02;Initial Catalog=Sql_Mvc_QLNH_Perfact;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<TbAccount>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__tbAccoun__349DA586A6136ECB");

            entity.ToTable("tbAccount");

            entity.HasIndex(e => e.Username, "UQ__tbAccoun__536C85E41E9F3AAD").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.AccountType).HasMaxLength(20);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<TbBillHistory>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("PK__tbBillHi__11F2FC4AAAD5E2D5");

            entity.ToTable("tbBillHistory");

            entity.Property(e => e.BillId).HasColumnName("BillID");
            entity.Property(e => e.TableID).HasColumnName("TableID");
            entity.Property(e => e.FoodId).HasColumnName("FoodID");
            entity.Property(e => e.Quantity).HasColumnName("Quantity");
            entity.Property(e => e.Price).HasColumnName("Price");
            entity.Property(e => e.TotalPrice).HasColumnName("TotalPrice");
            entity.Property(e => e.UserInfoId).HasColumnName("UserInfoID");
            entity.Property(e => e.BillDate).HasColumnName("BillDate");

            entity.HasOne(d => d.Food).WithMany(p => p.TbBillHistories)
                .HasForeignKey(d => d.FoodId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__tbBillHis__FoodI__5812160E");

            entity.HasOne(d => d.Table).WithMany(p => p.TbBillHistories)
                .HasForeignKey(d => d.TableID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__tbBillHis__Table__5629CD9C");

            entity.HasOne(d => d.UserInfo).WithMany(p => p.TbBillHistories)
                .HasForeignKey(d => d.UserInfoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__tbBillHis__UserI__571DF1D5");
        });

        modelBuilder.Entity<TbDmfood>(entity =>
        {
            entity.HasKey(e => e.DmfoodId).HasName("PK__tbDMFood__3DFF7D6B7C76EF70");

            entity.ToTable("tbDMFood");

            entity.Property(e => e.DmfoodId).HasColumnName("DMFoodID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<TbDstable>(entity =>
        {
            entity.HasKey(e => e.TableId).HasName("PK__tbDSTabl__7D5F018EB3A18054");

            entity.ToTable("tbDSTable");

            entity.Property(e => e.TableId).HasColumnName("TableID");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.TableName).HasMaxLength(50);
        });

        modelBuilder.Entity<TbFood>(entity =>
        {
            entity.HasKey(e => e.FoodId).HasName("PK__tbFood__856DB3CB4EC3396E");

            entity.ToTable("tbFood");

            entity.Property(e => e.FoodId).HasColumnName("FoodID");
            entity.Property(e => e.DmfoodId).HasColumnName("DMFoodID");
            entity.Property(e => e.FoodName).HasColumnName("FoodName");
            entity.Property(e => e.Price).HasColumnName("Price");
            entity.Property(e => e.AvtFood).HasColumnName("AvtFood");

            entity.HasOne(d => d.Dmfood).WithMany(p => p.TbFoods)
                .HasForeignKey(d => d.DmfoodId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__tbFood__DMFoodID__59063A47");
        });

        modelBuilder.Entity<TbRevenu>(entity =>
        {
            entity.HasKey(e => e.RevenuId).HasName("PK__tbRevenu__FBB5DE1D3ACEA80B");

            entity.ToTable("tbRevenu");

            entity.Property(e => e.RevenuId).HasColumnName("RevenuID");
            entity.Property(e => e.BillId).HasColumnName("BillID");

            entity.HasOne(d => d.Bill).WithMany(p => p.TbRevenus)
                .HasForeignKey(d => d.BillId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__tbRevenu__BillID__59FA5E80");
        });

        modelBuilder.Entity<TbUserInfo>(entity =>
        {
            entity.HasKey(e => e.UserInfoId).HasName("PK__tbUserIn__D07EF2C4EDC03C0B");

            entity.ToTable("tbUserInfo");

            entity.Property(e => e.UserInfoId).HasColumnName("UserInfoID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);

            entity.HasOne(d => d.Account).WithMany(p => p.TbUserInfos)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__tbUserInf__Accou__5AEE82B9");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
