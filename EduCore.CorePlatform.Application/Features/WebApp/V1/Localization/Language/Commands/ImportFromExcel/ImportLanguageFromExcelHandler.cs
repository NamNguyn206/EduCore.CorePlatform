using ClosedXML.Excel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.Specifications.Commands;
using EduCore.CorePlatform.Application.Abstractions.Authentication;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Localization.Language.Commands.ImportFromExcel;

public sealed class ImportLanguageFromExcelHandler : IRequestHandler<ImportLanguageFromExcelCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ICurrentUser _currentUser;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public ImportLanguageFromExcelHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant,
        ICurrentUser currentUser,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _currentUser = currentUser;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result> Handle(
        ImportLanguageFromExcelCommand request,
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

        var repo = _unitOfWork.Repository<Domain.Entities.Localization.Language>();

        var spec = new LanguageForImportFromExcelSpec();
        var existingEntities = await repo.ListAsync(spec, cancellationToken);

        foreach (var row in rows)
        {
            var code = row.Cell(1).GetString().Trim();
            var name = row.Cell(2).GetString().Trim();

            if (string.IsNullOrEmpty(code))
                return Result.Failure(ErrorCodeConstants.LanguageCodeRequired, await _systemMessageProvider.GetAsync(ErrorCodeConstants.LanguageCodeRequired));

            var existing = existingEntities.FirstOrDefault(x => x.Code.Equals(code));
            if (existing == null)
            {
                var entity = new Domain.Entities.Localization.Language(
                    code, 
                    name,
                    false,
                    _currentUser.UserId
                );

                await repo.AddAsync(entity, cancellationToken);
                existingEntities.Add(entity);
            }
            else
            {
                existing.Update(
                    code,
                    name
                );
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
