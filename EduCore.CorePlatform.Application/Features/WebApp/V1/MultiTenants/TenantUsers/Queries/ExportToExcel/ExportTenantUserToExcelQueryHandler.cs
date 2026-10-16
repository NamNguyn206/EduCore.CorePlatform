using ClosedXML.Excel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Common.Constants.System;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Queries.ExportToExcel;

public class ExportTenantUserToExcelQueryHandler : IRequestHandler<ExportTenantUserToExcelQuery, Result<byte[]>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public ExportTenantUserToExcelQueryHandler(ICurrentTenant currentTenant,
                                                IUnitOfWork unitOfWork,
                                                ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<byte[]>> Handle(ExportTenantUserToExcelQuery request, CancellationToken cancellationToken)
    {
        var tenantId = _currentTenant.TenantId;
        if (tenantId == null)
        {
            return Result<byte[]>.Failure(ErrorCodeConstants.InvalidTenant, await _systemMessageProvider.GetAsync(ErrorCodeConstants.InvalidTenant));
        }

        var repo = _unitOfWork.Repository<Domain.Entities.Identity.User>();
        var spec = new TenantUserExportToExcelSpec(
            _currentTenant, 
            request.RequestData.SearchTerm,
            request.RequestData.RoleId
        );
        var data = await repo.ListAsync(spec, cancellationToken);

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Users");
        ws.Cell(1, 1).Value = "Email";
        ws.Cell(1, 2).Value = "FullName";
        ws.Cell(1, 3).Value = "Quyền quản trị Đơn vị/Tổ chức";

        int row = 2;
        foreach (var item in data)
        {
            ws.Cell(row, 1).Value = item.Email;
            ws.Cell(row, 2).Value = item.GetFullName();
            ws.Cell(row, 3).Value = item.UserTenants.Select(ut => ut.IsTenantAdmin).FirstOrDefault() == true ? "1": "0";

            row++;
        }

        ws.Columns().AdjustToContents();


        using var stream = new MemoryStream();
        wb.SaveAs(stream);

        return Result<byte[]>.Success(stream.ToArray());
    }
}