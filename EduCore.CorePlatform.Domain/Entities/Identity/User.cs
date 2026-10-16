using EduCore.CorePlatform.Domain.Common.Constants.Identities;
using EduCore.CorePlatform.Domain.Common.Entities;
using EduCore.CorePlatform.Domain.Common.Enums.Identities;
using EduCore.CorePlatform.Domain.Entities.Localization;
using EduCore.CorePlatform.Domain.Entities.MultiTenants.Identities;
using EduCore.CorePlatform.Domain.Entities.StudentManagement;
using EduCore.CorePlatform.Domain.Events.Domain.User;

namespace EduCore.CorePlatform.Domain.Entities.Identity;

public class User : AggregateRoot<Guid>
{
    public Guid Username { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string Email { get; private set; }
    public bool EmailConfirmed { get; private set; }
    public bool PhoneNumberConfirmed { get; private set; }    
    public string? SecurityStamp { get; private set; }
    public UserStatus Status { get; private set; } = UserStatus.PendingActivation;
    public UserType UserType { get; private set; } = UserType.User;
    public Guid? LanguageId { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public int FailedLoginAttempts { get; private set; }
    public DateTime? LockoutEnd { get; private set; }
    public string? DeviceId { get; private set; }
    public UserVerificationStatus VerificationStatus { get; private set; } = UserVerificationStatus.Pending;

    public Guid? VerifiedBy { get; private set; }

    public DateTime? VerifiedAt { get; private set; }

    public string? VerificationRejectReason { get; private set; }

    public Guid? LastTenantId { get; private set; }
    public Guid? StudentId { get; private set; }

    // Navigation properties
    public ICollection<UserTenant> UserTenants { get; private set; } = new List<UserTenant>();
    private readonly List<UserSystemRole> _systemRoles = new();
    public IReadOnlyCollection<UserSystemRole> SystemRoles => _systemRoles;
    public ICollection<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();
    public Language? Language { get; private set; }
    private readonly List<UserDocument> _documents = new();

    public IReadOnlyCollection<UserDocument> Documents => _documents;

    public Student? Student { get; private set; }
    // Constructor
    private User() { }

    public User(string email, string passwordHash, 
                string? firstName, string? lastName)
    {
        Email = email.Trim().ToLower();
        PasswordHash = passwordHash;

        FirstName = firstName;
        LastName = lastName;

        UserType = UserType.User;

        SecurityStamp = Guid.NewGuid().ToString();
        Status = UserStatus.PendingActivation;
        
        CreatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new UserCreatedEvent(Id, Email));
    }

    // Factory method
    public static User Create(
        string email,
        string passwordHash,
        string? firstName,
        string? lastName,
        Guid tenantId,
        IEnumerable<Guid> defaultRoleIds,
        bool isTenantAdmin = false
    )
    {
        var user = new User(email, passwordHash, firstName, lastName);

        // Tạo quan hệ Tenant
        var userTenant = new UserTenant(user.Id, tenantId, isTenantAdmin);

        user.RaiseDomainEvent(new UserCreatedEvent(user.Id, user.Email));

        // Gán role mặc định
        foreach (var roleId in defaultRoleIds)
        {
            userTenant.AddRole(roleId);
        }

        user.UserTenants.Add(userTenant);

        return user;
    }
    public static User CreateSuperAdmin(
        string email,
        string passwordHash)
    {
        var user = new User()
        {
            Email = email,
            PasswordHash = passwordHash,
            Status = UserStatus.Active,
            UserType = Common.Enums.Identities.UserType.SuperAdmin,
            CreatedAt = DateTime.UtcNow
        };

        user.RaiseDomainEvent(new SuperAdminCreatedEvent(user.Id, user.Email));

        return user;
    }
    public void PromoteToSuperAdmin()
    {
        UserType = Common.Enums.Identities.UserType.SuperAdmin;
    }
    // Methods
    public string GetFullName()
    {
        return $"{FirstName} {LastName}".Trim();
    }

    // =========================
    // LOGIN DOMAIN LOGIC
    // =========================
    public void SetDeviceId(string deviceId)
    {
        DeviceId = deviceId;
    }
    public bool IsLocked()
    {
        return LockoutEnd.HasValue && LockoutEnd.Value > DateTime.UtcNow;
    }
    public void ConfirmEmail()
    {
        if (EmailConfirmed)
            return;

        EmailConfirmed = true;

        if (Status == UserStatus.PendingActivation)
        {
            Status = UserStatus.Active;
        }

        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new UserProfileUpdatedEvent(Id));
    }

