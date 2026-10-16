using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2013.Excel;
using MediatR;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.System.SystemMessages.Specifications.Commands;
using EduCore.CorePlatform.Domain.Entities.Localization;
using EduCore.CorePlatform.Domain.Common.Enums.System;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Domain.Common.Constants.Localization;
using EduCore.CorePlatform.Domain.Entities.System.Messages;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.System.SystemMessages.Commands.ImportSystemMessageFromExcel;

public sealed class ImportSystemMessageFromExcelCommandHandler
    : IRequestHandler<ImportSystemMessageFromExcelCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemMessageProvider _systemMessageProvider;
    
    public ImportSystemMessageFromExcelCommandHandler(IUnitOfWork unitOfWork,
                                                    ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result> Handle(
        ImportSystemMessageFromExcelCommand request,
        CancellationToken cancellationToken)
    {
        if (request.FileBytes == null || request.FileBytes.Length == 0)
            return Result.Failure(ErrorCodeConstants.FileEmpty, await _systemMessageProvider.GetAsync(ErrorCodeConstants.FileEmpty));

        using var stream = new MemoryStream(request.FileBytes);
        using var workbook = new XLWorkbook(stream);

        var worksheet = workbook.Worksheets.First();
        var rows = worksheet.RowsUsed().Skip(1); // bỏ header

        var languageRepo = _unitOfWork.Repository<Language>();
        var messageRepo = _unitOfWork.Repository<SystemMessage>();

        var languages = await languageRepo.ListAsync(new LanguageListForImportSystemMessageFromExcelSpec(), cancellationToken);

        foreach (var row in rows)
        {
            var typeRaw = row.Cell(1).GetString().Trim();
            var code = row.Cell(2).GetString().Trim();
            var languageCodeRaw = row.Cell(3).GetString().Trim();
            var content = row.Cell(4).GetString().Trim();            

            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(content))
                continue;

            var languageCode = LanguageCodeConstants.Vi;
            var language = languages.FirstOrDefault(l => l.Code.Equals(languageCodeRaw, StringComparison.OrdinalIgnoreCase));

            if (language != null)
                languageCode = language.Code;

            if (!Enum.TryParse<SystemMessageType>(typeRaw, true, out var messageType))
                messageType = SystemMessageType.Info;

            var systemMessage = await _unitOfWork.SystemMessageWriteRepository
                                                .GetByCodeAsync(code, cancellationToken);
            if (systemMessage == null)
            {
                systemMessage = SystemMessage.Create(code, messageType);
                systemMessage.AddTranslation(languageCode, content);

                await _unitOfWork.SystemMessageWriteRepository
                                .AddAsync(systemMessage, cancellationToken);
            }
            else
            {
                if(systemMessage.Type != messageType)
                {
                    systemMessage.ChangeType(messageType);
                }

                systemMessage.UpsertTranslation(languageCode, content);
                _unitOfWork.SystemMessageWriteRepository.Update(systemMessage);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        
        return Result.Success();
    }
}
