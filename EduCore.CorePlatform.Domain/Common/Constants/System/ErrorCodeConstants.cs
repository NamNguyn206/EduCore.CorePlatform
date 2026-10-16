using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Common.Constants.System;

public static class ErrorCodeConstants
{
    // Common (000-999)
    public const string ConcurrentUpdate = "COM_001";
    public const string NotFound = "COM_002";
    public const string SessionExpired = "COM_003";
    
    // Validation (1000-1999)
    public const string ValidationError = "VAL_1000";
    public const string RequiredField = "VAL_1001";
    public const string InvalidFormat = "VAL_1002";
    public const string OutOfRange = "VAL_1003";
    public const string DuplicateValue = "VAL_1004";
    public const string InvalidRequest = "VAL_1005";
    public const string EmailRequired = "VAL_1006";
    public const string UsernameRequired = "VAL_1007";
    public const string PasswordRequired = "VAL_1008";
    public const string RefreshTokenRequired = "VAL_1009";
    public const string InvalidClientType = "VAL_1010";
    public const string InvalidEmail = "VAL_1011";
    public const string InvalidUser = "VAL_1012";
    public const string InvalidRole = "VAL_1013";
    public const string UserRequired = "VAL_1014";
    public const string NameRequired = "VAL_1015";
    public const string CodeRequired = "VAL_1016";


    // authentication (2000-2999)    
    public const string InvalidToken = "AUTH_2001";
    public const string TokenExpired = "AUTH_2002";
    public const string AccessDenied = "AUTH_2003";
    public const string InvalidCredentials = "AUTH_2004";
    public const string Unauthorized = "AUTH_2005";
    public const string Forbidden = "AUTH_2006";    
    public const string AuthenticationFailed = "AUTH_2007";
    public const string UserSuspended = "AUTH_2008";
    public const string UserDeleted = "AUTH_2009";
    public const string InvalidRefreshToken = "AUTH_2010";
    public const string PasswordMinLength = "AUTH_2011";
    public const string RefreshTokenExpired = "AUTH_2012";
    public const string UserInactive = "AUTH_2013";
    public const string RefreshTokenRevoked = "AUTH_2014";
    public const string UserLocked = "AUTH_2015";
    public const string EmailAlreadyExists = "AUTH_2016";
    public const string UserPendingActivation = "AUTH_2017";
    public const string EmailNotConfirmed = "AUTH_2018";
    public const string WebAppAccessDenied = "AUTH_2019";
    public const string MobileAppAccessDenied = "AUTH_2020";



    // Authorization errors (3000-3999)
    public const string TenantAccessDenied = "PER_3000";
    public const string PermissionDenied = "PER_3001";
    public const string RoleRequired = "PER_3002";
    public const string InvalidTenantHeader = "PER_3003";
    public const string UserNotInTenant = "PER_3004";
    public const string NoTenantAssigned = "PER_3005";
    public const string RoleNotFound = "PER_3006";

    // Business logic (4000-4999)
    public const string EntityNotFound = "BUS_4000";
    public const string EntityInUse = "BUS_4001";
    public const string InvalidState = "BUS_4002";
    public const string BusinessRuleViolation = "BUS_4003";
    public const string ConcurrentModification = "BUS_4004";
    public const string CannotDeleteInUse = "BUS_4005";
    public const string DuplicateEntity = "BUS_4006";

    // System (5000-5999)
    public const string SystemError = "SYS_5000";
    public const string ExternalServiceError = "SYS_5001";
    public const string DatabaseError = "SYS_5002";
    public const string SuperAdminAlreadyExists = "SYS_5003";
    public const string SuperAdminNotFound = "SYS_5004";
    public const string InternalError = "SYS_5005";
    public const string ConfigurationError = "SYS_5006";
    public const string SuperAdminAlreadyInitialized = "SYS_5007";
    public const string BootstrapSuperAdminDisabled = "SYS_5008";

    // Custom field errors (6000-6999)
    public const string CustomFieldValidation = "CUSTOM_FIELD_6000";
    public const string CustomFieldNotFound = "CUSTOM_FIELD_6001";
    public const string CustomFieldError = "CUSTOM_FIELD_6002";
     
    // Webhook (7000-7999)
    public const string WebhookDeliveryFailed = "WEBHOOK_7000";
    public const string WebhookConfigError = "WEBHOOK_7001";
    public const string WebhookError = "WEBHOOK_7002";   

    // Mobile (8000-8999)
    public const string SyncConflict = "MOBILE_8000";
    public const string DeviceNotRegistered = "MOBILE_8001";
    public const string MobileError = "MOBILE_8002";    

