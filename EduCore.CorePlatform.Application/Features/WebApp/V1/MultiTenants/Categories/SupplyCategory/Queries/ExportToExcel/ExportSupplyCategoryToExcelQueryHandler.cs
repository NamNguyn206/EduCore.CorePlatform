using ClosedXML.Excel;
using MediatR;
using System;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.SupplyCategory.Queries.ExportToExcel;

public class ExportSupplyCategoryToExcelQueryHandler: IRequestHandler<ExportSupplyCategoryToExcelQuery, Result<byte[]>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public ExportSupplyCategoryToExcelQueryHandler(ICurrentTenant currentTenant, 
                                                IUnitOfWork unitOfWork,
                                                ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<byte[]>> Handle(ExportSupplyCategoryToExcelQuery request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.SupplyCategory>();
        var spec = new SupplyCategoryExportToExcelSpec(_currentTenant, request.RequestData.SearchTerm);
        var data = await repo.ListAsync(spec, cancellationToken);

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("SupplyCategories");
        ws.Cell(1, 1).Value = "Name";

        int row = 2;
        foreach (var item in data)
        {
            ws.Cell(row, 1).Value = item.Name;
           
            row++;
        }

        ws.Columns().AdjustToContents();


        using var stream = new MemoryStream();
        wb.SaveAs(stream);

        return Result<byte[]>.Success(stream.ToArray());
    }
}