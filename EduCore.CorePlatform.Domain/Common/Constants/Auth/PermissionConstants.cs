using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Common.Constants.Auth;

public static class PermissionConstants
{
    // System Admin permissions
    public const string SuperAdminFullAccess = "system.admin.full_access";

    // Tenant management permissions
    public static class Tenants
    {
        public const string Create = "Tenants.Create";
        public const string Read = "Tenants.Read";
        public const string Update = "Tenants.Update";
        public const string Delete = "Tenants.Delete";
        public const string Manage = "Tenants.Manage";
        public const string Admin = "Tenants.Admin";
        public const string FullAccess = "Tenants.FullAccess";
    }
    public static class Users
    {
        public const string View = "Users.View";
        public const string Create = "Users.Create";
        public const string Update = "Users.Update";
        public const string Delete = "Users.Delete";
        public const string Manage = "Users.Manage";
        public const string Admin = "Users.Admin";
        public const string AssignRole = "Users.AssignRole";
    }
    public static class UserPermission // self
    {
        public const string ViewProfile = "User.Profile.View";
        public const string UpdateProfile = "User.Profile.Update";
        public const string ChangePassword = "User.ChangePassword";
    }
    public static class System
    {
        public const string Manage = "System.Manage";
        public const string Config = "System.Config";
        public const string FullAccess = "System.FullAccess";
    }

    public static class Roles
    {
        public const string View = "Roles.View";
        public const string Create = "Roles.Create";
        public const string Update = "Roles.Update";
        public const string Delete = "Roles.Delete";
        public const string Manage = "Role.Manage";
    }

    public static class Permission
    {         
        public const string View = "Permission.View";
        public const string Sync = "Permission.Sync";
    }

    public static class AuditLogs
    {
        public const string View = "AuditLogs.View";
        public const string Export = "AuditLogs.Export";
    }

    public static class CustomFields
    {
        public const string Create = "CustomFields.Create";
        public const string View = "CustomFields.View";
        public const string Update = "CustomFields.Update";
        public const string Delete = "CustomFields.Delete";
    }

    public static class Webhooks
    {
        public const string Create = "Webhooks.Create";
        public const string View = "Webhooks.View";
        public const string Update = "Webhooks.Update";
        public const string Delete = "Webhooks.Delete";
    }
  
    //// Thêm các permission groups
    //public const string DefaultPermissionGroup = "General";
    //public const string SystemPermissionGroup = "System";
    //public const string TenantPermissionGroup = "Tenant";
    //public const string UserPermissionGroup = "User";
    //public const string RolePermissionGroup = "Role";
    //public const string AuditPermissionGroup = "Audit";
    //public const string SettingsPermissionGroup = "Settings";
    //public const string CustomFieldPermissionGroup = "Custom Fields";
    //public const string WebhookPermissionGroup = "Webhook";
    //public const string MobilePermissionGroup = "Mobile";

    //// Thêm dictionary để phân loại permission theo group
    //public static readonly Dictionary<string, string> PermissionGroups = new()
    //{
    //    { SuperAdminFullAccess, SystemPermissionGroup },
    //    { TenantCreate, TenantPermissionGroup },
    //    { TenantRead, TenantPermissionGroup },
    //    { TenantUpdate, TenantPermissionGroup },
    //    { TenantDelete, TenantPermissionGroup },
    //    { TenantManage, TenantPermissionGroup },
    //    { Users.Create, UserPermissionGroup },
    //    { Users.View, UserPermissionGroup },
    //    { Users.Update, UserPermissionGroup },
    //    { Users.Delete, UserPermissionGroup },
    //    { Users.Manage, UserPermissionGroup },
    //    { RoleCreate, RolePermissionGroup },
    //    { RoleRead, RolePermissionGroup },
    //    { RoleUpdate, RolePermissionGroup },
    //    { RoleDelete, RolePermissionGroup },
    //    { RoleManage, RolePermissionGroup },
    //    { PermissionSync, SystemPermissionGroup },
    //    { PermissionRead, SystemPermissionGroup },
    //    { AuditLogRead, AuditPermissionGroup },
    //    { AuditLogExport, AuditPermissionGroup },
    //    { SettingsRead, SettingsPermissionGroup },
    //    { SettingsUpdate, SettingsPermissionGroup },
    //    { CustomFieldCreate, CustomFieldPermissionGroup },
    //    { CustomFieldRead, CustomFieldPermissionGroup },
    //    { CustomFieldUpdate, CustomFieldPermissionGroup },
    //    { CustomFieldDelete, CustomFieldPermissionGroup },
    //    { WebhookCreate, WebhookPermissionGroup },
    //    { WebhookRead, WebhookPermissionGroup },
    //    { WebhookUpdate, WebhookPermissionGroup },
    //    { WebhookDelete, WebhookPermissionGroup },
    //    { MobileApiAccess, MobilePermissionGroup },
    //    { MobileSync, MobilePermissionGroup }
    //};

    //public static readonly Dictionary<string, string> PermissionDescriptions = new()
    //{
    //    { SuperAdminFullAccess, "Toàn quyền truy cập hệ thống" },
    //    { TenantCreate, "Tạo mới tenant" },
    //    { TenantRead, "Xem thông tin tenant" },
    //    { TenantUpdate, "Cập nhật tenant" },
    //    { TenantDelete, "Xóa tenant" },
    //    { TenantManage, "Quản lý tenant" },
    //    { Users.Create, "Tạo mới người dùng" },
    //    { Users.View, "Xem thông tin người dùng" },
    //    { Users.Update, "Cập nhật người dùng" },
    //    { Users.Delete, "Xóa người dùng" },
    //    { Users.Manage, "Quản lý người dùng" },
    //    { RoleCreate, "Tạo mới vai trò" },
    //    { RoleRead, "Xem thông tin vai trò" },
    //    { RoleUpdate, "Cập nhật vai trò" },
    //    { RoleDelete, "Xóa vai trò" },
    //    { RoleManage, "Quản lý vai trò" },
    //    { PermissionSync, "Đồng bộ permission từ controller" },
    //    { PermissionRead, "Xem danh sách permission" },
    //    { AuditLogRead, "Xem lịch sử thao tác" },
    //    { AuditLogExport, "Xuất lịch sử thao tác" },
    //    { SettingsRead, "Xem cài đặt" },
    //    { SettingsUpdate, "Cập nhật cài đặt" },
    //    { CustomFieldCreate, "Tạo trường dữ liệu tùy chỉnh" },
    //    { CustomFieldRead, "Xem trường dữ liệu tùy chỉnh" },
    //    { CustomFieldUpdate, "Cập nhật trường dữ liệu tùy chỉnh" },
    //    { CustomFieldDelete, "Xóa trường dữ liệu tùy chỉnh" },
    //    { WebhookCreate, "Tạo webhook" },
    //    { WebhookRead, "Xem webhook" },
    //    { WebhookUpdate, "Cập nhật webhook" },
    //    { WebhookDelete, "Xóa webhook" },
    //    { MobileApiAccess, "Truy cập API mobile" },
    //    { MobileSync, "Đồng bộ dữ liệu mobile" }
    //};

    public static class Articles
    {
        public const string View = "Articles.View";
        public const string Create = "Articles.Create";
        public const string Update = "Articles.Update";
        public const string Delete = "Articles.Delete";
        public const string Manage = "Articles.Manage";
    }
}