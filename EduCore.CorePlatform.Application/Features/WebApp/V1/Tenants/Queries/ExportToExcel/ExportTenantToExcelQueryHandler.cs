using ClosedXML.Excel;
using MediatR;
using Microsoft.Extensions.Options;
using EduCore.CorePlatform.Domain.Entities.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Enums.Tenants;
using EduCore.CorePlatform.Domain.Common.Settings.System;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.Tenants.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;


namespace EduCore.CorePlatform.Application.Features.Tenants.Queries.ExportToExcel;

public class ExportTenantToExcelQueryHandler : IRequestHandler<ExportTenantToExcelQuery, Result<byte[]>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;
    private readonly DataTypeFormatSettings _dataTypeFormatSettings;
    public ExportTenantToExcelQueryHandler(ICurrentTenant currentTenant,
                                                IUnitOfWork unitOfWork,
                                                ISystemMessageProvider systemMessageProvider,
                                                IOptions<DataTypeFormatSettings> dataTypeFormatSettings)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
        _dataTypeFormatSettings = dataTypeFormatSettings.Value;
    }

    public async Task<Result<byte[]>> Handle(ExportTenantToExcelQuery request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Tenant>();
        var spec = new TenantExportToExcelSpec(request.RequestData.SearchTerm);
        var data = await repo.ListAsync(spec, cancellationToken);

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Tenants");

        ws.Cell(1, 1).Value = "Code";
        ws.Cell(1, 2).Value = "Name";
        ws.Cell(1, 3).Value = "Description";
        ws.Cell(1, 4).Value = "Status";
        ws.Cell(1, 5).Value = "Email";
        ws.Cell(1, 6).Value = "Phone";
        ws.Cell(1, 7).Value = "Address";
        ws.Cell(1, 8).Value = "Website";
        ws.Cell(1, 9).Value = "Subscription Start Date";
        ws.Cell(1, 10).Value = "Subscription End Date";
        ws.Cell(1, 11).Value = "Max Users";
        ws.Cell(1, 12).Value = "Max StorageMB";
        ws.Cell(1, 13).Value = "TimeZone";


        int row = 2;
        foreach (var item in data)
        {
            ws.Cell(row, 1).Value = item.Code;
            ws.Cell(row, 2).Value = item.Name;
            ws.Cell(row, 3).Value = item.Description;

            if(item.Status == TenantStatus.Active)
            {
                ws.Cell(row, 4).Value = "Active";
            }    
            else if(item.Status == TenantStatus.Suspended)
            {
                ws.Cell(row, 4).Value = "Suspended";
            }
            else if (item.Status == TenantStatus.Pending)
            {
                ws.Cell(row, 4).Value = "Pending";
            }
            else if (item.Status == TenantStatus.Expired)
            {
                ws.Cell(row, 4).Value = "Expired";
            }
            else if (item.Status == TenantStatus.Deleted)
            {
                ws.Cell(row, 4).Value = "Deleted";
            }

            ws.Cell(row, 5).Value = item.Email;
            ws.Cell(row, 6).Value = item.Phone;
            ws.Cell(row, 7).Value = item.Address;
            ws.Cell(row, 8).Value = item.Website;
            ws.Cell(row, 9).Value = item.SubscriptionStartDate != null ? item.SubscriptionStartDate.Value.ToString(_dataTypeFormatSettings.DateFormat_MMyyyy) : string.Empty;
            ws.Cell(row, 10).Value = item.SubscriptionEndDate != null ? item.SubscriptionEndDate.Value.ToString(_dataTypeFormatSettings.DateFormat_MMyyyy) : string.Empty;
            ws.Cell(row, 11).Value = item.MaxUsers;
            ws.Cell(row, 12).Value = item.MaxStorageMB;
            ws.Cell(row, 13).Value = item.TimeZone;

            row++;
        }

        ws.Columns().AdjustToContents();


        using var stream = new MemoryStream();
        wb.SaveAs(stream);

        return Result<byte[]>.Success(stream.ToArray());
    }
}