using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SRC.DB.Models.EFMYSQL;

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

    public virtual DbSet<dismantle_picture_user> dismantle_picture_users { get; set; }

    public virtual DbSet<firm_maintain> firm_maintains { get; set; }

    public virtual DbSet<func> funcs { get; set; }

    public virtual DbSet<process_maintain> process_maintains { get; set; }

    public virtual DbSet<product_type_maintain> product_type_maintains { get; set; }

    public virtual DbSet<project> projects { get; set; }

    public virtual DbSet<project_no> project_nos { get; set; }

    public virtual DbSet<project_other_image> project_other_images { get; set; }

    public virtual DbSet<project_review_log> project_review_logs { get; set; }

    public virtual DbSet<project_state_log> project_state_logs { get; set; }

    public virtual DbSet<projects_assign_log> projects_assign_logs { get; set; }

    public virtual DbSet<projects_file> projects_files { get; set; }

    public virtual DbSet<projects_outsource> projects_outsources { get; set; }

    public virtual DbSet<projects_outsource_log> projects_outsource_logs { get; set; }

    public virtual DbSet<projects_outsource_relation> projects_outsource_relations { get; set; }

    public virtual DbSet<role> roles { get; set; }

    public virtual DbSet<role_del> role_dels { get; set; }

    public virtual DbSet<role_func> role_funcs { get; set; }

    public virtual DbSet<system_code> system_codes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<backend_dept>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PRIMARY");

            entity.ToTable("backend_dept");

            entity.HasIndex(e => e.backend_user_pid, "FK_backend_dept_backend_users");

            entity.Property(e => e.code).HasMaxLength(50);
            entity.Property(e => e.create_time).HasColumnType("datetime");

            entity.HasOne(d => d.backend_user_p).WithMany(p => p.backend_depts)
                .HasForeignKey(d => d.backend_user_pid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_backend_dept_backend_users");
        });

        modelBuilder.Entity<backend_unit>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("單位資料"));

            entity.Property(e => e.pid).HasComment("單位流水號");
            entity.Property(e => e.code)
                .HasMaxLength(10)
                .HasComment("單位編碼");
            entity.Property(e => e.name)
                .HasMaxLength(50)
                .HasComment("單位名稱");
        });

        modelBuilder.Entity<backend_user>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PRIMARY");

            entity.HasIndex(e => e.user_id, "user_id").IsUnique();

            entity.Property(e => e.account).HasMaxLength(30);
            entity.Property(e => e.ad_account).HasMaxLength(30);
            entity.Property(e => e.apply_date).HasColumnType("datetime");
            entity.Property(e => e.changed_password_time).HasColumnType("datetime");
            entity.Property(e => e.create_time).HasColumnType("datetime");
            entity.Property(e => e.creator).HasMaxLength(30);
            entity.Property(e => e.device_code).HasMaxLength(256);
            entity.Property(e => e.edit_time).HasColumnType("datetime");
            entity.Property(e => e.editor).HasMaxLength(30);
            entity.Property(e => e.email).HasMaxLength(512);
            entity.Property(e => e.email_confirmed_time).HasColumnType("datetime");
            entity.Property(e => e.first_login_time).HasColumnType("datetime");
            entity.Property(e => e.jwt_code).HasMaxLength(512);
            entity.Property(e => e.limit_time).HasColumnType("datetime");
            entity.Property(e => e.lockout_end).HasColumnType("datetime");
            entity.Property(e => e.name_ch).HasMaxLength(80);
            entity.Property(e => e.name_en).HasMaxLength(50);
            entity.Property(e => e.password_hash).HasMaxLength(256);
            entity.Property(e => e.person_in_charge).HasComment("負責人");
            entity.Property(e => e.phone_number).HasMaxLength(512);
            entity.Property(e => e.unit).HasMaxLength(20);
            entity.Property(e => e.user_id).HasDefaultValueSql("''");
            entity.Property(e => e.verification_code)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<backend_users_del>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PRIMARY");

            entity.ToTable("backend_users_del");

            entity.Property(e => e.account).HasMaxLength(30);
            entity.Property(e => e.apply_date).HasColumnType("datetime");
            entity.Property(e => e.changed_password_time).HasColumnType("datetime");
            entity.Property(e => e.create_time).HasColumnType("datetime");
            entity.Property(e => e.creator).HasMaxLength(30);
            entity.Property(e => e.device_code).HasMaxLength(256);
            entity.Property(e => e.edit_time).HasColumnType("datetime");
            entity.Property(e => e.editor).HasMaxLength(30);
            entity.Property(e => e.email).HasMaxLength(200);
            entity.Property(e => e.email_confirmed_time).HasColumnType("datetime");
            entity.Property(e => e.first_login_time).HasColumnType("datetime");
            entity.Property(e => e.jwt_code).HasMaxLength(512);
            entity.Property(e => e.limit_time).HasColumnType("datetime");
            entity.Property(e => e.lockout_end).HasColumnType("datetime");
            entity.Property(e => e.name_ch).HasMaxLength(80);
            entity.Property(e => e.name_en).HasMaxLength(50);
            entity.Property(e => e.password_hash).HasMaxLength(256);
            entity.Property(e => e.phone_number).HasMaxLength(30);
            entity.Property(e => e.unit).HasMaxLength(20);
            entity.Property(e => e.user_id).HasDefaultValueSql("''");
            entity.Property(e => e.verification_code)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<backend_users_role>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PRIMARY");

            entity.ToTable("backend_users_role");

            entity.HasIndex(e => e.user_id, "FK_backend_users_role_backend_users");

            entity.HasIndex(e => e.role_id, "FK_backend_users_role_role");

            entity.HasOne(d => d.role).WithMany(p => p.backend_users_roles)
                .HasForeignKey(d => d.role_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_backend_users_role_role");

            entity.HasOne(d => d.user).WithMany(p => p.backend_users_roles)
                .HasPrincipalKey(p => p.user_id)
                .HasForeignKey(d => d.user_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_backend_users_role_backend_users");
        });

        modelBuilder.Entity<dismantle_picture_user>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PRIMARY");

            entity.ToTable("dismantle_picture_user");

            entity.Property(e => e.pid).HasComment("流水號");
            entity.Property(e => e.create_time).HasColumnType("datetime");
            entity.Property(e => e.creator).HasMaxLength(50);
            entity.Property(e => e.project_pid).HasComment("專案pid");
            entity.Property(e => e.user_pid).HasComment("拆圖人員");
        });

        modelBuilder.Entity<firm_maintain>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PRIMARY");

            entity.ToTable("firm_maintain", tb => tb.HasComment("廠商維護表"));

            entity.Property(e => e.pid).HasComment("流水號");
            entity.Property(e => e.create_time)
                .HasComment("建立時間")
                .HasColumnType("datetime");
            entity.Property(e => e.creator)
                .HasMaxLength(50)
                .HasComment("建立人員");
            entity.Property(e => e.edit_time)
                .HasComment("修改時間")
                .HasColumnType("datetime");
            entity.Property(e => e.editor)
                .HasMaxLength(50)
                .HasComment("修改人員");
            entity.Property(e => e.firm_name)
                .HasMaxLength(50)
                .HasComment("廠商名稱");
        });

        modelBuilder.Entity<func>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PRIMARY");

            entity.ToTable("func");

            entity.Property(e => e.create_time).HasColumnType("datetime");
            entity.Property(e => e.creator).HasMaxLength(30);
            entity.Property(e => e.edit_time).HasColumnType("datetime");
            entity.Property(e => e.editor).HasMaxLength(30);
            entity.Property(e => e.icon).HasMaxLength(50);
            entity.Property(e => e.memo).HasMaxLength(50);
            entity.Property(e => e.name).HasMaxLength(100);
            entity.Property(e => e.type).HasMaxLength(15);
            entity.Property(e => e.url).HasMaxLength(400);
        });

        modelBuilder.Entity<process_maintain>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PRIMARY");

            entity.ToTable("process_maintain", tb => tb.HasComment("製程維護表"));

            entity.Property(e => e.pid).HasComment("流水號");
            entity.Property(e => e.create_time)
                .HasComment("建立時間")
                .HasColumnType("datetime");
            entity.Property(e => e.creator)
                .HasMaxLength(50)
                .HasComment("建立人員");
            entity.Property(e => e.edit_time)
                .HasComment("修改時間")
                .HasColumnType("datetime");
            entity.Property(e => e.editor)
                .HasMaxLength(50)
                .HasComment("修改人員");
            entity.Property(e => e.process_name)
                .HasMaxLength(50)
                .HasComment("製程名稱");
        });

        modelBuilder.Entity<product_type_maintain>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PRIMARY");

            entity.ToTable("product_type_maintain");

            entity.Property(e => e.pid).HasComment("流水號");
            entity.Property(e => e.create_time)
                .HasComment("建立時間")
                .HasColumnType("datetime");
            entity.Property(e => e.creator)
                .HasDefaultValueSql("''")
                .HasComment("建立人員");
            entity.Property(e => e.edit_time)
                .HasComment("修改時間")
                .HasColumnType("datetime");
            entity.Property(e => e.editor).HasComment("修改人員");
            entity.Property(e => e.product_type_name)
                .HasMaxLength(20)
                .HasComment("產品類別名稱");
        });

        modelBuilder.Entity<project>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("專案追蹤表"));

            entity.Property(e => e.appearance_handle).HasComment("外觀處理");
            entity.Property(e => e.appraise_number)
                .HasMaxLength(10)
                .HasDefaultValueSql("''")
                .HasComment("估價單號");
            entity.Property(e => e.artwork).HasComment("Artwork");
            entity.Property(e => e.artwork_datetime).HasColumnType("datetime");
            entity.Property(e => e.build_create_time)
                .HasComment("組裝完成時間的儲存時間")
                .HasColumnType("datetime");
            entity.Property(e => e.build_time)
                .HasComment("組裝完成時間")
                .HasColumnType("datetime");
            entity.Property(e => e.builder).HasComment("組裝完成時間建立者");
            entity.Property(e => e.bulid_inspection_after_time)
                .HasComment("組裝後品檢時間")
                .HasColumnType("datetime");
            entity.Property(e => e.bulid_inspection_before_time)
                .HasComment("組裝前品檢時間")
                .HasColumnType("datetime");
            entity.Property(e => e.business_manager).HasComment("業務負責人");
            entity.Property(e => e.cmf).HasComment("CMF");
            entity.Property(e => e.cmf_datetime).HasColumnType("datetime");
            entity.Property(e => e.complete_time)
                .HasComment("完工時間")
                .HasColumnType("datetime");
            entity.Property(e => e.consign_materia).HasComment("客供件(色板(樣))");
            entity.Property(e => e.consign_materia2).HasComment("客供件(零件)");
            entity.Property(e => e.consign_materia3).HasComment("客供件(其他)");
            entity.Property(e => e.contact_phone)
                .HasMaxLength(20)
                .HasComment("連絡電話");
            entity.Property(e => e.counter_weight).HasComment("配重");
            entity.Property(e => e.counter_weight_amount)
                .HasMaxLength(50)
                .HasComment("配種重量(g)");
            entity.Property(e => e.craftrd_datetime)
                .HasComment("工藝研發預計完成日期")
                .HasColumnType("datetime");
            entity.Property(e => e.craftrd_manager).HasComment("工藝研發部負責人");
            entity.Property(e => e.create_time)
                .HasComment("建立時間")
                .HasColumnType("datetime");
            entity.Property(e => e.creator)
                .HasDefaultValueSql("''")
                .HasComment("建立人員");
            entity.Property(e => e.cust_build_inspection_after_time)
                .HasComment("客戶需求時間_組裝後品檢時間")
                .HasColumnType("datetime");
            entity.Property(e => e.cust_build_inspection_before_time)
                .HasComment("客戶需求時間_組裝前品檢時間")
                .HasColumnType("datetime");
            entity.Property(e => e.cust_build_time)
                .HasComment("客戶需求時間_自身組裝完成時間")
                .HasColumnType("datetime");
            entity.Property(e => e.cust_craftrd_datetime)
                .HasComment("客戶需求時間_工藝研發預計完成時間")
                .HasColumnType("datetime");
            entity.Property(e => e.cust_dismantle_picture_estimated_time)
                .HasComment("客戶需求時間_拆圖出圖預計時間")
                .HasColumnType("datetime");
            entity.Property(e => e.cust_dismantle_picture_real_time)
                .HasComment("客戶需求時間_拆圖出圖實際時間")
                .HasColumnType("datetime");
            entity.Property(e => e.cust_picture_supply_time)
                .HasComment("客戶需求時間_照片提供時間")
                .HasColumnType("datetime");
            entity.Property(e => e.cust_prototypework_datetime)
                .HasComment("客戶需求時間_原型加工預計完成時間")
                .HasColumnType("datetime");
            entity.Property(e => e.customer_no)
                .HasMaxLength(10)
                .HasComment("客戶編號");
            entity.Property(e => e.customer_type)
                .HasMaxLength(1)
                .HasDefaultValueSql("''")
                .HasComment("0：國內 1：國外");
            entity.Property(e => e.customer_window)
                .HasMaxLength(30)
                .HasComment("客戶窗口");
            entity.Property(e => e.deadline)
                .HasComment("交件日期")
                .HasColumnType("datetime");
            entity.Property(e => e.deadline_mark)
                .HasMaxLength(2048)
                .HasComment("交件備註");
            entity.Property(e => e.deliver_method).HasComment("出貨方式(基本包裝)");
            entity.Property(e => e.deliver_method2).HasComment("出貨方式(訂製禮盒)");
            entity.Property(e => e.deliver_method3).HasComment("出貨方式(訂製木箱)");
            entity.Property(e => e.dismantle_picture).HasComment("拆圖");
            entity.Property(e => e.dismantle_picture_estimated_time)
                .HasComment("拆圖出圖預計時間")
                .HasColumnType("datetime");
            entity.Property(e => e.dismantle_picture_real_time)
                .HasComment("拆圖出圖實際時間")
                .HasColumnType("datetime");
            entity.Property(e => e.edit_time)
                .HasComment("編輯時間")
                .HasColumnType("datetime");
            entity.Property(e => e.editor)
                .HasDefaultValueSql("''")
                .HasComment("編輯人員");
            entity.Property(e => e.file_path)
                .HasMaxLength(2048)
                .HasComment("存檔路徑");
            entity.Property(e => e.init_date)
                .HasComment("專案日期")
                .HasColumnType("datetime");
            entity.Property(e => e.opening_angle).HasComment("開合角度");
            entity.Property(e => e.opening_angle_amount)
                .HasMaxLength(50)
                .HasComment("角度(度)");
            entity.Property(e => e.other)
                .HasComment("其他事項")
                .HasColumnType("text");
            entity.Property(e => e.parent_pid).HasComment("父專案pid(若本身為父專案則值為null)");
            entity.Property(e => e.picture_supply_time)
                .HasComment("照片提供時間")
                .HasColumnType("datetime");
            entity.Property(e => e.pm)
                .HasDefaultValueSql("'0'")
                .HasComment("專案負責PM");
            entity.Property(e => e.pm_super)
                .HasDefaultValueSql("'0'")
                .HasComment("指派PM主管");
            entity.Property(e => e.product_other)
                .HasMaxLength(1024)
                .HasDefaultValueSql("''")
                .HasComment("產品類別-其他");
            entity.Property(e => e.product_type).HasComment("產品類別");
            entity.Property(e => e.production_mark)
                .HasMaxLength(512)
                .HasComment("製作數量備註");
            entity.Property(e => e.production_quantity)
                .HasMaxLength(30)
                .HasComment("製作數量");
            entity.Property(e => e.project_action).HasComment("作動");
            entity.Property(e => e.project_action2).HasComment("替換");
            entity.Property(e => e.project_action3).HasComment(" 亮燈");
            entity.Property(e => e.project_mark)
                .HasMaxLength(2048)
                .HasDefaultValueSql("''")
                .HasComment("備註");
            entity.Property(e => e.project_name)
                .HasMaxLength(30)
                .HasComment("專案名稱");
            entity.Property(e => e.project_no)
                .HasMaxLength(19)
                .HasComment("專案編號");
            entity.Property(e => e.project_type)
                .HasMaxLength(50)
                .HasComment("0：外觀 1：機構 2：修改 3：色彩計畫");
            entity.Property(e => e.prototypework_datetime)
                .HasComment("原型加工預計完成日期")
                .HasColumnType("datetime");
            entity.Property(e => e.prototypework_manager).HasComment("原型加工部負責人");
            entity.Property(e => e.purchase_memo)
                .HasMaxLength(2048)
                .HasComment("採購端備註");
            entity.Property(e => e.review_date)
                .HasComment("檢討日期")
                .HasColumnType("datetime");
            entity.Property(e => e.state)
                .HasMaxLength(25)
                .HasComment("專案狀態(STORAGE:暫存 START:新專案 PROCESS:已指派PM WORK:生產中 AFTER_WORK:後處理中 BUILD:組裝中 COMPLETE:已完成 DELIVERY:出貨 FINISH:結案 ABANDON:已廢棄)");
        });

        modelBuilder.Entity<project_no>(entity =>
        {
            entity.HasKey(e => e.code).HasName("PRIMARY");

            entity.ToTable("project_no");

            entity.Property(e => e.code).HasMaxLength(26);
        });

        modelBuilder.Entity<project_other_image>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PRIMARY");

            entity.ToTable("project_other_image");

            entity.HasIndex(e => e.project_pid, "FK__projects");

            entity.Property(e => e.create_time)
                .HasComment("建立時間")
                .HasColumnType("datetime");
            entity.Property(e => e.file_path)
                .HasMaxLength(1024)
                .HasComment("照片儲存路徑");
            entity.Property(e => e.ori_name)
                .HasMaxLength(256)
                .HasComment("原始照片名稱");
            entity.Property(e => e.project_pid).HasComment("專案pid");
            entity.Property(e => e.replace_string)
                .HasMaxLength(50)
                .HasComment("取代文字");

            entity.HasOne(d => d.project_p).WithMany(p => p.project_other_images)
                .HasForeignKey(d => d.project_pid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__projects");
        });

        modelBuilder.Entity<project_review_log>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PRIMARY");

            entity.ToTable("project_review_log", tb => tb.HasComment("專案以及委外單審核歷程"));

            entity.Property(e => e.pid).HasComment("流水號");
            entity.Property(e => e.create_time)
                .HasComment("建立時間(審核時間)")
                .HasColumnType("datetime");
            entity.Property(e => e.creator).HasComment("建立人員(審核人員)");
            entity.Property(e => e.project_outsource_pid).HasComment("委外單pid(如果是白身組裝檢查中以及組裝品檢檢查中，則此欄位為NULL)");
            entity.Property(e => e.project_pid).HasComment("專案pid");
            entity.Property(e => e.reason)
                .HasMaxLength(1024)
                .HasComment("退回原因");
            entity.Property(e => e.review_state)
                .HasMaxLength(50)
                .HasComment("審核狀態(通過或退回)");
            entity.Property(e => e.type).HasMaxLength(50);
        });

        modelBuilder.Entity<project_state_log>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PRIMARY");

            entity.ToTable("project_state_log");

            entity.Property(e => e.pid).HasComment("流水號");
            entity.Property(e => e.create_time)
                .HasComment("建立時間")
                .HasColumnType("datetime");
            entity.Property(e => e.creator).HasComment("建立人員");
            entity.Property(e => e.memo)
                .HasMaxLength(1024)
                .HasComment("備註");
            entity.Property(e => e.new_state)
                .HasMaxLength(25)
                .HasComment("變更後狀態");
            entity.Property(e => e.ori_state)
                .HasMaxLength(25)
                .HasComment("變更前狀態");
            entity.Property(e => e.project_pid).HasComment("專案pid");
        });

        modelBuilder.Entity<projects_assign_log>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PRIMARY");

            entity.ToTable("projects_assign_log");

            entity.Property(e => e.pid).HasComment("流水號");
            entity.Property(e => e.craftrd_pid).HasComment("工藝研發部負責人pid");
            entity.Property(e => e.create_time)
                .HasComment("指派時間")
                .HasColumnType("datetime");
            entity.Property(e => e.pm_pid).HasComment("專案負責人pid");
            entity.Property(e => e.project_pid).HasComment("專案pid");
            entity.Property(e => e.project_type)
                .HasMaxLength(20)
                .HasDefaultValueSql("''")
                .HasComment("專案狀態");
            entity.Property(e => e.prototypework_pid).HasComment("原型加工部負責人pid");
            entity.Property(e => e.user_pid).HasComment("指派人員pid");
        });

        modelBuilder.Entity<projects_file>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PRIMARY");

            entity.ToTable(tb => tb.HasComment("專案追蹤表相關文件"));

            entity.HasIndex(e => e.projects_pid, "FK_projects_files_projects");

            entity.Property(e => e.pid).HasComment("流水號");
            entity.Property(e => e.a_path)
                .HasMaxLength(512)
                .HasComment("儲存位置");
            entity.Property(e => e.a_type)
                .HasMaxLength(25)
                .HasComment("可用項目包含projects的state欄位(就是專案狀態)");
            entity.Property(e => e.create_time)
                .HasComment("上傳時間")
                .HasColumnType("datetime");
            entity.Property(e => e.filename)
                .HasMaxLength(50)
                .HasDefaultValueSql("''")
                .HasComment("檔案名稱");
            entity.Property(e => e.projects_pid).HasComment("Projects的pid");
            entity.Property(e => e.upload_action)
                .HasMaxLength(25)
                .HasDefaultValueSql("''")
                .HasComment("從哪個功能上傳的");
            entity.Property(e => e.upload_user).HasComment("上傳人員pid");

            entity.HasOne(d => d.projects_p).WithMany(p => p.projects_files)
                .HasForeignKey(d => d.projects_pid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_projects_files_projects");
        });

        modelBuilder.Entity<projects_outsource>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PRIMARY");

            entity.ToTable("projects_outsource", tb => tb.HasComment("委外加工單"));

            entity.Property(e => e.back_time_remark)
                .HasMaxLength(50)
                .HasComment("回件備註");
            entity.Property(e => e.bulid_time_remark)
                .HasMaxLength(50)
                .HasComment("發包備註");
            entity.Property(e => e.create_time)
                .HasComment("建立時間")
                .HasColumnType("datetime");
            entity.Property(e => e.creator).HasComment("建立人員");
            entity.Property(e => e.finish_time)
                .HasComment("完成時間")
                .HasColumnType("datetime");
            entity.Property(e => e.firm).HasComment("廠商pid(來源：廠商維護功能)");
            entity.Property(e => e.form_remark)
                .HasMaxLength(256)
                .HasComment("表單備註");
            entity.Property(e => e.outsource_back_time)
                .HasComment("委外回件時間")
                .HasColumnType("datetime");
            entity.Property(e => e.outsource_build_time)
                .HasComment("發包時間")
                .HasColumnType("datetime");
            entity.Property(e => e.outsource_no)
                .HasMaxLength(26)
                .HasComment("委外加工單編號");
            entity.Property(e => e.picture_number)
                .HasMaxLength(50)
                .HasComment("圖號");
            entity.Property(e => e.process).HasComment("製程pid(來源：製程維護功能)");
            entity.Property(e => e.send_time)
                .HasComment("預計送件時間")
                .HasColumnType("datetime");
            entity.Property(e => e.state)
                .HasMaxLength(50)
                .HasComment("審核狀態");
            entity.Property(e => e.total_component).HasComment("零件總數");
            entity.Property(e => e.type_name)
                .HasMaxLength(50)
                .HasComment("類別");
        });

        modelBuilder.Entity<projects_outsource_log>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PRIMARY");

            entity.ToTable("projects_outsource_log", tb => tb.HasComment("紀錄委外加工單修改的歷程"));

            entity.Property(e => e.edit_time)
                .HasComment("異動時間")
                .HasColumnType("datetime");
            entity.Property(e => e.editor)
                .HasDefaultValueSql("''")
                .HasComment("異動人員");
            entity.Property(e => e.ori_back_time)
                .HasComment("異動前的回件時間")
                .HasColumnType("datetime");
            entity.Property(e => e.ori_build_time)
                .HasComment("異動前的發包時間")
                .HasColumnType("datetime");
            entity.Property(e => e.projects_outsource_pid).HasComment("委外加工單PID");
        });

        modelBuilder.Entity<projects_outsource_relation>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PRIMARY");

            entity.ToTable("projects_outsource_relation", tb => tb.HasComment("白身組裝完成時間與委外加工單關聯表"));

            entity.HasIndex(e => e.projects_outsource_pid, "FK_projects_outsource_relation_projects_outsource");

            entity.Property(e => e.pid).HasComment("流水號");
            entity.Property(e => e.build_time)
                .HasComment("白身組裝完成時間")
                .HasColumnType("datetime");
            entity.Property(e => e.outsource_back_time)
                .HasComment("委外加工單完成時間")
                .HasColumnType("datetime");
            entity.Property(e => e.projects_outsource_pid).HasComment("委外加工單pid");
            entity.Property(e => e.projects_pid).HasComment("專案追蹤表pid");
            entity.Property(e => e.remark)
                .HasMaxLength(50)
                .HasComment("備註");

            entity.HasOne(d => d.projects_outsource_p).WithMany(p => p.projects_outsource_relations)
                .HasForeignKey(d => d.projects_outsource_pid)
                .HasConstraintName("FK_projects_outsource_relation_projects_outsource");
        });

        modelBuilder.Entity<role>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.create_time).HasColumnType("datetime");
            entity.Property(e => e.creator).HasMaxLength(30);
            entity.Property(e => e.edit_time).HasColumnType("datetime");
            entity.Property(e => e.editor).HasMaxLength(30);
            entity.Property(e => e.name).HasMaxLength(50);
            entity.Property(e => e.programe_code).HasMaxLength(30);
        });

        modelBuilder.Entity<role_del>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PRIMARY");

            entity.ToTable("role_del");

            entity.Property(e => e.create_time).HasColumnType("datetime");
            entity.Property(e => e.creator).HasMaxLength(30);
            entity.Property(e => e.edit_time).HasColumnType("datetime");
            entity.Property(e => e.editor).HasMaxLength(30);
            entity.Property(e => e.name).HasMaxLength(50);
            entity.Property(e => e.programe_code).HasMaxLength(30);
        });

        modelBuilder.Entity<role_func>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PRIMARY");

            entity.ToTable("role_func");
        });

        modelBuilder.Entity<system_code>(entity =>
        {
            entity.HasKey(e => e.pid).HasName("PRIMARY");

            entity.ToTable("system_code");

            entity.Property(e => e.code).HasMaxLength(20);
            entity.Property(e => e.data).HasMaxLength(128);
            entity.Property(e => e.description).HasMaxLength(1024);
            entity.Property(e => e.sub_description).HasMaxLength(128);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
