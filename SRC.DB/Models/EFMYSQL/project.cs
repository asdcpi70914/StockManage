using System;
using System.Collections.Generic;

namespace SRC.DB.Models.EFMYSQL;

/// <summary>
/// 專案追蹤表
/// </summary>
public partial class project
{
    public long pid { get; set; }

    /// <summary>
    /// 父專案pid(若本身為父專案則值為null)
    /// </summary>
    public long? parent_pid { get; set; }

    /// <summary>
    /// 專案編號
    /// </summary>
    public string project_no { get; set; } = null!;

    /// <summary>
    /// 專案名稱
    /// </summary>
    public string project_name { get; set; } = null!;

    /// <summary>
    /// 專案日期
    /// </summary>
    public DateTime? init_date { get; set; }

    /// <summary>
    /// 交件日期
    /// </summary>
    public DateTime? deadline { get; set; }

    /// <summary>
    /// 指派PM主管
    /// </summary>
    public long? pm_super { get; set; }

    /// <summary>
    /// 專案負責PM
    /// </summary>
    public long? pm { get; set; }

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime create_time { get; set; }

    /// <summary>
    /// 建立人員
    /// </summary>
    public Guid creator { get; set; }

    /// <summary>
    /// 編輯人員
    /// </summary>
    public Guid editor { get; set; }

    /// <summary>
    /// 編輯時間
    /// </summary>
    public DateTime edit_time { get; set; }

    /// <summary>
    /// 專案狀態(STORAGE:暫存 START:新專案 PROCESS:已指派PM WORK:生產中 AFTER_WORK:後處理中 BUILD:組裝中 COMPLETE:已完成 DELIVERY:出貨 FINISH:結案 ABANDON:已廢棄)
    /// </summary>
    public string state { get; set; } = null!;

    /// <summary>
    /// 組裝完成時間
    /// </summary>
    public DateTime? build_time { get; set; }

    /// <summary>
    /// 組裝完成時間的儲存時間
    /// </summary>
    public DateTime? build_create_time { get; set; }

    /// <summary>
    /// 組裝完成時間建立者
    /// </summary>
    public long? builder { get; set; }

    /// <summary>
    /// 組裝前品檢時間
    /// </summary>
    public DateTime? bulid_inspection_before_time { get; set; }

    /// <summary>
    /// 組裝後品檢時間
    /// </summary>
    public DateTime? bulid_inspection_after_time { get; set; }

    /// <summary>
    /// 照片提供時間
    /// </summary>
    public DateTime? picture_supply_time { get; set; }

    /// <summary>
    /// 客戶編號
    /// </summary>
    public string customer_no { get; set; } = null!;

    /// <summary>
    /// 製作數量
    /// </summary>
    public string production_quantity { get; set; } = null!;

    /// <summary>
    /// 製作數量備註
    /// </summary>
    public string? production_mark { get; set; }

    /// <summary>
    /// 客戶窗口
    /// </summary>
    public string customer_window { get; set; } = null!;

    /// <summary>
    /// 連絡電話
    /// </summary>
    public string contact_phone { get; set; } = null!;

    /// <summary>
    /// 檢討日期
    /// </summary>
    public DateTime? review_date { get; set; }

    /// <summary>
    /// 作動
    /// </summary>
    public bool project_action { get; set; }

    /// <summary>
    /// 替換
    /// </summary>
    public bool project_action2 { get; set; }

    /// <summary>
    ///  亮燈
    /// </summary>
    public bool project_action3 { get; set; }

    /// <summary>
    /// 外觀處理
    /// </summary>
    public bool appearance_handle { get; set; }

