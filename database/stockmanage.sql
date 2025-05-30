USE [master]
GO
/****** Object:  Database [StockManage]    Script Date: 2025/5/14 下午 05:26:22 ******/
CREATE DATABASE [StockManage]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'StockManage', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\StockManage.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'StockManage_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\StockManage_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [StockManage] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [StockManage].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [StockManage] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [StockManage] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [StockManage] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [StockManage] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [StockManage] SET ARITHABORT OFF 
GO
ALTER DATABASE [StockManage] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [StockManage] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [StockManage] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [StockManage] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [StockManage] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [StockManage] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [StockManage] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [StockManage] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [StockManage] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [StockManage] SET  DISABLE_BROKER 
GO
ALTER DATABASE [StockManage] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [StockManage] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [StockManage] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [StockManage] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [StockManage] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [StockManage] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [StockManage] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [StockManage] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [StockManage] SET  MULTI_USER 
GO
ALTER DATABASE [StockManage] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [StockManage] SET DB_CHAINING OFF 
GO
ALTER DATABASE [StockManage] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [StockManage] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [StockManage] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [StockManage] SET QUERY_STORE = OFF
GO
USE [StockManage]
GO
/****** Object:  Table [dbo].[backend_dept]    Script Date: 2025/5/14 下午 05:26:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[backend_dept](
	[pid] [bigint] IDENTITY(1,1) NOT NULL,
	[backend_user_pid] [bigint] NOT NULL,
	[parent_pid] [bigint] NULL,
	[code] [varchar](50) NOT NULL,
	[create_time] [datetime] NOT NULL,
 CONSTRAINT [PK_backend_dept] PRIMARY KEY CLUSTERED 
(
	[pid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[backend_units]    Script Date: 2025/5/14 下午 05:26:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[backend_units](
	[pid] [bigint] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[code] [varchar](10) NOT NULL,
 CONSTRAINT [PK_backend_units] PRIMARY KEY CLUSTERED 
(
	[pid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[backend_users]    Script Date: 2025/5/14 下午 05:26:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[backend_users](
	[pid] [bigint] IDENTITY(1,1) NOT NULL,
	[user_id] [uniqueidentifier] NOT NULL,
	[account] [varchar](30) NOT NULL,
	[ad_account] [varchar](30) NULL,
	[name_ch] [nvarchar](80) NOT NULL,
	[name_en] [varchar](50) NULL,
	[email] [nvarchar](200) NULL,
	[password_hash] [nvarchar](256) NOT NULL,
	[phone_number] [nvarchar](30) NULL,
	[lockout_end] [datetime] NULL,
	[enabled] [bit] NOT NULL,
	[access_failed_count] [int] NULL,
	[changed_password_time] [datetime] NULL,
	[create_time] [datetime] NOT NULL,
	[creator] [varchar](30) NOT NULL,
	[edit_time] [datetime] NULL,
	[editor] [varchar](30) NULL,
	[unit] [varchar](5) NULL,
	[first_login] [bit] NOT NULL,
	[apply_date] [datetime] NULL,
	[verification_code] [nchar](10) NULL,
	[first_login_time] [datetime] NULL,
	[jwt_code] [varchar](512) NULL,
	[device_code] [varchar](256) NULL,
	[email_confirmed] [bit] NULL,
	[email_confirmed_time] [datetime] NULL,
	[limit_time] [datetime] NULL,
	[state] [tinyint] NOT NULL,
	[person_in_charge] [bit] NOT NULL,
 CONSTRAINT [PK_backend_users] PRIMARY KEY CLUSTERED 
(
	[pid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[backend_users_del]    Script Date: 2025/5/14 下午 05:26:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[backend_users_del](
	[pid] [bigint] IDENTITY(1,1) NOT NULL,
	[backend_user_pid] [bigint] NOT NULL,
	[user_id] [uniqueidentifier] NOT NULL,
	[account] [varchar](30) NOT NULL,
	[ad_account] [varchar](30) NULL,
	[name_ch] [nvarchar](80) NOT NULL,
	[name_en] [varchar](50) NULL,
	[email] [nvarchar](200) NULL,
	[password_hash] [nvarchar](256) NOT NULL,
	[phone_number] [nvarchar](30) NULL,
	[lockout_end] [datetime] NULL,
	[enabled] [bit] NOT NULL,
	[access_failed_count] [int] NULL,
	[changed_password_time] [datetime] NULL,
	[create_time] [datetime] NOT NULL,
	[creator] [varchar](30) NOT NULL,
	[edit_time] [datetime] NULL,
	[editor] [varchar](30) NOT NULL,
	[unit] [varchar](5) NULL,
	[first_login] [bit] NOT NULL,
	[apply_date] [datetime] NULL,
	[verification_code] [nchar](10) NULL,
	[first_login_time] [datetime] NULL,
	[jwt_code] [varchar](512) NULL,
	[device_code] [varchar](256) NULL,
	[email_confirmed] [bit] NULL,
	[email_confirmed_time] [datetime] NULL,
	[limit_time] [datetime] NULL,
	[state] [tinyint] NOT NULL,
	[person_in_charge] [bit] NOT NULL,
 CONSTRAINT [PK_backend_users_del] PRIMARY KEY CLUSTERED 
(
	[pid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[backend_users_role]    Script Date: 2025/5/14 下午 05:26:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[backend_users_role](
	[pid] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [uniqueidentifier] NOT NULL,
	[role_id] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Member_Role] PRIMARY KEY CLUSTERED 
(
	[pid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[func]    Script Date: 2025/5/14 下午 05:26:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[func](
	[pid] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NULL,
	[url] [nvarchar](400) NULL,
	[parentid] [int] NULL,
	[editor] [varchar](30) NULL,
	[edit_time] [datetime] NULL,
	[type] [varchar](15) NULL,
	[create_time] [datetime] NOT NULL,
	[creator] [varchar](30) NULL,
	[icon] [varchar](50) NULL,
	[weight] [smallint] NOT NULL,
	[state] [smallint] NOT NULL,
	[memo] [nvarchar](500) NULL,
 CONSTRAINT [PK_dbo.Func] PRIMARY KEY CLUSTERED 
(
	[pid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[role]    Script Date: 2025/5/14 下午 05:26:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[role](
	[pid] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NULL,
	[editor] [varchar](50) NULL,
	[edit_time] [datetime] NULL,
	[create_time] [datetime] NOT NULL,
	[creator] [varchar](30) NOT NULL,
	[state] [int] NOT NULL,
	[programe_code] [varchar](30) NULL,
 CONSTRAINT [PK_dbo.Role] PRIMARY KEY CLUSTERED 
(
	[pid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[role_del]    Script Date: 2025/5/14 下午 05:26:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[role_del](
	[pid] [int] IDENTITY(1,1) NOT NULL,
	[role_pid] [int] NOT NULL,
	[name] [nvarchar](50) NULL,
	[editor] [varchar](50) NULL,
	[edit_time] [datetime] NULL,
	[create_time] [datetime] NOT NULL,
	[creator] [varchar](30) NOT NULL,
	[state] [int] NOT NULL,
	[programe_code] [varchar](30) NULL,
 CONSTRAINT [PK_role_del] PRIMARY KEY CLUSTERED 
(
	[pid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[role_func]    Script Date: 2025/5/14 下午 05:26:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[role_func](
	[pid] [int] IDENTITY(1,1) NOT NULL,
	[func_id] [int] NOT NULL,
	[role_id] [int] NOT NULL,
	[weight] [smallint] NOT NULL,
 CONSTRAINT [PK_dbo.Role_Func] PRIMARY KEY CLUSTERED 
(
	[pid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[system_code]    Script Date: 2025/5/14 下午 05:26:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[system_code](
	[pid] [bigint] IDENTITY(1,1) NOT NULL,
	[code] [varchar](20) NOT NULL,
	[data] [nvarchar](128) NOT NULL,
	[description] [nvarchar](128) NOT NULL,
	[sub_description] [nvarchar](128) NOT NULL,
	[weight] [int] NULL,
 CONSTRAINT [PK_system_code] PRIMARY KEY CLUSTERED 
(
	[pid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[backend_users] ON 

INSERT [dbo].[backend_users] ([pid], [user_id], [account], [ad_account], [name_ch], [name_en], [email], [password_hash], [phone_number], [lockout_end], [enabled], [access_failed_count], [changed_password_time], [create_time], [creator], [edit_time], [editor], [unit], [first_login], [apply_date], [verification_code], [first_login_time], [jwt_code], [device_code], [email_confirmed], [email_confirmed_time], [limit_time], [state], [person_in_charge]) VALUES (1, N'bdef6119-6203-4eee-b3af-6e3f20df1542', N'admin', NULL, N'系統主帳號', N'system', NULL, N'0ACFD0885C7F1404167F6CEE45BE9E47300018FE84BAFC8E2A99DB2B707FA44AC611F0E1114108733617507EE0222C3BB2B21F6320A168841559B5AA81DE2985', NULL, NULL, 1, 1, NULL, CAST(N'2023-03-02T19:03:04.797' AS DateTime), N'admin', NULL, N'admin', NULL, 0, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 1, 0)
SET IDENTITY_INSERT [dbo].[backend_users] OFF
GO
SET IDENTITY_INSERT [dbo].[backend_users_del] ON 

INSERT [dbo].[backend_users_del] ([pid], [backend_user_pid], [user_id], [account], [ad_account], [name_ch], [name_en], [email], [password_hash], [phone_number], [lockout_end], [enabled], [access_failed_count], [changed_password_time], [create_time], [creator], [edit_time], [editor], [unit], [first_login], [apply_date], [verification_code], [first_login_time], [jwt_code], [device_code], [email_confirmed], [email_confirmed_time], [limit_time], [state], [person_in_charge]) VALUES (1, 3, N'a1d66e2b-beb4-414f-b6f1-bd2dbbfed1da', N'admin', NULL, N'alan1', NULL, N'asdcpi70914@gmail.com', N'5BC95C7DC4AD31F3BE00E94DA2B9B4A4B61F591B8BE09EC26E96471661ABEAC8AB7A8D6048B3941E55F3B6E9D473DB45436E5B861441F0FCAABAAB8E8732FC21', N'0953216487', NULL, 1, 0, NULL, CAST(N'2025-05-14T16:51:50.240' AS DateTime), N'admin', CAST(N'2025-05-14T17:01:07.247' AS DateTime), N'admin', NULL, 0, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 0, 0)
INSERT [dbo].[backend_users_del] ([pid], [backend_user_pid], [user_id], [account], [ad_account], [name_ch], [name_en], [email], [password_hash], [phone_number], [lockout_end], [enabled], [access_failed_count], [changed_password_time], [create_time], [creator], [edit_time], [editor], [unit], [first_login], [apply_date], [verification_code], [first_login_time], [jwt_code], [device_code], [email_confirmed], [email_confirmed_time], [limit_time], [state], [person_in_charge]) VALUES (2, 4, N'59447179-6d10-4ec7-ae24-6416b95f5f06', N'admin', NULL, N'1', NULL, N'a@b.c', N'0ACFD0885C7F1404167F6CEE45BE9E47300018FE84BAFC8E2A99DB2B707FA44AC611F0E1114108733617507EE0222C3BB2B21F6320A168841559B5AA81DE2985', N'0953218171', NULL, 1, 0, NULL, CAST(N'2025-05-14T17:10:17.417' AS DateTime), N'admin', NULL, N'admin', NULL, 0, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, 0, 0)
SET IDENTITY_INSERT [dbo].[backend_users_del] OFF
GO
SET IDENTITY_INSERT [dbo].[backend_users_role] ON 

INSERT [dbo].[backend_users_role] ([pid], [user_id], [role_id]) VALUES (8, N'bdef6119-6203-4eee-b3af-6e3f20df1542', 4)
SET IDENTITY_INSERT [dbo].[backend_users_role] OFF
GO
SET IDENTITY_INSERT [dbo].[func] ON 

INSERT [dbo].[func] ([pid], [name], [url], [parentid], [editor], [edit_time], [type], [create_time], [creator], [icon], [weight], [state], [memo]) VALUES (99, N'首頁', N'/home/Index', NULL, NULL, NULL, N'menu', CAST(N'2020-01-02T15:33:20.000' AS DateTime), N'SYSTEM_ADMIN', N'metismenu-icon fa-duotone fa-house-user', 1, 1, N'系統初始建置')
INSERT [dbo].[func] ([pid], [name], [url], [parentid], [editor], [edit_time], [type], [create_time], [creator], [icon], [weight], [state], [memo]) VALUES (100, N'網站系統管理', N'#', NULL, NULL, NULL, N'menu', CAST(N'2020-01-02T15:33:20.000' AS DateTime), N'SYSTEM_ADMIN', N'fa-thin fa-person-circle-question', 2, 1, N'系統初始建置')
INSERT [dbo].[func] ([pid], [name], [url], [parentid], [editor], [edit_time], [type], [create_time], [creator], [icon], [weight], [state], [memo]) VALUES (101, N'後台帳號管理', N'/backenduser/Index', 100, NULL, NULL, N'menu', CAST(N'2020-01-02T15:33:20.000' AS DateTime), N'SYSTEM_ADMIN', N'fa-thin fa-people-roof', 1, 1, N'系統初始建置')
INSERT [dbo].[func] ([pid], [name], [url], [parentid], [editor], [edit_time], [type], [create_time], [creator], [icon], [weight], [state], [memo]) VALUES (102, N'後台角色權限維護', N'/role/Index', 100, NULL, NULL, N'menu', CAST(N'2020-01-02T15:33:20.000' AS DateTime), N'SYSTEM_ADMIN', N'fa-thin fa-person-carry-box', 2, 1, N'系統初始建置')
INSERT [dbo].[func] ([pid], [name], [url], [parentid], [editor], [edit_time], [type], [create_time], [creator], [icon], [weight], [state], [memo]) VALUES (103, N'後台帳號管理-查詢', N'/backenduser/search', 101, NULL, NULL, N'btn', CAST(N'2020-01-02T15:33:20.000' AS DateTime), N'SYSTEM_ADMIN', N'asterisk', 1, 1, N'系統初始建置')
INSERT [dbo].[func] ([pid], [name], [url], [parentid], [editor], [edit_time], [type], [create_time], [creator], [icon], [weight], [state], [memo]) VALUES (104, N'後台帳號管理-新增', N'/backenduser/create', 101, NULL, NULL, N'btn', CAST(N'2020-01-02T15:33:20.000' AS DateTime), N'SYSTEM_ADMIN', N'asterisk', 1, 1, N'系統初始建置')
INSERT [dbo].[func] ([pid], [name], [url], [parentid], [editor], [edit_time], [type], [create_time], [creator], [icon], [weight], [state], [memo]) VALUES (105, N'後台帳號管理-停用', N'/backenduser/DISABLE', 101, NULL, NULL, N'btn', CAST(N'2020-01-02T15:33:20.000' AS DateTime), N'SYSTEM_ADMIN', N'asterisk', 1, 1, N'系統初始建置')
INSERT [dbo].[func] ([pid], [name], [url], [parentid], [editor], [edit_time], [type], [create_time], [creator], [icon], [weight], [state], [memo]) VALUES (106, N'後台帳號管理-修改', N'/backenduser/edit', 101, NULL, NULL, N'btn', CAST(N'2020-01-02T15:33:20.000' AS DateTime), N'SYSTEM_ADMIN', N'asterisk', 1, 1, N'系統初始建置')
INSERT [dbo].[func] ([pid], [name], [url], [parentid], [editor], [edit_time], [type], [create_time], [creator], [icon], [weight], [state], [memo]) VALUES (107, N'後台角色維護-查詢', N'/role/search', 102, NULL, NULL, N'btn', CAST(N'2020-01-02T15:33:20.000' AS DateTime), N'SYSTEM_ADMIN', N'asterisk', 1, 1, N'系統初始建置')
INSERT [dbo].[func] ([pid], [name], [url], [parentid], [editor], [edit_time], [type], [create_time], [creator], [icon], [weight], [state], [memo]) VALUES (108, N'後台角色維護-新增', N'/role/add', 102, NULL, NULL, N'btn', CAST(N'2020-01-02T15:33:20.000' AS DateTime), N'SYSTEM_ADMIN', N'asterisk', 1, 1, N'系統初始建置')
INSERT [dbo].[func] ([pid], [name], [url], [parentid], [editor], [edit_time], [type], [create_time], [creator], [icon], [weight], [state], [memo]) VALUES (109, N'後台角色維護-刪除', N'/role/delete', 102, NULL, NULL, N'btn', CAST(N'2020-01-02T15:33:20.000' AS DateTime), N'SYSTEM_ADMIN', N'asterisk', 1, 1, N'系統初始建置')
INSERT [dbo].[func] ([pid], [name], [url], [parentid], [editor], [edit_time], [type], [create_time], [creator], [icon], [weight], [state], [memo]) VALUES (110, N'後台角色維護-修改', N'/role/edit', 102, NULL, NULL, N'btn', CAST(N'2020-01-02T15:33:20.000' AS DateTime), N'SYSTEM_ADMIN', N'asterisk', 1, 1, N'系統初始建置')
INSERT [dbo].[func] ([pid], [name], [url], [parentid], [editor], [edit_time], [type], [create_time], [creator], [icon], [weight], [state], [memo]) VALUES (111, N'後台角色維護-帳號/權限', N'/role/edit_func', 102, NULL, NULL, N'btn', CAST(N'2020-01-02T15:33:20.000' AS DateTime), N'SYSTEM_ADMIN', N'asterisk', 1, 1, N'系統初始建置')
INSERT [dbo].[func] ([pid], [name], [url], [parentid], [editor], [edit_time], [type], [create_time], [creator], [icon], [weight], [state], [memo]) VALUES (112, N'修改個人密碼', N'/backenduser/changepassword', 99, NULL, NULL, N'btn', CAST(N'2020-01-02T15:33:20.000' AS DateTime), N'SYSTEM_ADMIN', N'asterisk', 1, 1, N'系統初始建置')
INSERT [dbo].[func] ([pid], [name], [url], [parentid], [editor], [edit_time], [type], [create_time], [creator], [icon], [weight], [state], [memo]) VALUES (113, N'使用者帳號管理-刪除', N'/backenduser/Delete', 101, NULL, NULL, N'btn', CAST(N'2020-01-02T15:33:20.000' AS DateTime), N'SYSTEM_ADMIN', N'asterisk', 1, 1, N'系統初始建置')
INSERT [dbo].[func] ([pid], [name], [url], [parentid], [editor], [edit_time], [type], [create_time], [creator], [icon], [weight], [state], [memo]) VALUES (114, N'使用者帳號管理-密碼重設', N'/backenduser/ResetPassword', 101, NULL, NULL, N'btn', CAST(N'2020-01-02T15:33:20.000' AS DateTime), N'SYSTEM_ADMIN', N'asterisk', 1, 1, N'系統初始建置')
INSERT [dbo].[func] ([pid], [name], [url], [parentid], [editor], [edit_time], [type], [create_time], [creator], [icon], [weight], [state], [memo]) VALUES (115, N'後台帳號角色維護', N'/backendUserRole/Index', 100, NULL, NULL, N'menu', CAST(N'2020-01-02T15:33:20.000' AS DateTime), N'SYSTEM_ADMIN', N'fa-thin fa-hat-cowboy', 3, 1, N'系統初始建置')
INSERT [dbo].[func] ([pid], [name], [url], [parentid], [editor], [edit_time], [type], [create_time], [creator], [icon], [weight], [state], [memo]) VALUES (116, N'後台帳號角色維護-編輯', N'/backendUserRole/Edit', 115, NULL, NULL, N'btn', CAST(N'2020-01-02T15:33:20.000' AS DateTime), N'SYSTEM_ADMIN', N'asterisk', 22, 1, N'系統初始建置')
INSERT [dbo].[func] ([pid], [name], [url], [parentid], [editor], [edit_time], [type], [create_time], [creator], [icon], [weight], [state], [memo]) VALUES (117, N'後台帳號角色維護-查詢', N'/backendUserRole/Search', 115, NULL, NULL, N'btn', CAST(N'2020-01-02T15:33:20.000' AS DateTime), N'SYSTEM_ADMIN', N'asterisk', 22, 1, N'系統初始建置')
SET IDENTITY_INSERT [dbo].[func] OFF
GO
SET IDENTITY_INSERT [dbo].[role] ON 

INSERT [dbo].[role] ([pid], [name], [editor], [edit_time], [create_time], [creator], [state], [programe_code]) VALUES (4, N'系統管理員', NULL, NULL, CAST(N'2023-03-02T19:03:04.797' AS DateTime), N'admin', 1, NULL)
SET IDENTITY_INSERT [dbo].[role] OFF
GO
SET IDENTITY_INSERT [dbo].[role_del] ON 

INSERT [dbo].[role_del] ([pid], [role_pid], [name], [editor], [edit_time], [create_time], [creator], [state], [programe_code]) VALUES (1, 5, N'測試1', N'admin', CAST(N'2025-05-14T16:47:04.720' AS DateTime), CAST(N'2025-05-14T16:46:39.020' AS DateTime), N'admin', 1, NULL)
INSERT [dbo].[role_del] ([pid], [role_pid], [name], [editor], [edit_time], [create_time], [creator], [state], [programe_code]) VALUES (2, 6, N'222', N'admin', CAST(N'2025-05-14T16:47:15.510' AS DateTime), CAST(N'2025-05-14T16:47:08.103' AS DateTime), N'admin', 1, NULL)
SET IDENTITY_INSERT [dbo].[role_del] OFF
GO
SET IDENTITY_INSERT [dbo].[role_func] ON 

INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (103, 99, 5, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (104, 112, 5, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (105, 100, 5, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (106, 101, 5, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (107, 103, 5, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (108, 104, 5, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (109, 105, 5, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (110, 106, 5, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (111, 113, 5, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (112, 114, 5, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (113, 102, 5, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (114, 107, 5, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (115, 108, 5, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (116, 109, 5, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (117, 110, 5, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (118, 111, 5, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (119, 115, 5, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (120, 116, 5, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (121, 117, 5, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (122, 99, 6, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (123, 112, 6, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (162, 99, 4, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (163, 112, 4, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (164, 100, 4, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (165, 101, 4, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (166, 103, 4, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (167, 104, 4, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (168, 105, 4, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (169, 106, 4, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (170, 113, 4, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (171, 114, 4, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (172, 102, 4, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (173, 107, 4, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (174, 108, 4, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (175, 109, 4, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (176, 110, 4, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (177, 111, 4, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (178, 115, 4, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (179, 116, 4, 10)
INSERT [dbo].[role_func] ([pid], [func_id], [role_id], [weight]) VALUES (180, 117, 4, 10)
SET IDENTITY_INSERT [dbo].[role_func] OFF
GO
SET IDENTITY_INSERT [dbo].[system_code] ON 

INSERT [dbo].[system_code] ([pid], [code], [data], [description], [sub_description], [weight]) VALUES (2, N'BACKEND_USER_SALT', N'HS(A*2aa', N'Hash Salt for backend', N'Hash Salt for backend', 10)
INSERT [dbo].[system_code] ([pid], [code], [data], [description], [sub_description], [weight]) VALUES (3, N'USERINFO', N'HashKey', N'AEBYCY6DX37DGGKTTMDX9GMC', N'Hash Key For UserInfo', 10)
INSERT [dbo].[system_code] ([pid], [code], [data], [description], [sub_description], [weight]) VALUES (4, N'USERINFO', N'HashIV', N'E5C6GK85', N'Hash Key For UserInfo', 10)
INSERT [dbo].[system_code] ([pid], [code], [data], [description], [sub_description], [weight]) VALUES (5, N'SMTPCONFIG', N'From', N'service@tripler.com.tw', N'', 1)
INSERT [dbo].[system_code] ([pid], [code], [data], [description], [sub_description], [weight]) VALUES (6, N'SMTPCONFIG', N'MailServerAccount', N'colin.server.tpr@gmail.com', N'', 1)
INSERT [dbo].[system_code] ([pid], [code], [data], [description], [sub_description], [weight]) VALUES (7, N'SMTPCONFIG', N'MailServerPassword', N'pqxifpbyizkzrewy', N'', 1)
INSERT [dbo].[system_code] ([pid], [code], [data], [description], [sub_description], [weight]) VALUES (8, N'SMTPCONFIG', N'MailServer', N'smtp.gmail.com', N'', 1)
INSERT [dbo].[system_code] ([pid], [code], [data], [description], [sub_description], [weight]) VALUES (9, N'SMTPCONFIG', N'Port', N'465', N'', 1)
SET IDENTITY_INSERT [dbo].[system_code] OFF
GO
/****** Object:  Index [UQ_USER_ID]    Script Date: 2025/5/14 下午 05:26:22 ******/
ALTER TABLE [dbo].[backend_users] ADD  CONSTRAINT [UQ_USER_ID] UNIQUE NONCLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[backend_users] ADD  CONSTRAINT [DF_BackendUsers_AccessFailedCount]  DEFAULT ((0)) FOR [access_failed_count]
GO
ALTER TABLE [dbo].[backend_users] ADD  CONSTRAINT [DF_BackendUsers_CreateTime]  DEFAULT (getdate()) FOR [create_time]
GO
ALTER TABLE [dbo].[backend_users] ADD  CONSTRAINT [DF_BackendUsers_Validation]  DEFAULT ((0)) FOR [first_login]
GO
ALTER TABLE [dbo].[backend_users] ADD  CONSTRAINT [DF_BackendUsers_EmailConfirmed]  DEFAULT ((0)) FOR [email_confirmed]
GO
ALTER TABLE [dbo].[backend_users] ADD  CONSTRAINT [DF_backend_users_person_in_charge]  DEFAULT ((0)) FOR [person_in_charge]
GO
ALTER TABLE [dbo].[backend_dept]  WITH CHECK ADD  CONSTRAINT [FK_backend_dept_backend_users] FOREIGN KEY([backend_user_pid])
REFERENCES [dbo].[backend_users] ([pid])
GO
ALTER TABLE [dbo].[backend_dept] CHECK CONSTRAINT [FK_backend_dept_backend_users]
GO
ALTER TABLE [dbo].[backend_users_role]  WITH CHECK ADD  CONSTRAINT [FK_backend_users_role_backend_users_role] FOREIGN KEY([user_id])
REFERENCES [dbo].[backend_users] ([user_id])
GO
ALTER TABLE [dbo].[backend_users_role] CHECK CONSTRAINT [FK_backend_users_role_backend_users_role]
GO
ALTER TABLE [dbo].[backend_users_role]  WITH CHECK ADD  CONSTRAINT [FK_backend_users_role_role] FOREIGN KEY([role_id])
REFERENCES [dbo].[role] ([pid])
GO
ALTER TABLE [dbo].[backend_users_role] CHECK CONSTRAINT [FK_backend_users_role_role]
GO
/****** Object:  StoredProcedure [dbo].[usp_get_role_func_all]    Script Date: 2025/5/14 下午 05:26:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[usp_get_role_func_all]
	@roleID int = null
AS
BEGIN

	select rf.* ,
	f.pid , f.[name] , f.[url] , f.parentid , f.[type] , f.icon , f.[weight],
		case  
			when rf.func_id is null then 0
			else 1
		end as isChecked 
	from
	(
		select func_id , role_id from Role_Func with (nolock) where role_id = @roleID
	) as rf 
	right join Func f with (nolock) 
	on rf.func_id = f.pid
	where f.[state] = 1 
	order by f.[weight] asc

END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色權限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'backend_users_role'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'功能清單' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'func'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'權限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'role'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'功能權限' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'role_func'
GO
USE [master]
GO
ALTER DATABASE [StockManage] SET  READ_WRITE 
GO
