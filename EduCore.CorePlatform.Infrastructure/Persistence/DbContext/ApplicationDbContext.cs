using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Events;
using EduCore.CorePlatform.Domain.Entities.Identity;
using EduCore.CorePlatform.Domain.Entities.Localization;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;
using EduCore.CorePlatform.Domain.Entities.MultiTenants.Categories;
using EduCore.CorePlatform.Domain.Entities.MultiTenants.Identities;
using EduCore.CorePlatform.Domain.Entities.System.Categories;
using EduCore.CorePlatform.Domain.Entities.System.Files;
using EduCore.CorePlatform.Domain.Entities.System.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace EduCore.CorePlatform.Infrastructure.Persistence.DbContext;

public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    private readonly Guid? _currentTenantId;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentTenant currentTenant)
        : base(options)
    {
        _currentTenantId = currentTenant?.TenantId;
    }
    
    // ============================
    // Model configuration
    // ============================
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //ÁP DỤNG CONFIGURATION TỰ ĐỘNG
        //1 dòng → load toàn bộ configuration
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // =========================================
        // Ignore domain events (NOT entities)
        // Loại bỏ DomainEvent khỏi EF model
        // =========================================
        modelBuilder.Ignore<DomainEvent>();

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            ApplySoftDeleteFilter(modelBuilder, entityType);
            ApplyTenantFilter(modelBuilder, entityType);
        }

    }
    private void ApplySoftDeleteFilter(ModelBuilder modelBuilder, IMutableEntityType entityType)
    {
        var prop = entityType.FindProperty("IsDeleted");
        if (prop == null) return;

        var parameter = Expression.Parameter(entityType.ClrType, "e");
        var body = Expression.Equal(
            Expression.Property(parameter, "IsDeleted"),
            Expression.Constant(false)
        );

        var lambda = Expression.Lambda(body, parameter);

        modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
    }

    private void ApplyTenantFilter(ModelBuilder modelBuilder, IMutableEntityType entityType)
    {
        if (_currentTenantId == null) return;

        var prop = entityType.FindProperty("TenantId");
        if (prop == null) return;

        var parameter = Expression.Parameter(entityType.ClrType, "e");

        var body = Expression.Equal(
            Expression.Property(parameter, "TenantId"),
            Expression.Constant(_currentTenantId.Value)
        );

        var lambda = Expression.Lambda(body, parameter);

        modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
    }

    // ============================
    // DbSets
    // ============================

    //Auth
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<EduCore.CorePlatform.Domain.Entities.Identity.Role> Roles => Set<EduCore.CorePlatform.Domain.Entities.Identity.Role>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<UserTenantRole> UserTenantRoles => Set<UserTenantRole>();

    //Identity
    //public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserTenant> UserTenants => Set<UserTenant>();
    public DbSet<UserDocument> UserDocuments => Set<UserDocument>();


    //MultiTenants
    public DbSet<Tenant> Tenants => Set<Tenant>();


    //System
    public DbSet<FileStorage> FileStorages => Set<FileStorage>();
    public DbSet<SystemMessage> SystemMessages => Set<SystemMessage>();
    
    

    //Localization
    public DbSet<SystemMessageTranslation> SystemMessageTranslations => Set<SystemMessageTranslation>();
    public DbSet<Language> Languages => Set<Language>();

    //Category - Global
    public DbSet<Country> Countries => Set<Country>();

    //MultiTenants

    //MultiTenants - Category
    public DbSet<Bank> Banks => Set<Bank>();
    public DbSet<Department> Departments => Set<Department>();    
    public DbSet<Manufacturer> Manufacturers => Set<Manufacturer>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<SupplyCategory> SupplyCategories => Set<SupplyCategory>();
    public DbSet<Unit> Units => Set<Unit>();

    
}