    public void ConfirmPhoneNumber()
    {
        PhoneNumberConfirmed = true;
        RaiseDomainEvent(new UserProfileUpdatedEvent(Id));
    }

    public void UpdateProfile(string? firstName, string? lastName, string? phoneNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new UserProfileUpdatedEvent(Id));
    }
    public void AddToTenant(Guid tenantId, bool isTenantAdmin, IEnumerable<Guid> roleIds)
    {
        var userTenant = new UserTenant(Id, tenantId, isTenantAdmin);

        foreach (var roleId in roleIds)
            userTenant.AddRole(roleId);

        UserTenants.Add(userTenant);
    }
    public void RecordSuccessfulLogin()
    {
        LastLoginAt = DateTime.UtcNow;

        ResetLockout();

        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordFailedLogin(int maxAttempts = 5)
    {
        FailedLoginAttempts++;

        if (FailedLoginAttempts >= maxAttempts)
        {
            LockoutEnd = DateTime.UtcNow.AddMinutes(30);
        }

        UpdatedAt = DateTime.UtcNow;
    }

    public void ResetLockout()
    {
        FailedLoginAttempts = 0;
        LockoutEnd = null;
        UpdatedAt = DateTime.UtcNow;
    }
    // =========================
    // SECURITY
    // =========================
    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        SecurityStamp = Guid.NewGuid().ToString();
        UpdatedAt = DateTime.UtcNow;
    }
    public void Activate()
    {
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Status = UserStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Suspend()
    {
        Status = UserStatus.Suspended;
        UpdatedAt = DateTime.UtcNow;
    }
    public void Unsuspend()
    {
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Delete(Guid? deletedBy = null)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
        Status = UserStatus.Deleted;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }
    public void ChangeLanguage(Guid languageId, Guid? updatedByUserId)
    {
        LanguageId = languageId;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedByUserId;

        RaiseDomainEvent(new UserLanguageChangedEvent(Id, languageId, updatedByUserId));
    }
    public void Verify(Guid verifiedBy)
    {
        VerificationStatus = UserVerificationStatus.Verified;

        VerifiedBy = verifiedBy;

        VerifiedAt = DateTime.UtcNow;

        VerificationRejectReason = null;

        UpdatedAt = DateTime.UtcNow;
    }

    public void RejectVerification(
        Guid verifiedBy,
        string reason)
    {
        VerificationStatus = UserVerificationStatus.Rejected;

        VerifiedBy = verifiedBy;

        VerifiedAt = DateTime.UtcNow;

        VerificationRejectReason = reason;

        UpdatedAt = DateTime.UtcNow;
    }
    public void AddDocument(
        UserDocumentType type,
        string fileUrl
    )
    {
        _documents.Add(
            new UserDocument(
                Id,
                type,
                fileUrl));
    }
   
    public void AssignSystemRole(Guid roleId)
    {
        if (_systemRoles.Any(x => x.RoleId == roleId))
            return;

        _systemRoles.Add(
            new UserSystemRole(
                Id,
                roleId));
    }
    public void SetLastTenant(Guid tenantId)
    {
        LastTenantId = tenantId;
        UpdatedAt = DateTime.UtcNow;
    }
    public bool CanAccessWebApp()
    {
        if (UserType == UserType.SuperAdmin)
            return true;

        var allowedRoles = new[]
        {
            RoleConstants.SuperAdmin,
            RoleConstants.SchoolOwner,
            RoleConstants.TenantAdmin,
            RoleConstants.Principal
        };

        return UserTenants.Where(x => x.LeftAt == null)
                            .SelectMany(x => x.Roles)
                            .Any(x => allowedRoles.Contains(x.Role.Code));
    }
    public bool CanAccessMobileApp()
    {
        // SuperAdmin luôn được phép
        if (UserType == UserType.SuperAdmin)
            return true;

        var allowedRoles = new[]
        {
            RoleConstants.SchoolOwner,
            RoleConstants.TenantAdmin,
            RoleConstants.Principal,
            RoleConstants.Teacher,
            RoleConstants.Parent,
            RoleConstants.Driver,
            RoleConstants.SecurityOfficer,
            RoleConstants.MedicalStaff,
            RoleConstants.Student
        };

        return UserTenants
            .Where(x => x.LeftAt == null)
            .SelectMany(x => x.Roles)
            .Any(x => allowedRoles.Contains(x.Role.Code));
    }
}