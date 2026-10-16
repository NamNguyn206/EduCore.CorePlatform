using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Common.Settings.System;

public class FileStorageSettings
{
    public string? Provider { get; set; }
    public string? DownloadRootPath { get; set; }
    public string? MaxFileSizeMB { get; set; }

    //Tenant
    public string? TenantPath { get; set; }

    //Asset
    public string? AssetPath { get; set; }
    public string? AssetAllocationPath { get; set; }
    public string? AssetBatchPath { get; set; }
    public string? AssetRetirementPath { get; set; }
    public string? AssetDisposalRequestPath { get; set; }

    //User
    public string? UserPath { get; set; }
    public string? UserAvatarPath { get; set; }
    public string? UserCCCDPath { get; set; }
    public string? UserDrivingLicensePath { get; set; }
    public string? UserVehicleRegistrationPath { get; set; }
    public string? UserVehicleInsurancePath { get; set; }
}

