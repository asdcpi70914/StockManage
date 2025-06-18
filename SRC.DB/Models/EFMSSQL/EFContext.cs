using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SRC.DB.Models.EFMSSQL;

public partial class EFContext : DbContext
{
    public EFContext()
    {
    }

    public EFContext(DbContextOptions<EFContext> options)
        : base(options)
    {
    }

    public virtual DbSet<backend_dept> backend_depts { get; set; }

    public virtual DbSet<backend_unit> backend_units { get; set; }

    public virtual DbSet<backend_user> backend_users { get; set; }

    public virtual DbSet<backend_users_del> backend_users_dels { get; set; }

    public virtual DbSet<backend_users_role> backend_users_roles { get; set; }

    public virtual DbSet<equipment_maintain> equipment_maintains { get; set; }

    public virtual DbSet<factoryinfo_maintain> factoryinfo_maintains { get; set; }

    public virtual DbSet<func> funcs { get; set; }

    public virtual DbSet<min_base_stock_subscribe_setting> min_base_stock_subscribe_settings { get; set; }

    public virtual DbSet<role> roles { get; set; }

    public virtual DbSet<role_del> role_dels { get; set; }

    public virtual DbSet<role_func> role_funcs { get; set; }

    public virtual DbSet<stockin_log> stockin_logs { get; set; }

    public virtual DbSet<subscribepoint_maintain> subscribepoint_maintains { get; set; }

    public virtual DbSet<system_city_code> system_city_codes { get; set; }

    public virtual DbSet<system_code> system_codes { get; set; }

    public virtual DbSet<system_towncode> system_towncodes { get; set; }

    public virtual DbSet<unit_apply> unit_applies { get; set; }

