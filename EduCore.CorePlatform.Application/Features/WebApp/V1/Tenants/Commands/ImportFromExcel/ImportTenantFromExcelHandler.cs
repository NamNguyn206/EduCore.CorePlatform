using ClosedXML.Excel;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using EduCore.CorePlatform.Application.Common.Extensions;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.Tenants.Specifications.Commands;
using EduCore.CorePlatform.Application.Abstractions.Authentication;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Enums;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Domain.Common.Settings.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.Tenants.Commands.ImportFromExcel;

public sealed class ImportTenantFromExcelHandler: IRequestHandler<ImportTenantFromExcelCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ICurrentUser _currentUser;
    private readonly ISystemMessageProvider _systemMessageProvider;
    private readonly DataTypeFormatSettings _dataTypeFormatSettings;
    public ImportTenantFromExcelHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant,
        ICurrentUser currentUser,
        ISystemMessageProvider systemMessageProvider,
        IOptions<DataTypeFormatSettings> dataTypeFormatSettings)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _currentUser = currentUser;
        _systemMessageProvider = systemMessageProvider;
        _dataTypeFormatSettings = dataTypeFormatSettings.Value;
    }

    public async Task<Result> Handle(
        ImportTenantFromExcelCommand request,
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

        var repo = _unitOfWork.Repository<Tenant>();

        var spec = new TenantForImportFromExcelSpec();
        var existingEntities = await repo.ListAsync(spec, cancellationToken);

        foreach (var row in rows)
        {
            DateTime? subscriptionStartDateValue = null, subscriptionEndDateValue = null;

            var code = row.Cell(1).GetString().Trim();
            var name = row.Cell(2).GetString().Trim();
            var description = row.Cell(3).GetString().Trim();
            var email = row.Cell(4).GetString().Trim();
            var phone = row.Cell(5).GetString().Trim();
            var address = row.Cell(6).GetString().Trim();
            var webste = row.Cell(7).GetString().Trim();
            var subscriptionStartDate = row.Cell(8).GetString().Trim();
            var subscriptionEndDate = row.Cell(9).GetString().Trim();

            if (string.IsNullOrEmpty(code))
                return Result.Failure(ErrorCodeConstants.TenantCodeRequired, await _systemMessageProvider.GetAsync(ErrorCodeConstants.TenantCodeRequired));

            if (!string.IsNullOrEmpty(subscriptionStartDate)
                && DateTime.TryParseExact(subscriptionStartDate, _dataTypeFormatSettings.DateFormat_ddMMyyyy, null, DateTimeStyles.None, out var subscriptionStartDateDateTime))
            {
                subscriptionStartDateValue = subscriptionStartDateDateTime;
            }

            if (!string.IsNullOrEmpty(subscriptionEndDate)
                && DateTime.TryParseExact(subscriptionEndDate, _dataTypeFormatSettings.DateFormat_ddMMyyyy, null, DateTimeStyles.None, out var subscriptionEndDateValueDateTime))
            {
                subscriptionEndDateValue = subscriptionEndDateValueDateTime;
            }

            var existing = existingEntities.FirstOrDefault(x => x.Code.Equals(code));
            if (existing == null)
            {
                var entity = new Tenant(
                    code,    
                    name,
                    description,
                    email,
                    phone,
                    address,
                    webste, 
                    subscriptionStartDateValue,
                    subscriptionEndDateValue,
                    10,
                    1024,
                    null,
                    _currentUser.UserId
                );

                await repo.AddAsync(entity, cancellationToken);
                existingEntities.Add(entity);
            }
            else
            {
                existing.Update(
                    name,
                    description,
                    email,
                    phone,
                    address,
                    webste,
                    subscriptionStartDateValue,
                    subscriptionEndDateValue,
                    10,
                    1024,
                    null,
                    _currentUser.UserId
                );
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
