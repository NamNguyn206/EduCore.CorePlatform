using ClosedXML.Excel;
using EduCore.CorePlatform.Application.Abstractions.Localization;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Specifications.Commands;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Domain.Common.Enums.MultiTenants.CRM;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Commands.ImportFromExcel;

public sealed class ImportLeadFromExcelHandler
    : IRequestHandler<ImportLeadFromExcelCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public ImportLeadFromExcelHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result> Handle(
        ImportLeadFromExcelCommand request,
        CancellationToken cancellationToken)
    {
        if (request.RequestData.FileBytes == null || request.RequestData.FileBytes.Length == 0)
            return Result.Failure(ErrorCodeConstants.FileEmpty, await _systemMessageProvider.GetAsync(ErrorCodeConstants.FileEmpty));

        using var stream = new MemoryStream(request.RequestData.FileBytes);
        using var workbook = new XLWorkbook(stream);
        var ws = workbook.Worksheet(1);
        var rows = ws.RowsUsed().Skip(1); // Bỏ qua dòng header

        if (!rows.Any())
        {
            return Result.Failure(
                ErrorCodeConstants.FileEmpty,
                await _systemMessageProvider.GetAsync(ErrorCodeConstants.FileEmpty));
        }

        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.CRM.Lead>();

        var spec = new LeadForImportFromExcelSpec(_currentTenant);
        var existingEntities = await repo.ListAsync(spec, cancellationToken);

        var tenantId = _currentTenant.TenantId!.Value;

        foreach (var row in rows)
        {
            var fullName = row.Cell(1).GetString().Trim();
            var email = row.Cell(2).GetString().Trim();
            var phoneNumber = row.Cell(3).GetString().Trim();

            if (string.IsNullOrEmpty(fullName))
                return Result.Failure(ErrorCodeConstants.LeadNameRequired, await _systemMessageProvider.GetAsync(ErrorCodeConstants.LeadNameRequired));

            // Parse các thuộc tính bổ sung
            int.TryParse(row.Cell(4).GetString().Trim(), out var score);
            int.TryParse(row.Cell(5).GetString().Trim(), out var prediction);

            var tempStr = row.Cell(6).GetString().Trim();
            Enum.TryParse<LeadTemperature>(tempStr, true, out var temperature);

            var aiNextAction = row.Cell(7).GetString().Trim();

            // Kiểm tra Lead đã tồn tại dựa theo Email hoặc Phone (nếu có)
            var existing = existingEntities.FirstOrDefault(x =>
                (!string.IsNullOrEmpty(email) && x.Email == email) ||
                (!string.IsNullOrEmpty(phoneNumber) && x.PhoneNumber == phoneNumber));

            if (existing == null)
            {
                var entity = new Domain.Entities.MultiTenants.CRM.Lead(
                    fullName,
                    tenantId,
                    string.IsNullOrEmpty(phoneNumber) ? null : phoneNumber,
                    string.IsNullOrEmpty(email) ? null : email);

                entity.UpdateScoreAndPrediction(score, prediction, temperature);

                if (!string.IsNullOrEmpty(aiNextAction))
                {
                    entity.SetAiNextAction(aiNextAction);
                }

                await repo.AddAsync(entity, cancellationToken);
                existingEntities.Add(entity);
            }
            else
            {
                existing.UpdateScoreAndPrediction(score, prediction, temperature);
                if (!string.IsNullOrEmpty(aiNextAction))
                {
                    existing.SetAiNextAction(aiNextAction);
                }
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}