    public virtual DbSet<unit_apply_review_log> unit_apply_review_logs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<backend_dept>(entity =>
        {
            entity.HasKey(e => e.pid);

            entity.ToTable("backend_dept");

            entity.Property(e => e.code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.create_time).HasColumnType("datetime");

            entity.HasOne(d => d.backend_user_p).WithMany(p => p.backend_depts)
                .HasForeignKey(d => d.backend_user_pid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_backend_dept_backend_users");
        });

        modelBuilder.Entity<backend_unit>(entity =>
        {
            entity.HasKey(e => e.pid);

            entity.Property(e => e.code)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<backend_user>(entity =>
        {
            entity.HasKey(e => e.pid);

            entity.HasIndex(e => e.user_id, "UQ_USER_ID").IsUnique();

            entity.Property(e => e.access_failed_count).HasDefaultValueSql("((0))");
            entity.Property(e => e.account)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ad_account)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.apply_date).HasColumnType("datetime");
            entity.Property(e => e.changed_password_time).HasColumnType("datetime");
            entity.Property(e => e.create_time)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.creator)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.device_code)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.edit_time).HasColumnType("datetime");
            entity.Property(e => e.editor)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.email).HasMaxLength(200);
            entity.Property(e => e.email_confirmed).HasDefaultValueSql("((0))");
            entity.Property(e => e.email_confirmed_time).HasColumnType("datetime");
            entity.Property(e => e.first_login_time).HasColumnType("datetime");
            entity.Property(e => e.jwt_code)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.limit_time).HasColumnType("datetime");
            entity.Property(e => e.lockout_end).HasColumnType("datetime");
            entity.Property(e => e.name_ch).HasMaxLength(80);
            entity.Property(e => e.name_en)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.password_hash).HasMaxLength(256);
            entity.Property(e => e.phone_number).HasMaxLength(30);
            entity.Property(e => e.verification_code)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<backend_users_del>(entity =>
        {
            entity.HasKey(e => e.pid);

            entity.ToTable("backend_users_del");

            entity.Property(e => e.account)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ad_account)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.apply_date).HasColumnType("datetime");
            entity.Property(e => e.changed_password_time).HasColumnType("datetime");
            entity.Property(e => e.create_time).HasColumnType("datetime");
            entity.Property(e => e.creator)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.device_code)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.edit_time).HasColumnType("datetime");
            entity.Property(e => e.editor)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.email).HasMaxLength(200);
            entity.Property(e => e.email_confirmed_time).HasColumnType("datetime");
            entity.Property(e => e.first_login_time).HasColumnType("datetime");
            entity.Property(e => e.jwt_code)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.limit_time).HasColumnType("datetime");
            entity.Property(e => e.lockout_end).HasColumnType("datetime");
            entity.Property(e => e.name_ch).HasMaxLength(80);
            entity.Property(e => e.name_en)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.password_hash).HasMaxLength(256);
            entity.Property(e => e.phone_number).HasMaxLength(30);
            entity.Property(e => e.verification_code)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<backend_users_role>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PK_dbo.Member_Role");

            entity.ToTable("backend_users_role", tb => tb.HasComment("角色權限"));

            entity.HasOne(d => d.role).WithMany(p => p.backend_users_roles)
                .HasForeignKey(d => d.role_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_backend_users_role_role");

            entity.HasOne(d => d.user).WithMany(p => p.backend_users_roles)
                .HasPrincipalKey(p => p.user_id)
                .HasForeignKey(d => d.user_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_backend_users_role_backend_users_role");
        });

        modelBuilder.Entity<equipment_maintain>(entity =>
        {
            entity.HasKey(e => e.pid);

            entity.ToTable("equipment_maintain");

            entity.Property(e => e.create_time).HasColumnType("datetime");
            entity.Property(e => e.creator)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.edit_time).HasColumnType("datetime");
            entity.Property(e => e.editor)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.price).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.state)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<factoryinfo_maintain>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PK_companynfo_maintain");

            entity.ToTable("factoryinfo_maintain");

            entity.Property(e => e.address)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.city)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.company_number)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.contact_phone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.create_time).HasColumnType("datetime");
            entity.Property(e => e.creator)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.edit_time).HasColumnType("datetime");
            entity.Property(e => e.editor)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.town)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<func>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PK_dbo.Func");

            entity.ToTable("func", tb => tb.HasComment("功能清單"));

            entity.Property(e => e.create_time).HasColumnType("datetime");
            entity.Property(e => e.creator)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.edit_time).HasColumnType("datetime");
            entity.Property(e => e.editor)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.icon)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.memo).HasMaxLength(500);
            entity.Property(e => e.name).HasMaxLength(100);
            entity.Property(e => e.type)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.url).HasMaxLength(400);
        });

        modelBuilder.Entity<min_base_stock_subscribe_setting>(entity =>
        {
            entity.HasKey(e => e.pid);

            entity.ToTable("min_base_stock_subscribe_setting");

            entity.Property(e => e.create_time).HasColumnType("datetime");
            entity.Property(e => e.creator)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.edit_time).HasColumnType("datetime");
            entity.Property(e => e.editor)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.sub_pid).HasComment("器材或是裝備pid");
            entity.Property(e => e.type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<role>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PK_dbo.Role");

            entity.ToTable("role", tb => tb.HasComment("權限"));

            entity.Property(e => e.create_time).HasColumnType("datetime");
            entity.Property(e => e.creator)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.edit_time).HasColumnType("datetime");
            entity.Property(e => e.editor)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.name).HasMaxLength(50);
            entity.Property(e => e.programe_code)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<role_del>(entity =>
        {
            entity.HasKey(e => e.pid);

            entity.ToTable("role_del");

            entity.Property(e => e.create_time).HasColumnType("datetime");
            entity.Property(e => e.creator)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.edit_time).HasColumnType("datetime");
            entity.Property(e => e.editor)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.name).HasMaxLength(50);
            entity.Property(e => e.programe_code)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<role_func>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PK_dbo.Role_Func");

            entity.ToTable("role_func", tb => tb.HasComment("功能權限"));
        });

        modelBuilder.Entity<stockin_log>(entity =>
        {
            entity.HasKey(e => e.pid);

            entity.ToTable("stockin_log");

            entity.Property(e => e.create_time).HasColumnType("datetime");
            entity.Property(e => e.creator)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<subscribepoint_maintain>(entity =>
        {
            entity.HasKey(e => e.pid);

            entity.ToTable("subscribepoint_maintain");

            entity.Property(e => e.create_time).HasColumnType("datetime");
            entity.Property(e => e.creator)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.edit_time).HasColumnType("datetime");
            entity.Property(e => e.editor)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<system_city_code>(entity =>
        {
            entity.HasKey(e => e.county_code).HasName("PK_system_city_code_1");

            entity.ToTable("system_city_code");

            entity.Property(e => e.county_code)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.county_code01)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.name)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<system_code>(entity =>
        {
            entity.HasKey(e => e.pid);

            entity.ToTable("system_code");

            entity.Property(e => e.code)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.data).HasMaxLength(128);
            entity.Property(e => e.description).HasMaxLength(128);
            entity.Property(e => e.sub_description).HasMaxLength(128);
        });

        modelBuilder.Entity<system_towncode>(entity =>
        {
            entity.HasKey(e => e.TownCode);

            entity.ToTable("system_towncode");

            entity.Property(e => e.TownCode)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.TownName)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.postalcode)
                .HasMaxLength(3)
                .IsUnicode(false);
        });

        modelBuilder.Entity<unit_apply>(entity =>
        {
            entity.HasKey(e => e.pid);

            entity.ToTable("unit_apply");

            entity.Property(e => e.create_time).HasColumnType("datetime");
            entity.Property(e => e.creator)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.edit_time).HasColumnType("datetime");
            entity.Property(e => e.editor)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.pay_treasury).HasComment("繳庫數量");
            entity.Property(e => e.state)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<unit_apply_review_log>(entity =>
        {
            entity.HasKey(e => e.pid);

            entity.ToTable("unit_apply_review_log");

            entity.Property(e => e.create_time).HasColumnType("datetime");
            entity.Property(e => e.creator)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.memo)
                .HasMaxLength(1024)
                .IsUnicode(false);
            entity.Property(e => e.new_state)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ori_state)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.unit_apply_p).WithMany(p => p.unit_apply_review_logs)
                .HasForeignKey(d => d.unit_apply_pid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_unit_apply_review_log_unit_apply");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