    // Tenant (9000-9999)
    public const string TenantNotFound = "TENANT_9000";
    public const string InvalidTenant = "TENANT_9001";
    public const string TenantAlreadyExists = "TENANT_9002";
    public const string OneOrMoreTenantIdsDoNotExist = "TENANT_9003";
    public const string TenantCodeRequired = "TENANT_9004";
    

    // User (10000-10999)
    public const string UserNotFound = "USER_10000";
    public const string UserAlreadyExists = "USER_10001";
    public const string UserAlreadyInTenant = "USER_10002";
    public const string YouCannotRemoveYourselfFromTenant = "USER_10003";
    public const string CannotRemoveSuperAdmin = "USER_10004";
    public const string CannotRemoveTheLastTenantAdmin = "USER_10005";
    public const string UserVerificationPending = "USER_10006";



    // DBContext Query filters (11000-11999)
    public const string QueryFiltersNotInitialized = "QueryFilters_11000";

    //File (12000-12999)
    public const string FileEmpty = "FILE_12000";

    //Supplier (13000-13999)
    public const string SupplierCodeExists = "SUPPLIER_13000";
    public const string SupplierNotFound = "SUPPLIER_13001";
    public const string SupplierCodeRequired = "SUPPLIER_13002";
    public const string SupplierNameRequired = "SUPPLIER_13003";
    public const string OneOrMoreSupplierIdsDoNotExist = "SUPPLIER_13004";
    

    //SupplyCategory (14000-14999)
    public const string SupplyCategoryNameExists = "SUPPLY_CATEGORY_14000";
    public const string SupplyCategoryNotFound = "SUPPLY_CATEGORY_14001";
    public const string SupplyCategoryNameRequired = "SUPPLY_CATEGORY_14002";
    public const string OneOrMoreSupplyCategoryIdsDoNotExist = "SUPPLY_CATEGORY_14003";

    //Bank (15000-15999)
    public const string BankNameExists = "BANK_15000";
    public const string BankNotFound = "BANK_15001";
    public const string BankNameRequired = "BANK_15002";
    public const string OneOrMoreBankIdsDoNotExist = "BANK_15003";

    //CRM (16000-16999)
    public const string LeadNotFound = "LEAD_16000";
    public const string OneOrMoreLeadIdsDoNotExist = "LEAD_16001";
    public const string LeadNameRequired = "LEAD_16002";


    //Department (21000-21999)
    public const string DepartmentNameExists = "DEPARTMENT_21000";
    public const string DepartmentNotFound = "DEPARTMENT_21001";
    public const string DepartmentNameRequired = "DEPARTMENT_21002";
    public const string DepartmentAddressRequired = "DEPARTMENT_21003";
    public const string OneOrMoreDepartmentIdsDoNotExist = "DEPARTMENT_21004";

    //Country (22000-22999)
    public const string CountryNameExists = "COUNTRY_22000";
    public const string CountryNotFound = "COUNTRY_22001";
    public const string CountryNameRequired = "COUNTRY_22002";
    public const string OneOrMoreCountryIdsDoNotExist = "COUNTRY_22003";

    //Manufacturer (23000-23999)
    public const string ManufacturerNameExists = "MANUFACTURER_23000";
    public const string ManufacturerNotFound = "MANUFACTURER_23001";
    public const string ManufacturerNameRequired = "MANUFACTURER_23002";
    public const string OneOrMoreManufacturerIdsDoNotExist = "MANUFACTURER_23003";

    //Unit (24000-24999)
    public const string UnitNameExists = "UNIT_24000";
    public const string UnitNotFound = "UNIT_24001";
    public const string UnitNameRequired = "UNIT_24002";
    public const string OneOrMoreUnitIdsDoNotExist = "UNIT_24003";

    //Language (25000-25999)
    public const string LanguageCodeExists = "LANGUAGE_25000";
    public const string LanguageNotFound = "LANGUAGE_25001";
    public const string LanguageCodeRequired = "LANGUAGE_25002";
    public const string OneOrMoreLanguageIdsDoNotExist = "LANGUAGE_25003";

  

    //Warehouse (29000-29999)
    public const string WarehouseNameExists = "WAREHOUSE_29000";
    public const string WarehouseNotFound = "WAREHOUSE_29001";
    public const string WarehouseNameRequired = "WAREHOUSE_29002";
    public const string OneOrMoreWarehouseIdsDoNotExist = "WAREHOUSE_29003";

}