    /// <summary>
    /// 拆圖
    /// </summary>
    public bool dismantle_picture { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    public string? project_mark { get; set; }

    /// <summary>
    /// CMF
    /// </summary>
    public bool cmf { get; set; }

    public DateTime? cmf_datetime { get; set; }

    /// <summary>
    /// Artwork
    /// </summary>
    public bool artwork { get; set; }

    public DateTime? artwork_datetime { get; set; }

    /// <summary>
    /// 配重
    /// </summary>
    public bool counter_weight { get; set; }

    /// <summary>
    /// 配種重量(g)
    /// </summary>
    public string? counter_weight_amount { get; set; }

    /// <summary>
    /// 開合角度
    /// </summary>
    public bool opening_angle { get; set; }

    /// <summary>
    /// 角度(度)
    /// </summary>
    public string? opening_angle_amount { get; set; }

    /// <summary>
    /// 出貨方式(基本包裝)
    /// </summary>
    public bool deliver_method { get; set; }

    /// <summary>
    /// 出貨方式(訂製禮盒)
    /// </summary>
    public bool deliver_method2 { get; set; }

    /// <summary>
    /// 出貨方式(訂製木箱)
    /// </summary>
    public bool deliver_method3 { get; set; }

    /// <summary>
    /// 客供件(色板(樣))
    /// </summary>
    public bool consign_materia { get; set; }

    /// <summary>
    /// 客供件(零件)
    /// </summary>
    public bool consign_materia2 { get; set; }

    /// <summary>
    /// 客供件(其他)
    /// </summary>
    public bool consign_materia3 { get; set; }

    /// <summary>
    /// 其他事項
    /// </summary>
    public string? other { get; set; }

    /// <summary>
    /// 0：外觀 1：機構 2：修改 3：色彩計畫
    /// </summary>
    public string? project_type { get; set; }

    /// <summary>
    /// 業務負責人
    /// </summary>
    public long? business_manager { get; set; }

    /// <summary>
    /// 原型加工部負責人
    /// </summary>
    public long? prototypework_manager { get; set; }

    /// <summary>
    /// 原型加工預計完成日期
    /// </summary>
    public DateTime? prototypework_datetime { get; set; }

    /// <summary>
    /// 工藝研發部負責人
    /// </summary>
    public long? craftrd_manager { get; set; }

    /// <summary>
    /// 工藝研發預計完成日期
    /// </summary>
    public DateTime? craftrd_datetime { get; set; }

    /// <summary>
    /// 0：國內 1：國外
    /// </summary>
    public string customer_type { get; set; } = null!;

    /// <summary>
    /// 產品類別
    /// </summary>
    public long product_type { get; set; }

    /// <summary>
    /// 產品類別-其他
    /// </summary>
    public string? product_other { get; set; }

    /// <summary>
    /// 完工時間
    /// </summary>
    public DateTime? complete_time { get; set; }

    /// <summary>
    /// 拆圖出圖預計時間
    /// </summary>
    public DateTime? dismantle_picture_estimated_time { get; set; }

    /// <summary>
    /// 拆圖出圖實際時間
    /// </summary>
    public DateTime? dismantle_picture_real_time { get; set; }

    /// <summary>
    /// 估價單號
    /// </summary>
    public string? appraise_number { get; set; }

    /// <summary>
    /// 客戶需求時間_自身組裝完成時間
    /// </summary>
    public DateTime? cust_build_time { get; set; }

    /// <summary>
    /// 客戶需求時間_組裝前品檢時間
    /// </summary>
    public DateTime? cust_build_inspection_before_time { get; set; }

    /// <summary>
    /// 客戶需求時間_組裝後品檢時間
    /// </summary>
    public DateTime? cust_build_inspection_after_time { get; set; }

    /// <summary>
    /// 客戶需求時間_照片提供時間
    /// </summary>
    public DateTime? cust_picture_supply_time { get; set; }

    /// <summary>
    /// 客戶需求時間_原型加工預計完成時間
    /// </summary>
    public DateTime? cust_prototypework_datetime { get; set; }

    /// <summary>
    /// 客戶需求時間_工藝研發預計完成時間
    /// </summary>
    public DateTime? cust_craftrd_datetime { get; set; }

    /// <summary>
    /// 客戶需求時間_拆圖出圖預計時間
    /// </summary>
    public DateTime? cust_dismantle_picture_estimated_time { get; set; }

    /// <summary>
    /// 客戶需求時間_拆圖出圖實際時間
    /// </summary>
    public DateTime? cust_dismantle_picture_real_time { get; set; }

    /// <summary>
    /// 存檔路徑
    /// </summary>
    public string? file_path { get; set; }

    /// <summary>
    /// 採購端備註
    /// </summary>
    public string? purchase_memo { get; set; }

    /// <summary>
    /// 交件備註
    /// </summary>
    public string? deadline_mark { get; set; }

    public virtual ICollection<project_other_image> project_other_images { get; } = new List<project_other_image>();

    public virtual ICollection<projects_file> projects_files { get; } = new List<projects_file>();
}
