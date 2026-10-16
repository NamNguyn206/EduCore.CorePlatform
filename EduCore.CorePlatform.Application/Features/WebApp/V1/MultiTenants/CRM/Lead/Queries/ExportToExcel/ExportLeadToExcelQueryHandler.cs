using ClosedXML.Excel;
using EduCore.CorePlatform.Application.Abstractions.Localization;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Specifications.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Queries.ExportToExcel;

public class ExportLeadToExcelQueryHandler : IRequestHandler<ExportLeadToExcelQuery, Result<byte[]>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public ExportLeadToExcelQueryHandler(
        ICurrentTenant currentTenant,
        IUnitOfWork unitOfWork,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<byte[]>> Handle(ExportLeadToExcelQuery request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.CRM.Lead>();
        var spec = new LeadExportToExcelSpec(_currentTenant, request.RequestData.SearchTerm);
        var data = await repo.ListAsync(spec, cancellationToken);

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Leads");

        // Đặt Header cho các cột
        ws.Cell(1, 1).Value = "Họ và Tên";
        ws.Cell(1, 2).Value = "Email";
        ws.Cell(1, 3).Value = "Số điện thoại";
        ws.Cell(1, 4).Value = "Điểm số";
        ws.Cell(1, 5).Value = "Dự đoán (%)";
        ws.Cell(1, 6).Value = "Mức độ";
        ws.Cell(1, 7).Value = "Hành động AI tiếp theo";

        int row = 2;
        foreach (var item in data)
        {
            ws.Cell(row, 1).Value = item.FullName;
            ws.Cell(row, 2).Value = item.Email ?? string.Empty;
            ws.Cell(row, 3).Value = item.PhoneNumber ?? string.Empty;
            ws.Cell(row, 4).Value = item.Score;
            ws.Cell(row, 5).Value = item.PredictionPercentage;
            ws.Cell(row, 6).Value = item.Temperature.ToString();
            ws.Cell(row, 7).Value = item.AiNextAction ?? string.Empty;

            row++;
        }

        ws.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        wb.SaveAs(stream);

        return Result<byte[]>.Success(stream.ToArray());
    }
}