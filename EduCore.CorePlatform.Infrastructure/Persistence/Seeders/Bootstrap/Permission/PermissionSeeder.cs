using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Entities.Identity;
using EduCore.CorePlatform.Infrastructure.Persistence.DbContext;
using static EduCore.CorePlatform.Domain.Common.Constants.Auth.PermissionConstants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Seeders.Bootstrap.Permission;
public sealed class PermissionSeeder: IDataSeeder
{
    public int Order => 2;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PermissionSeeder> _logger;

    public PermissionSeeder(IUnitOfWork unitOfWork,
                            ILogger<PermissionSeeder> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.Identity.Permission>();

        //1️ AUTO SCAN PERMISSIONS Lấy toàn bộ permission định nghĩa trong hệ thống
        var permissionDefinitions = PermissionCatalog.All.ToList();

        if (!permissionDefinitions.Any())
        {
            _logger.LogInformation("No permission definitions found.");
            return;
        }

        //2️ Lấy tất cả code cần check
        var codes = permissionDefinitions.Select(x => x.Code)
                                        .ToList();

        //3️ Query 1 lần để lấy các permission đã tồn tại
        var existingPermissions = await repo.ListAsync(new PermissionCodesSpec(codes), cancellationToken);

        var existingCodeSet = existingPermissions.Select(x => x.Code)
                                                .ToHashSet();

        
        //4️ Tạo permission chưa tồn tại
        var newPermissions = new List<Domain.Entities.Identity.Permission>();
        
        foreach (var def in permissionDefinitions)
        {
            if(existingCodeSet.Contains(def.Code))
                continue;

            // TẠO ENTITY ĐƠN GIẢN
            var permission = new Domain.Entities.Identity.Permission(def.Code, def.Name, def.Description, null);

            newPermissions.Add(permission);

            _logger.LogInformation("Permission seeded: {PermissionCode}", def.Code);
        }

        if (!newPermissions.Any())
        {
            _logger.LogInformation("All permissions already seeded.");
            return;
        }

        //5️ Insert batch
        foreach (var permission in newPermissions)
        {
            await repo.AddAsync(permission, cancellationToken);
        }
                
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Permission seeding completed. Total added: {Count}", newPermissions.Count);
    }

}
