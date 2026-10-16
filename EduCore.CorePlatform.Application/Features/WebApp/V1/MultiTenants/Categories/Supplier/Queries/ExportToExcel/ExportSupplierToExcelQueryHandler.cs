using ClosedXML.Excel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Specifications.Queries;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Abstractions.Persistence;
using EduCore.CorePlatform.Application.Abstractions.Localization;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Supplier.Queries.ExportToExcel;

public sealed class ExportSupplierToExcelHandler
    : IRequestHandler<ExportSupplierToExcelQuery, Result<byte[]>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;
    public ExportSupplierToExcelHandler(
                                        IUnitOfWork unitOfWork,
                                        ICurrentTenant currentTenant,
                                        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<byte[]>> Handle(
        ExportSupplierToExcelQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<Domain.Entities.MultiTenants.Categories.Supplier>();

        var spec = new SupplierListWithSupplierAndBankSpec(_currentTenant, request.RequestData.SearchTerm);
        var suppliers = await repo.ListAsync(spec, cancellationToken);

        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Suppliers");

        ws.Cell(1, 1).Value = "Name";
        ws.Cell(1, 2).Value = "Address";
        ws.Cell(1, 3).Value = "TaxCode";
        ws.Cell(1, 4).Value = "Phone";
        ws.Cell(1, 5).Value = "Supply Category";
        ws.Cell(1, 6).Value = "Bank account";
        ws.Cell(1, 7).Value = "Bank name";
        ws.Cell(1, 8).Value = "Bank branch";
        ws.Cell(1, 9).Value = "Note";

        var row = 2;
        foreach (var s in suppliers)
        {
            ws.Cell(row, 1).Value = s.Name;
            ws.Cell(row, 2).Value = s.Address;
            ws.Cell(row, 3).Value = s.TaxCode;
            ws.Cell(row, 4).Value = s.Phone;
            ws.Cell(row, 5).Value = s.SupplyCategory != null ? s.SupplyCategory.Name : string.Empty;
            ws.Cell(row, 6).Value = s.BankAccount;
            ws.Cell(row, 7).Value = s.Bank != null ? s.Bank.Name : string.Empty;
            ws.Cell(row, 8).Value = s.BankBranch;
            ws.Cell(row, 9).Value = s.Note;
            row++;
        }

        ws.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return Result<byte[]>.Success(stream.ToArray());
    }
}
