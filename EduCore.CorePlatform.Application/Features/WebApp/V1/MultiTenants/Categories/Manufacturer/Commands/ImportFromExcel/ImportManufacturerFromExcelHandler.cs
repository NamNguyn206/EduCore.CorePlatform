using ClosedXML.Excel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Manufacturer.Commands.ImportFromExcel;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Manufacturer.Specifications.Commands;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Manufacturer.Commands.ImportFromExcel;

public sealed class ImportManufacturerFromExcelHandler
    : IRequestHandler<ImportManufacturerFromExcelCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public ImportManufacturerFromExcelHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result> Handle(
        ImportManufacturerFromExcelCommand request,
        CancellationToken cancellationToken)
    {
        if (request.RequestData.FileBytes == null || request.RequestData.FileBytes.Length == 0)
            return Result.Failure(ErrorCodeConstants.FileEmpty, await _systemMessageProvider.GetAsync(ErrorCodeConstants.FileEmpty));

        using var stream = new MemoryStream(request.RequestData.FileBytes);
        using var workbook = new XLWorkbook(stream);
        var ws = workbook.Worksheet(1);
        var rows = ws.RowsUsed().Skip(1); // bỏ header

        if (!rows.Any())
        {
            return Result.Failure(
                ErrorCodeConstants.FileEmpty,
                await _systemMessageProvider.GetAsync(ErrorCodeConstants.FileEmpty));
        }

        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Manufacturer>();

        var spec = new ManufacturerForImportFromExcelSpec(_currentTenant);
        var existingEntities = await repo.ListAsync(spec, cancellationToken);

        foreach (var row in rows)
        {
            var name = row.Cell(1).GetString().Trim();

            if (string.IsNullOrEmpty(name))
                return Result.Failure(ErrorCodeConstants.ManufacturerNameRequired, await _systemMessageProvider.GetAsync(ErrorCodeConstants.ManufacturerNameRequired));

            var existing = existingEntities.FirstOrDefault(x => x.Name.Equals(name));
            if (existing == null)
            {
                var entity = new Domain.Entities.MultiTenants.Categories.Manufacturer(_currentTenant.TenantId!.Value, name);

                await repo.AddAsync(entity, cancellationToken);
                existingEntities.Add(entity);
            }
            else
            {
                existing.Update(name);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
