using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Common.Constants.Identities;

public static class RoleConstants
{
    //System Roles
    public const string SuperAdmin = "SuperAdmin";


    //Tenant Roles
    public const string SchoolOwner = "SchoolOwner";
    public const string TenantAdmin = "TenantAdmin";
    public const string Principal = "Principal";
    public const string Teacher = "Teacher";
    public const string Parent = "Parent";
    public const string Driver = "Driver";
    public const string SecurityOfficer = "SecurityOfficer";
    public const string Nurse = "Nurse";
    public const string MedicalStaff = "MedicalStaff";
    public const string Student = "Student";
}
